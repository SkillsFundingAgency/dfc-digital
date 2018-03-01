using DFC.Digital.Core;
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Web.Caching;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core.HtmlExtentions
{
    public static class AssetVersioningExtention
    {
        public static string VersionAssetFile(this HtmlHelper helper, string fileName)
        {
            var context = helper.ViewContext.RequestContext.HttpContext;
            string version = null;

            if (context.Cache[fileName] == null)
            {
                var physicalPath = context.Server.MapPath(fileName);
                version = GetFileHash(physicalPath);
                int cacheExpiryMins = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.AssetCacheExpiryTimeMins]);
                context.Cache.Add(fileName, version, null, DateTime.Now.AddMinutes(cacheExpiryMins), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }
            else
            {
                version = context.Cache[fileName] as string;
            }

            return $"{fileName}?{version}";
        }

        private static string GetFileHash(string file)
        {
            MD5 md5 = MD5.Create();
            using (FileStream stream = new FileStream(file, FileMode.Open, FileAccess.Read))
            {
                return BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", string.Empty);
            }
        }
    }
}