using DFC.Digital.Data.Interfaces;
using System.Configuration;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule
{
    public class JobProfileSearchIndexConfig : ISearchIndexConfig
    {
        public string Name => ConfigurationManager.AppSettings["DFC.Digital.JobProfileSearchIndex"]?.ToLower();

        public string AccessKey => ConfigurationManager.AppSettings.Get("DFC.Digital.DFC.Digital.SearchServiceQueryAPIKey");
    }
}