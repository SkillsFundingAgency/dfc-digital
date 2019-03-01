using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly IDynamicModuleRepository<Configuration> configurationRepository;
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public ConfigurationRepository(
          IDynamicModuleRepository<Configuration> configurationRepository,
          IDynamicContentExtensions dynamicContentExtensions)
        {
            this.configurationRepository = configurationRepository;
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public Configuration GetByTemplateName(string templateName)
        {
            return new Configuration();
        }

        public Type GetContentType()
        {
            throw new NotImplementedException();
        }

        public string GetProviderName()
        {
            throw new NotImplementedException();
        }
    }
}
