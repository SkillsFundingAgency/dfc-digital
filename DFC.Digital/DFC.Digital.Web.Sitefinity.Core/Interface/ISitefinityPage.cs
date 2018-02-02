using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using Telerik.Sitefinity.Mvc.Proxy;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core.Interface
{
    public interface ISitefinityPage
    {
        ControlData GetControlOnPage(Guid id);

        ControlData GetNextControlOnPage(Guid siblingId);

        IEnumerable<Guid> GetControlOnPageByCaption(IEnumerable<string> captionFilter);

        IEnumerable<Guid> GetControlsInOrder(IEnumerable<string> sectionFilter);

        IEnumerable<KeyValuePair<string, MvcControllerProxy>> GetWidgets(IEnumerable<Guid> controls);

        PageDraft GetContextPagePreview();

        JobProfileSection GetJobProfileSectionFromWidget(JobProfileSectionFilter sectionFilter, KeyValuePair<string, MvcControllerProxy> widget);

        string ReadSettingFromControl(string controlName, string settingName);

        string GetDefaultJobProfileToUse(string widgetJobProfileDefault);
    }
}