using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace DFC.Digital.Web.Core.HtmlExtentions
{
    public class AssetLocationAndVersion : IAssetLocationAndVersion
    {
        private readonly IConfigurationProvider configuration;
        private readonly IHttpClientService<IAssetLocationAndVersion> httpClientService;

        public AssetLocationAndVersion(IConfigurationProvider configuration, IHttpClientService<IAssetLocationAndVersion> httpClientService)
        {
            this.configuration = configuration;
            this.httpClientService = httpClientService;
        }

        private string CDNLocation => configuration.GetConfig<string>(Constants.CDNLocation);

        public string GetLocationAssetFileAndVersion(string fileName)
        {
            string version = null;
            AsyncHelper asyncHelper = new AsyncHelper();
            version = asyncHelper.Synchronise(() => GetFileHashAsync(fileName));
            return $"{CDNLocation}{fileName}?{version}";
        }

        private async Task<string> GetFileHashAsync(string fileName)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(CDNLocation);
            HttpResponseMessage response = await httpClient.GetAsync(fileName);
            if (response.IsSuccessStatusCode)
            {
                var hashCode = response.Content.Headers.GetValues("content-md5").FirstOrDefault();
                return hashCode.Replace("-", string.Empty);
            }

            //If we dont get a valid response use the current time to the nearest hour.
            return DateTime.Now.ToString("yyyyMMddHH");
        }
     }
}
