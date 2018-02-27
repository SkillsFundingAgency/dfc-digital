using DFC.Digital.Core.Utilities;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System.Text.RegularExpressions;
using System.Web;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Widgets.UnitTests.Controllers
{
    public class BauSearchResultsSignPostControllerTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("test")]
        public void IndexTest(string searchTerm)
        {
            //Setup the fakes and dummies
            var loggerFake = A.Fake<IApplicationLogger>();
            var contentValue = "http://www.search.com?searcherm={0}";

            // Set up calls
            var modelContent = string.Format(contentValue, searchTerm);

            //Instantiate & Act
            var bauSearchResultsSignPostController = new BauSearchResultsSignPostController(loggerFake)
            {
                BannerContent = contentValue
            };

            //Act
            var indexMethodCall = bauSearchResultsSignPostController.WithCallTo(c => c.Index(searchTerm));

            //Assert
            indexMethodCall
                .ShouldRenderDefaultView().WithModel<BauSearchResultsViewModel>(vm =>
                {
                    vm.Content.ShouldBeEquivalentTo(modelContent);
                })
                .AndNoModelErrors();
        }

        [Theory]
        [InlineData("<nur se>.-+:'’?,/&")]
        public void IndexSpecialCharactersTest(string searchTerm)
        {
            //Setup the fakes and dummies
            var loggerFake = A.Fake<IApplicationLogger>();
            var contentValue = "http://www.search.com?searcherm={0}";

            // Set up calls
            var modelContent = string.Format(contentValue, HttpUtility.UrlEncode(Regex.Replace(searchTerm, Constants.ValidBAUSearchCharacters, string.Empty)));

            //Instantiate & Act
            using (var bauSearchResultsSignPostController = new BauSearchResultsSignPostController(loggerFake)
            {
                BannerContent = contentValue
            })
            {
                //Act
                var indexMethodCall = bauSearchResultsSignPostController.WithCallTo(c => c.Index(searchTerm));

                //Assert
                indexMethodCall
                    .ShouldRenderDefaultView().WithModel<BauSearchResultsViewModel>(vm =>
                    {
                        vm.Content.ShouldBeEquivalentTo(modelContent);
                    })
                    .AndNoModelErrors();
            }
        }
    }
}