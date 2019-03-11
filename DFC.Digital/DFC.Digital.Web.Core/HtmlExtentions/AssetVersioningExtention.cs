using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web.Caching;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class AssetVersioningExtention
    {
       //private static HttpClient httpClient = new HttpClient();
        public static string GetLocationAssetFileAndVersion(this HtmlHelper helper, string fileName)
        {
            var context = helper?.ViewContext.RequestContext.HttpContext;
            string version = null;

            if (context.Cache[fileName] == null)
            {
                AsyncHelper asyncHelper = new AsyncHelper();

                version = asyncHelper.Synchronise(() => GetFileHashAsync(fileName));
                int cacheExpiryMins = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.AssetCacheExpiryTimeMins]);
                context.Cache.Add(fileName, version, null, DateTime.Now.AddMinutes(cacheExpiryMins), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
            else
            {
                version = context.Cache[fileName] as string;
            }

            return $"{ConfigurationManager.AppSettings[Constants.CDNLocation]}{fileName}?{version}";
        }

        public static string GetLocationAssetFile(this HtmlHelper helper, string fileName)
        {
            return $"{ConfigurationManager.AppSettings[Constants.CDNLocation]}{fileName}";
        }

        private static async Task<string> GetFileHashAsync(string fileName)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings[Constants.CDNLocation]);
            HttpResponseMessage response = await httpClient.GetAsync(fileName);
            if (response.IsSuccessStatusCode)
            {
                var hashCodes = response.Content.Headers.GetValues("content-md5").FirstOrDefault();
                return hashCodes;
            }

            //If we dont get a valid response use the current time to the nearest hour.
            return DateTime.Now.ToString("yyyyMMddHH");
        }

        private static string GetFileHashOrld(string file)
        {
            MD5 md5 = MD5.Create();
            using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
            }
        }
    }
}