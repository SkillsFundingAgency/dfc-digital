using ASP;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests.Views
{
    public class JobProfileStructuredDataViewTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Dfc9493ScriptTagsOnJobProfilePagesViewTests(bool inPreviewMode)
        {
            //Assign
            var view = new _MVC_Views_JobProfileStructuredData_Index_cshtml();
            var viewModel = new JobProfileStructuredDataViewModel
            {
                InPreviewMode = inPreviewMode,
                Script = nameof(JobProfileStructuredDataViewModel.Script),
                DemoScript = nameof(JobProfileStructuredDataViewModel.DemoScript)
            };

            //Act
            var htmlDocument = view.RenderAsHtml(viewModel);

            //Assert
            if (inPreviewMode)
            {
                AssertContentStatus(htmlDocument, true, viewModel.DemoScript);
                AssertContentStatus(htmlDocument, false, viewModel.Script);
            }
            else
            {
                AssertContentStatus(htmlDocument, true, viewModel.Script);
                AssertContentStatus(htmlDocument, false, viewModel.DemoScript);
            }
        }

        private static void AssertContentStatus(HtmlDocument htmlDocument, bool exists, string innerText)
        {
            htmlDocument = htmlDocument ?? new HtmlDocument();

            htmlDocument.DocumentNode.InnerHtml.Equals(innerText).Should().Be(exists);
        }
    }
}
