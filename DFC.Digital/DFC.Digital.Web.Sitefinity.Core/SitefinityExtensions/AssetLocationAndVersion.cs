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
        private readonly IApplicationLogger applicationLogger;

        public AssetLocationAndVersion(
            IConfigurationProvider configuration,
            IHttpClientService<IAssetLocationAndVersion> httpClientService,
            IAsyncHelper asyncHelper,
            IWebAppContext context,
            IApplicationLogger applicationLogger)
        {
            this.configuration = configuration;
            this.httpClientService = httpClientService;
            this.asyncHelper = asyncHelper;
            this.context = context;
            this.applicationLogger = applicationLogger;
        }

        private string CDNLocation => configuration.GetConfig<string>(Constants.CDNLocation);

        private string WebChatEndPoint => configuration.GetConfig<string>(Constants.WebChatEndPoint);

        public string GetLocationAssetFileAndVersion(string fileName)
        {
            if (fileName == Constants.WebChatEndPoint)
            {
                if (!string.IsNullOrEmpty(WebChatEndPoint))
                {
                    var assetLocation = $"{WebChatEndPoint}";
                    var version = asyncHelper.Synchronise(() => GetFileHashAsync(assetLocation));
                    return $"{assetLocation}?{version}";
                }
            }

            if (!string.IsNullOrEmpty(CDNLocation))
            {
                var assetLocation = $"{CDNLocation}/{fileName}";
                var version = asyncHelper.Synchronise(() => GetFileHashAsync(assetLocation));
                return $"{assetLocation}?{version}";
            }
            else
            {
                var assetLocation = $"/ResourcePackages/{fileName.Substring(0, fileName.IndexOf("/"))}/assets/dist/{fileName.Substring(fileName.IndexOf("/") + 1)}";
                var physicalPath = context.ServerMapPath($"~{assetLocation}");
                var version = GetFileHash(physicalPath);
                return $"{assetLocation}?{version}";
            }
        }

        private static string GetFileHash(string file)
        {
            if (File.Exists(file))
            {
                var md5 = MD5.Create();
                using (var stream = new FileStream(file, FileMode.Open, FileAccess.Read))
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
            try
            {
                var response = await httpClientService.GetAsync(assetLocation);
                if (response.IsSuccessStatusCode)
                {
                    var hashCode = response.Content.Headers.GetValues(ContentMDS).FirstOrDefault();
                    return !string.IsNullOrWhiteSpace(hashCode) ? hashCode.Replace("-", string.Empty) : DateTime.Now.ToString("yyyyMMddHH");
                }
            }
            catch (Exception ex)
            {
                applicationLogger.ErrorJustLogIt("Failed to get file hash", ex);
            }

            //If we dont get a valid response use the current time to the nearest hour.
            return DateTime.Now.ToString("yyyyMMddHH");
        }
    }
}