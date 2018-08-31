﻿using DFC.Digital.Core.Interceptors;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS
{
    public interface IContentPropertyConverter<T>
    {
        [IgnoreInputInInterception]
        T ConvertFrom(DynamicContent content);
    }
}
