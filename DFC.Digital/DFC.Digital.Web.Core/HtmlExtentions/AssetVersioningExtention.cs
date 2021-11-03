using Autofac;
using Autofac.Integration.Mvc;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Web.Caching;
using System.Web.Mvc;

namespace DFC.Digital.Web.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static class AssetVersioningExtention
    {
        public static string GetLocationAssetFileAndVersion(this HtmlHelper helper, string fileName)
        {
            var context = helper?.ViewContext.RequestContext.HttpContext;
            if (context.Cache[fileName] is null)
            {
                var autofacLifetimeScope = AutofacDependencyResolver.Current.RequestLifetimeScope;
                var htmlHelper = autofacLifetimeScope.ResolveOptional<IAssetLocationAndVersion>();
                var assetUrl = htmlHelper?.GetLocationAssetFileAndVersion(fileName);
                int cacheExpiryMins = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.AssetCacheExpiryTimeMins]);
                if (assetUrl is null)
                {
                    return fileName;
                }
                else
                {
                    context.Cache.Add(fileName, assetUrl, null, DateTime.Now.AddMinutes(cacheExpiryMins), TimeSpan.Zero, CacheItemPriority.Normal, null);
                }
            }

            return context.Cache[fileName].ToString();
        }

        public static string GetMetaTag(this HtmlHelper helper)
        {
                return ConfigurationManager.AppSettings[Constants.MetaTag];
        }
    }
}