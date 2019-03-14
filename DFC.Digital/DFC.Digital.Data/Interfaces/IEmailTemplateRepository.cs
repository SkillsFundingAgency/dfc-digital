using DFC.Digital.Data.Model;
using System;

namespace DFC.Digital.Data.Interfaces
{
    public interface IEmailTemplateRepository
    {
        EmailTemplate GetByTemplateName(string templateName);

        string GetProviderName();

        Type GetContentType();
    }
}
