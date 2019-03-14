using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class EmailTemplateRepository : IEmailTemplateRepository
    {
        #region Fields
        private readonly IDynamicModuleRepository<EmailTemplate> emailTemplateRepository;
        private readonly IDynamicModuleConverter<EmailTemplate> converter;
        #endregion Fields

        #region Ctor
        public EmailTemplateRepository(
          IDynamicModuleRepository<EmailTemplate> emailTemplateRepository,
          IDynamicModuleConverter<EmailTemplate> converter)
        {
            this.emailTemplateRepository = emailTemplateRepository;
            this.converter = converter;
        }
        #endregion Ctor

        #region IEmailTemplateRepository Implementations
        public EmailTemplate GetByTemplateName(string templateName)
        {
            return ConvertDynamicContent(emailTemplateRepository.Get(item =>
                  item.UrlName == templateName && item.Status == ContentLifecycleStatus.Live && item.Visible));
        }

        public Type GetContentType()
        {
            return emailTemplateRepository.GetContentType();
        }

        public string GetProviderName()
        {
            return emailTemplateRepository.GetProviderName();
        }

        private EmailTemplate ConvertDynamicContent(DynamicContent dynamicContent)
        {
            if (dynamicContent != null)
            {
                return converter.ConvertFrom(dynamicContent);
            }

            return null;
        }

        #endregion IEmailTemplateRepositoryRepository Implementations
    }
}
