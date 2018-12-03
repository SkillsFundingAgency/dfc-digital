﻿using DFC.Digital.Data.Interfaces;
using System.Configuration;

namespace DFC.Digital.Service.AzureSearch.IntegrationTests
{
    public class JobProfileIntegrationTestIndex : ISearchIndexConfig
    {
        public string Name => ConfigurationManager.AppSettings.Get("DFC.Digital.JobProfileSearchIndex");

        public string AccessKey => ConfigurationManager.AppSettings.Get("DFC.Digital.SearchServiceAdminAPIKey");
    }
}