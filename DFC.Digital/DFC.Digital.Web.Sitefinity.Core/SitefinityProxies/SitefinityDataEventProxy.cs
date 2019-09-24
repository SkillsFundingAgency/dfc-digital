using System;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.DynamicModules.Events;
using Telerik.Sitefinity.Publishing;

namespace DFC.Digital.Web.Sitefinity.Core
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class SitefinityDataEventProxy : ISitefinityDataEventProxy
    {
        public T GetDynamicContentPropertyValue<T>(IDynamicContentUpdatedEvent eventInfo, string propertyName)
        {
            return eventInfo.GetPropertyValue<T>(propertyName);
        }

        public T GetPropertyValue<T>(IDataEvent eventInfo, string propertyName)
        {
            return eventInfo.GetPropertyValue<T>(propertyName);
        }
    }
}