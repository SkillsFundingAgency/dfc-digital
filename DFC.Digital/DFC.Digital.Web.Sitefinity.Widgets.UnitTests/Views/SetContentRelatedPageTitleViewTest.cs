using ASP;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using FluentAssertions;
using RazorGenerator.Testing;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Widgets.UnitTests.Views
{
    public class SetContentRelatedPageTitleViewTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CheckViewIsOnlyDisplayedInDesignMode(bool inDesignMode)
        {
            // Arrange
            var indexView = new _MVC_Views_SetContentRelatedPageTitle_Index_cshtml();
            var setContentRelatedPageTitleModel = new SetContentRelatedPageTitleModel { DisplayView = inDesignMode };

            //// Act
            var htmlDom = indexView.RenderAsHtml(setContentRelatedPageTitleModel);

            //// Check that when not in design mode we do not display anything
            if (!inDesignMode)
            {
                htmlDom.DocumentNode.InnerHtml.ShouldBeEquivalentTo(string.Empty);
            }
            else
            {
                //should display the information message in design mode
                var sectionText = htmlDom.DocumentNode.SelectNodes("//p").FirstOrDefault().InnerText;
                sectionText.Should().Contain("Placing this widget");
            }
        }
    }
}