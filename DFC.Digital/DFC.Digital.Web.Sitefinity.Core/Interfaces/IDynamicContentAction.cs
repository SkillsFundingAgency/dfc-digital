using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Web.Sitefinity.Core.Interfaces
{
    public interface IDynamicContentAction
    {
        MessageAction GetDynamicContentEventAction(DynamicContent item);
    }
}
