using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.Data.Events;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ISitefinityDataEventProxy
    {
      T GetPropertyValue<T>(IDataEvent eventInfo, string propertyName);
    }
}
