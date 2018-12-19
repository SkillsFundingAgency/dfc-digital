﻿using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using System.Configuration;

namespace DFC.Digital.NonUIAcceptanceTest
{
    public class JobProfileIntegrationTestIndex : ISearchIndexConfig
    {
        public string Name => ConfigurationManager.AppSettings.Get(Constants.JobProfileSearchIndex);

        public string AccessKey => ConfigurationManager.AppSettings.Get(Constants.SearchServiceQueryAPIKey);

    }
}