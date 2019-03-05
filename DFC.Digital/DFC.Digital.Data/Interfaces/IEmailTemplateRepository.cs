using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IEmailTemplateRepository
    {
        EmailTemplate GetByTemplateName(string urlName);

        string GetProviderName();

        Type GetContentType();
    }
}
