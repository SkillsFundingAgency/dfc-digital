using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IConfigurationRepository
    {
        Configuration GetByTemplateName(string urlName);

        string GetProviderName();

        Type GetContentType();
    }
}
