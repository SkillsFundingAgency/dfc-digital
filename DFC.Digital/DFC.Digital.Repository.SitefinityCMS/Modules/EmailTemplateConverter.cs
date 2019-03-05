using DFC.Digital.Core;
using DFC.Digital.Core.Logging;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class EmailTemplateConverter : IDynamicModuleConverter<EmailTemplate>
    {
        #region Fields
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        #endregion Fields
        #region Ctor

        public EmailTemplateConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        #endregion Ctor

        public EmailTemplate ConvertFrom(DynamicContent content)
        {
            var emailTemplate = new EmailTemplate
            {
                TemplateName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(EmailTemplate.TemplateName)),
                From = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(EmailTemplate.From)),
                To = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(EmailTemplate.To)),
                Subject = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(EmailTemplate.Subject)),
                Body = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(EmailTemplate.Body))
            };

            return emailTemplate;
        }
    }
}
