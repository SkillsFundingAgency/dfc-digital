using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Base
{
    public interface IContentPropertyConverter<T>
    {
        T ConvertFrom(DynamicContent content);
    }
}
