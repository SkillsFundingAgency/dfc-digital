using System;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.Publishing;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class SitefinityDataEventProxy : ISitefinityDataEventProxy
    {
        public T GetPropertyValue<T>(IDataEvent eventInfo, string propertyName)
        {
            return eventInfo.GetPropertyValue<T>(propertyName);
        }
    }
}