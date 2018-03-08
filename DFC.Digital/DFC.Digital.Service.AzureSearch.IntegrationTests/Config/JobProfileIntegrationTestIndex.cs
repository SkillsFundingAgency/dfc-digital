using DFC.Digital.Data.Interfaces; using DFC.Digital.Core;
using System.Configuration;

namespace DFC.Digital.Service.AzureSearch.IntegrationTests.Config
{
    internal class JobProfileIntegrationTestIndex : ISearchIndexConfig
    {
        public string Name => ConfigurationManager.AppSettings.Get("DFC.Digital.JobProfileSearchIndex");
    }
}