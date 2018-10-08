using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Generic;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class BaseJobProfileWidgetControllerTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(false, true)]
        [InlineData(false, false)]
        public void BaseIndexTest(bool inContentAuthoringSite, bool isContentPreviewMode)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());
            var formatContentServiceFake = A.Fake<IFormatContentService>(ops => ops.Strict());
            var dummyJobProfile = GetDummyJobPRofile(true);

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => formatContentServiceFake.GetParagraphText(A<string>._, A<IEnumerable<string>>._, A<string>._)).Returns("test");
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);

            //Instantiate & Act
            using (var baseJobprofileController = new JobProfileWhatYouWillDoController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage, formatContentServiceFake))
            {
                //Act
                var indexMethodCall = baseJobprofileController.WithCallTo(c => c.BaseIndex());

                //Assert
                A.CallTo(() => webAppContextFake.IsContentAuthoringSite).MustHaveHappened();

                if (inContentAuthoringSite)
                {
                    A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).MustHaveHappened();
                }
                else
                {
                    indexMethodCall.ShouldRedirectTo("\\");
                }
            }
        }

        [Theory]
        [InlineData("Test", true, false)]
        [InlineData("Test", false, false)]
        [InlineData("Test", true, true)]
        [InlineData("Test", false, true)]
        public void BaseIndexUrlNameTest(string urlName, bool useValidJobProfile, bool isContentPreviewMode)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());
            var sitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());
            var formatContentServiceFake = A.Fake<IFormatContentService>(ops => ops.Strict());

            var dummyJobProfile = GetDummyJobPRofile(useValidJobProfile);

            // Set up calls
            A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => webAppContextFake.IsContentPreviewMode).Returns(isContentPreviewMode);
            A.CallTo(() => webAppContextFake.SetMetaDescription(A<string>._)).DoesNothing();
            A.CallTo(() => sitefinityPage.GetDefaultJobProfileToUse(A<string>._)).ReturnsLazily((string defaultProfile) => defaultProfile);
            A.CallTo(() => formatContentServiceFake.GetParagraphText(A<string>._, A<IEnumerable<string>>._, A<string>._)).Returns("test");

            //Instantiate & Act
            using (var baseJobprofileController = new JobProfileWhatYouWillDoController(repositoryFake, webAppContextFake, loggerFake, sitefinityPage, formatContentServiceFake))
            {
                //Act
                var indexWithUrlNameMethodCall = baseJobprofileController.WithCallTo(c => c.BaseIndex(urlName));
                A.CallTo(() => webAppContextFake.IsContentPreviewMode).MustHaveHappened();
                if (isContentPreviewMode)
                {
                    A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).MustHaveHappened();
                    A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).MustNotHaveHappened();
                }
                else
                {
                    A.CallTo(() => repositoryFake.GetByUrlName(A<string>._)).MustHaveHappened();
                    A.CallTo(() => repositoryFake.GetByUrlNameForPreview(A<string>._)).MustNotHaveHappened();
                }

                if (useValidJobProfile)
                {
                    A.CallTo(() => webAppContextFake.SetMetaDescription(A<string>._)).MustHaveHappened();
                }
                else
                {
                    A.CallTo(() => webAppContextFake.SetMetaDescription(A<string>._)).MustNotHaveHappened();
                    indexWithUrlNameMethodCall.ShouldGiveHttpStatus(404);
                }
            }
        }

        private static JobProfile GetDummyJobPRofile(bool useValidJobProfile)
        {
            return useValidJobProfile ?
                   new JobProfile
                   {
                       AlternativeTitle = nameof(JobProfile.AlternativeTitle),
                       SalaryRange = nameof(JobProfile.SalaryRange),
                       Overview = nameof(JobProfile.Overview),
                       Title = nameof(JobProfile.Title),
                       SOCCode = nameof(JobProfile.SOCCode),
                       CareerPathAndProgression = nameof(JobProfile.CareerPathAndProgression),
                       HowToBecome = nameof(JobProfile.HowToBecome),
                       Salary = nameof(JobProfile.Salary),
                       Skills = nameof(JobProfile.Skills),
                       WhatYouWillDo = nameof(JobProfile.WhatYouWillDo),
                       WhatYouWillDoData = new WhatYouWillDo { Locations = new List<string> { "Office and Client Site" }, Uniforms = new List<string> { "Casual / Smart Casual / Business Dress" }, Environments = new List<string> { "Friendly / Business / Secured" }, IsCadReady = true },
                       WorkingHoursPatternsAndEnvironment = nameof(JobProfile.WorkingHoursPatternsAndEnvironment),
                       HowToBecomeData = new HowToBecome(),
                       Restrictions = new List<Restriction> { new Restriction { Info = nameof(Restriction.Info), Title = nameof(Restriction.Title) } },
                       OtherRequirements = nameof(JobProfile.OtherRequirements)
                   }
                   : null;
        }
    }
}