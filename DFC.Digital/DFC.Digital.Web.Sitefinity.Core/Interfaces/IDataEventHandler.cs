using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using Telerik.Sitefinity.Data.Events;
using Telerik.Sitefinity.Modules.Pages;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface IDataEventHandler
    {
        void ExportCompositePage(IDataEvent eventInfo);
    }
}
