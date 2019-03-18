using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class AssetLocationAndVersion : IAssetLocationAndVersion
    {
        private const string ContentMDS = "content-md5";
        private readonly IConfigurationProvider configuration;
        private readonly IHttpClientService<IAssetLocationAndVersion> httpClientService;
        private readonly IAsyncHelper asyncHelper;

        public AssetLocationAndVersion(IConfigurationProvider configuration, IHttpClientService<IAssetLocationAndVersion> httpClientService, IAsyncHelper asyncHelper)
        {
            this.configuration = configuration;
            this.httpClientService = httpClientService;
            this.asyncHelper = asyncHelper;
        }

        private string CDNLocation => configuration.GetConfig<string>(Constants.CDNLocation);

        public string GetLocationAssetFileAndVersion(string fileName)
        {
            string assetLocation = $"{CDNLocation}/{fileName}";
            string version = asyncHelper.Synchronise(() => GetFileHashAsync(assetLocation));
            return $"{assetLocation}?{version}";
        }

        private async Task<string> GetFileHashAsync(string assetLocation)
        {
            HttpResponseMessage response = await httpClientService.GetAsync(assetLocation);
            if (response.IsSuccessStatusCode)
            {
                var hashCode = response.Content.Headers.GetValues(ContentMDS).FirstOrDefault();
                return hashCode.Replace("-", string.Empty);
            }

            //If we dont get a valid response use the current time to the nearest hour.
            return DateTime.Now.ToString("yyyyMMddHH");
        }
     }
}
