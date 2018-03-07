using DFC.Digital.Core;
using DFC.Digital.Data.Model;
using FluentAssertions;
using System.Collections.Generic;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Core.Tests
{
    public class WebAppContextTest
    {
        [Fact]
        public void GetCookieTest()
        {
            var expectation = new VocSurveyPersonalisation
            {
                Personalisation = new Dictionary<string, string>
                {
                    { Constants.LastVisitedJobProfileKey, Constants.Unknown },
                    { Constants.GoogleClientIdKey, Constants.Unknown }
                }
            };

            var webAppContext = new WebAppContext();
            var result = webAppContext.GetVocCookie("name");

            result.ShouldBeEquivalentTo(expectation);
        }

        [Fact]
        public void GetCookieWithHttpContextTest()
        {
            using (new FakeHttpContext.FakeHttpContext())
            {
                var expectation = new VocSurveyPersonalisation
                {
                    Personalisation = new Dictionary<string, string>
                    {
                        {
                            Constants.LastVisitedJobProfileKey, Constants.Unknown },
                        { Constants.GoogleClientIdKey, Constants.Unknown }
                    }
                };

                var webAppContext = new WebAppContext();
                var result = webAppContext.GetVocCookie("name");

                result.ShouldBeEquivalentTo(expectation);
            }
        }
    }
}