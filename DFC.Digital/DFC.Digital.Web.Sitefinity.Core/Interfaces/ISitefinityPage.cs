using DFC.Digital.Core.Interceptors;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using Telerik.Sitefinity.Mvc.Proxy;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public interface ISitefinityPage
    {
        [IgnoreOutputInInterception]
        ControlData GetControlOnPage(Guid id);

        [IgnoreOutputInInterception]
        ControlData GetNextControlOnPage(Guid siblingId);

        IEnumerable<Guid> GetControlOnPageByCaption(IEnumerable<string> captionFilter);

        IEnumerable<Guid> GetControlsInOrder(IEnumerable<string> sectionFilter);

        [IgnoreOutputInInterception]
        IEnumerable<KeyValuePair<string, MvcControllerProxy>> GetWidgets(IEnumerable<Guid> controls);

        [IgnoreOutputInInterception]
        PageDraft GetContextPagePreview();

        [IgnoreOutputInInterception]
        IEnumerable<KeyValuePair<string, string>> GetPagePreviewByUrlName(string urlName);

        [IgnoreInputInInterception]
        JobProfileSection GetJobProfileSectionFromWidget(JobProfileSectionFilter sectionFilter, KeyValuePair<string, MvcControllerProxy> widget);

        string ReadSettingFromControl(string controlName, string settingName);

        string GetDefaultJobProfileToUse(string widgetJobProfileDefault);
    }
}