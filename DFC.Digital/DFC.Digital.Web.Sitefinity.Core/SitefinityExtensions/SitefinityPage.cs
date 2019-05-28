using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Mvc.Proxy;
using Telerik.Sitefinity.Pages.Model;

namespace DFC.Digital.Web.Sitefinity.Core
{
    public class SitefinityPage : ISitefinityPage
    {
        public SitefinityPage(IWebAppContext context, ISitefinityCurrentContext sitefinityContext)
        {
            this.Context = context;
            this.SitefinityContext = sitefinityContext;
        }

        public IWebAppContext Context { get; }

        public ISitefinityCurrentContext SitefinityContext { get; }

        public PageDraft GetContextPagePreview()
        {
            return this.SitefinityContext.CurrentPageManager.GetPreview(this.SitefinityContext.CurrentPage.Id);
        }

        public PageDraft GetPagePreviewByUrlName(string urlName)
        {
            var pageNode = SitefinityContext.CurrentPageManager.GetPageNodes().FirstOrDefault(page => page.UrlName == urlName);

            if (pageNode != null)
            {
                return SitefinityContext.CurrentPageManager.GetPreview(pageNode.Id);
            }

            return null;
        }

        public IEnumerable<Guid> GetControlsInOrder(IEnumerable<string> sectionFilter)
        {
            var firstControlsIDs = Context.IsPreviewMode ? GetContextPagePreview().Controls.Where(x => x.SiblingId.Equals(Guid.Empty)).Select(x => x.Id) : SitefinityContext.CurrentPage.Controls.Where(x => x.SiblingId.Equals(Guid.Empty)).Select(x => x.Id);
            foreach (var id in firstControlsIDs)
            {
                var nextControl = GetNextControlOnPage(id);
                while (nextControl != null)
                {
                    if (sectionFilter.Any(js => js.Equals(nextControl.Caption, StringComparison.InvariantCultureIgnoreCase)))
                    {
                        yield return nextControl.Id;
                    }

                    nextControl = GetNextControlOnPage(nextControl.Id);
                }
            }
        }

        public ControlData GetNextControlOnPage(Guid siblingId)
        {
            if (Context.IsPreviewMode)
            {
                return GetContextPagePreview().Controls.FirstOrDefault(x => x.SiblingId.Equals(siblingId));
            }
            else
            {
                return SitefinityContext.CurrentPage.Controls.FirstOrDefault(x => x.SiblingId.Equals(siblingId));
            }
        }

        public ControlData GetControlOnPageByCaption(string captionTitle)
        {
            if (Context.IsPreviewMode)
            {
                return GetContextPagePreview().Controls.FirstOrDefault(x => x.Caption.Equals(captionTitle, StringComparison.InvariantCultureIgnoreCase));
            }
            else
            {
                return SitefinityContext.CurrentPage.Controls.FirstOrDefault(x => x.Caption.Equals(captionTitle, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        public ControlData GetControlOnPage(Guid id)
        {
            if (Context.IsPreviewMode)
            {
                return GetContextPagePreview().Controls.FirstOrDefault(x => x.Id.Equals(id));
            }
            else
            {
                return SitefinityContext.CurrentPage.Controls.FirstOrDefault(x => x.Id.Equals(id));
            }
        }

        public IEnumerable<KeyValuePair<string, MvcControllerProxy>> GetWidgets(IEnumerable<Guid> controls)
        {
            return controls
                .Select(c =>
                {
                    var control = GetControlOnPage(c);
                    return new KeyValuePair<string, MvcControllerProxy>(
                        control.Caption,
                        SitefinityContext.CurrentPageManager.LoadControl(control) as MvcControllerProxy);
                });
        }

        public IEnumerable<Guid> GetControlOnPageByCaption(IEnumerable<string> captionFilter)
        {
            return SitefinityContext.CurrentPage.Controls.Where(control => captionFilter.Contains(control.Caption))
                ?.Select(control => control.Id);
        }

        public JobProfileSection GetJobProfileSectionFromWidget(JobProfileSectionFilter sectionFilter, KeyValuePair<string, MvcControllerProxy> widget)
        {
            var titleMember = sectionFilter?.TitleMember;
            var contentFieldMember = sectionFilter?.ContentFieldMember;
            Dictionary<string, object> settings = widget.Value.Settings.Values;
            return new JobProfileSection
            {
                Title = settings[titleMember]?.ToString(),
                ContentField = settings[contentFieldMember]?.ToString(),
            };
        }

        public string ReadSettingFromControl(string controlName, string settingName)
        {
            var settingWidgetControl = SitefinityContext.CurrentPageManager.EditPage(SitefinityContext.CurrentPage.Id).
                                       Controls.FirstOrDefault(c => c.Caption.Equals(controlName));

            // if we find the settings widget
            if (settingWidgetControl != null)
            {
                var widgetSetting = settingWidgetControl.Properties.Where(p => p.Name.Equals("Settings")).FirstOrDefault();
                if (widgetSetting != null)
                {
                    var settingsField = settingWidgetControl.Properties.Where(p => p.Name.Equals("Settings")).FirstOrDefault().
                        ChildProperties.Where(n => n.Name.Equals(settingName)).FirstOrDefault();

                    if (settingsField != null)
                    {
                        return settingsField.Value;
                    }
                }
            }

            return null;
        }

        public string GetDefaultJobProfileToUse(string widgetJobProfileDefault)
        {
            var globalJobProfileName = ReadSettingFromControl(SitefinityConstants.JobProfileSettingsWidget, SitefinityConstants.DefaultJobProfileUrlName);

            if (globalJobProfileName != null)
            {
                return globalJobProfileName;
            }

            return widgetJobProfileDefault;
        }
    }
}