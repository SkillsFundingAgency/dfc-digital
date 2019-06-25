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
                Script = inPreviewMode ? string.Empty : nameof(JobProfileStructuredDataViewModel.Script)
            };

            //Act
            var htmlDocument = view.RenderAsHtml(viewModel);

            //Assert
            AssertContentStatus(htmlDocument, viewModel.Script);
        }

        private static void AssertContentStatus(HtmlDocument htmlDocument, string innerText)
        {
            htmlDocument = htmlDocument ?? new HtmlDocument();

            htmlDocument.DocumentNode.InnerHtml.Equals(innerText).Should().BeTrue();
        }
    }
}
