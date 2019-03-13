using Autofac;
using Autofac.Integration.Mvc;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using System;
using System.Configuration;
using System.Web;
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
                var hTMLHelper = autofacLifetimeScope.Resolve<IAssetLocationAndVersion>();
                var assetUrl = hTMLHelper.GetLocationAssetFileAndVersion(fileName);
                int cacheExpiryMins = Convert.ToInt32(ConfigurationManager.AppSettings[Constants.AssetCacheExpiryTimeMins]);
                context.Cache.Add(fileName, assetUrl, null, DateTime.Now.AddMinutes(cacheExpiryMins), TimeSpan.Zero, CacheItemPriority.Normal, null);
            }

            return context.Cache[fileName].ToString();
        }
    }
}