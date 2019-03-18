namespace DFC.Digital.Web.Sitefinity.ContactUsModule.UnitTests.Views
{
    using ASP;
    using DFC.Digital.Web.Sitefinity.ContactUsModule.Mvc.Models;
    using RazorGenerator.Testing;
    using Xunit;

    public class TechnicalViewTests
    {
        [Fact]
        public void TechnicalViewTest()
        {
            // Arrange
            var technicalIndex = new _MVC_Views_Technical_Index_cshtml();
            var contactUsTechnicalViewModel = new ContactUsTechnicalViewModel() { Message = "Dummy message" };

            // Act
            var htmlDocument = technicalIndex.RenderAsHtml(contactUsTechnicalViewModel);

            //Asserts

            htmlDocument.DocumentNode.DescendantNodes("");



        }
    }
}
