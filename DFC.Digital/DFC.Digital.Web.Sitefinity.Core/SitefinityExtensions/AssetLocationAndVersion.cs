using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class AssetLocationAndVersion : IAssetLocationAndVersion
    {
        private const string ContentMDS = "content-md5";
        private readonly IConfigurationProvider configuration;
        private readonly IHttpClientService<IAssetLocationAndVersion> httpClientService;
        private readonly IAsyncHelper asyncHelper;
        private readonly IWebAppContext context;

        public AssetLocationAndVersion(
            IConfigurationProvider configuration,
            IHttpClientService<IAssetLocationAndVersion> httpClientService,
            IAsyncHelper asyncHelper,
            IWebAppContext context)
        {
            this.configuration = configuration;
            this.httpClientService = httpClientService;
            this.asyncHelper = asyncHelper;
            this.context = context;
        }

        private string CDNLocation => configuration.GetConfig<string>(Constants.CDNLocation);

        public string GetLocationAssetFileAndVersion(string fileName)
        {
            if (!string.IsNullOrEmpty(CDNLocation))
            {
                string assetLocation = $"{CDNLocation}/{fileName}";
                string version = asyncHelper.Synchronise(() => GetFileHashAsync(assetLocation));
                return $"{assetLocation}?{version}";
            }
            else
            {
                string assetLocation = $"ResourcePackages/{fileName.Substring(0, fileName.IndexOf("/"))}/assets/dist/{fileName.Substring(fileName.IndexOf("/") + 1)}";
                var physicalPath = context.ServerMapPath($"~/{assetLocation}");
                var version = GetFileHash(physicalPath);
                return $"{assetLocation}?{version}";
            }
        }

        private static string GetFileHash(string file)
        {
            if (File.Exists(file))
            {
                MD5 md5 = MD5.Create();
                using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
                }
            }
            else
            {
                return string.Empty;
            }
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