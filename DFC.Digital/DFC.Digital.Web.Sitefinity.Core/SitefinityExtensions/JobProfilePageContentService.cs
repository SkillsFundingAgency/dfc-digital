using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.Mvc.Proxy;

namespace DFC.Digital.Web.Sitefinity.Core
{
    /// <summary>
    /// Sitefinity Page Content Service
    /// </summary>
    /// <seealso cref="IJobProfilePage" />
    public class JobProfilePageContentService : IJobProfilePage
    {
        public JobProfilePageContentService(ISitefinityPage sitefinityPage)
        {
            SitefinityPage = sitefinityPage;
        }

        public ISitefinityPage SitefinityPage { get; }

        /// <summary>
        /// Gets the job profile anchor links.
        /// </summary>
        /// <param name="sectionFilters">The section filters.</param>
        /// <returns>JobProfile Sections</returns>
        public IEnumerable<JobProfileSection> GetJobProfileSections(IEnumerable<JobProfileSectionFilter> sectionFilters)
        {
            var controls = SitefinityPage.GetControlsInOrder(sectionFilters.Select(f => f.SectionCaption));

            //We had to preserve the order while selecting the widgets or the child widget
            var widgets = SitefinityPage.GetWidgets(controls)
                .Select(widget =>
                {
                    var sectionFilter = sectionFilters.First(f => f.SectionCaption.Equals(widget.Key));

                    //If there are any sub filters then we are only Interested in first child
                    if (sectionFilter.SubFilters.Any())
                    {
                        return GetFirstMatchedJobProfileSection(sectionFilter);
                    }
                    else
                    {
                        return GetJobProfileSectionFromWidget(sectionFilter, widget);
                    }
                });

            return widgets.Where(w => w != null);
        }

        public JobProfileSection GetFirstMatchedJobProfileSection(JobProfileSectionFilter sectionFilter)
        {
            var subControls = SitefinityPage.GetControlOnPageByCaption(sectionFilter.SubFilters);
            var widgetChildren = SitefinityPage.GetWidgets(subControls);

            return widgetChildren
                .Select(w => GetJobProfileSectionFromWidget(sectionFilter, w))
                .FirstOrDefault(child => !string.IsNullOrEmpty(child.Title) && !string.IsNullOrEmpty(child.ContentField));
        }

        private static JobProfileSection GetJobProfileSectionFromWidget(JobProfileSectionFilter sectionFilter, KeyValuePair<string, MvcControllerProxy> widget)
        {
            var titleMember = sectionFilter.TitleMember;
            var contentFieldMember = sectionFilter.ContentFieldMember;
            Dictionary<string, object> settings = widget.Value.Settings.Values;
            return new JobProfileSection
            {
                Title = settings[titleMember]?.ToString(),
                ContentField = settings[contentFieldMember]?.ToString(),
            };
        }
    }
}