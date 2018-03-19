using ASP;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using RazorGenerator.Testing;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class JobProfileSettingsAndPreviewViewTests
    {
        [Fact]
        public void SettingOnJobProfilesTest()
        {
            var index = new _MVC_Views_JobProfileSettingsAndPreview_Index_cshtml();
            var jobProfileSettingsAndPreviewModel = GenerateViewModel();
            var htmlDom = index.RenderAsHtml(jobProfileSettingsAndPreviewModel);

            var sectionText = htmlDom.DocumentNode.SelectNodes("//h2[contains(@class, 'heading-medium')]").FirstOrDefault().InnerText;

            //all we can check is that the view is displayed.
            //none of the logic is controlled by the model.
            sectionText.EndsWith("DefaultProfile", System.StringComparison.Ordinal);
        }

        private JobProfileSettingsAndPreviewModel GenerateViewModel()
        {
            return new JobProfileSettingsAndPreviewModel() { DefaultJobProfileUrl = "DefaultProfile" };
        }
    }
}