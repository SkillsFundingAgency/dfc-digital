using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Config;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Generic;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Tests.Controllers
{
    /// <summary>
    /// Job Profile AnchorLinks Controller tests
    /// </summary>
    public class JobProfileAnchorLinksControllerTests
    {
        [Theory]
        [InlineData(true, true)]
        [InlineData(true, false)]
        [InlineData(false, false)]
        [InlineData(false, true)]
        public void IndexTest(bool inContentAuthoringSite, bool linksAvailable)
        {
            //Setup the fakes and dummies
            var pageContentServiceFake = A.Fake<IJobProfilePage>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());

            var dummyAnchorLinks = linksAvailable
                ? new List<AnchorLink>
                {
                    new AnchorLink
                    {
                        LinkTarget = $"dummy {nameof(AnchorLink.LinkTarget)}",
                        LinkText = $"dummy {nameof(AnchorLink.LinkTarget)}"
                    },
                    new AnchorLink
                    {
                        LinkTarget = $"dummy {nameof(AnchorLink.LinkTarget)}",
                        LinkText = $"dummy {nameof(AnchorLink.LinkTarget)}"
                    }
                }
                : null;

            var dummyJpSections = linksAvailable
                ? new List<JobProfileSection>
                {
                    new JobProfileSection
                    {
                        ContentField = $"dummy {nameof(AnchorLink.LinkTarget)}",
                        Title = $"dummy {nameof(AnchorLink.LinkTarget)}"
                    },
                    new JobProfileSection
                    {
                        ContentField = $"dummy {nameof(AnchorLink.LinkTarget)}",
                        Title = $"dummy {nameof(AnchorLink.LinkTarget)}"
                    }
                }
                : null;

            // Set up calls
            A.CallTo(() => pageContentServiceFake.GetJobProfileSections(A<IEnumerable<JobProfileSectionFilter>>._)).Returns(dummyJpSections);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);

            //Instantiate & Act
            var mapper = new AutoMapper.MapperConfiguration(c => c.AddProfile<JobProfilesAutoMapperProfile>()).CreateMapper();
            var jobProfileAnchorListsController = new JobProfileAnchorLinksController(webAppContextFake, loggerFake, pageContentServiceFake, mapper);

            //Act
            var indexMethodCall = jobProfileAnchorListsController.WithCallTo(c => c.Index());

            //Assert
            if (inContentAuthoringSite)
            {
                indexMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileAnchorLinksViewModel>(vm =>
                    {
                        vm.AnchorLinks.ShouldBeEquivalentTo(dummyAnchorLinks);
                    })
                    .AndNoModelErrors();

                A.CallTo(() => pageContentServiceFake.GetJobProfileSections(A<IEnumerable<JobProfileSectionFilter>>._)).MustHaveHappened();
            }
            else
            {
                A.CallTo(() => pageContentServiceFake.GetJobProfileSections(A<IEnumerable<JobProfileSectionFilter>>._)).MustNotHaveHappened();
                indexMethodCall.ShouldRedirectTo("\\");
            }

            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).MustHaveHappened();
        }

        [Theory]
        [InlineData(true, true, "test")]
        [InlineData(true, false, "test")]
        [InlineData(false, false, "test")]
        [InlineData(false, true, "test")]
        public void IndexTestUrlName(bool inContentAuthoringSite, bool linksAvailable, string urlname)
        {
            //Setup the fakes and dummies
            var pageContentServiceFake = A.Fake<IJobProfilePage>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>(ops => ops.Strict());
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());

            var dummyAnchorLinks = linksAvailable
                ? new List<AnchorLink>
                {
                    new AnchorLink
                    {
                        LinkTarget = $"dummy {nameof(AnchorLink.LinkTarget)}",
                        LinkText = $"dummy {nameof(AnchorLink.LinkText)}"
                    },
                    new AnchorLink
                    {
                        LinkTarget = $"dummy {nameof(AnchorLink.LinkTarget)}",
                        LinkText = $"dummy {nameof(AnchorLink.LinkText)}"
                    }
                }
                : null;

            var dummyJpSections = linksAvailable
                ? new List<JobProfileSection>
                {
                    new JobProfileSection
                    {
                        ContentField = $"dummy {nameof(AnchorLink.LinkTarget)}",
                        Title = $"dummy {nameof(AnchorLink.LinkText)}"
                    },
                    new JobProfileSection
                    {
                        ContentField = $"dummy {nameof(AnchorLink.LinkTarget)}",
                        Title = $"dummy {nameof(AnchorLink.LinkText)}"
                    }
                }
                : null;

            // Set up calls
            A.CallTo(() => pageContentServiceFake.GetJobProfileSections(A<IEnumerable<JobProfileSectionFilter>>._)).Returns(dummyJpSections);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);

            //Instantiate & Act
            var mapper = new AutoMapper.MapperConfiguration(c => c.AddProfile<JobProfilesAutoMapperProfile>()).CreateMapper();
            var jobProfileAnchorListsController = new JobProfileAnchorLinksController(webAppContextFake, loggerFake, pageContentServiceFake, mapper);

            //Act
            var indexUrlNameMethodCall = jobProfileAnchorListsController.WithCallTo(c => c.Index(urlname));

            //Assert
            indexUrlNameMethodCall
                .ShouldRenderDefaultView()
                .WithModel<JobProfileAnchorLinksViewModel>(vm =>
                {
                    vm.AnchorLinks.ShouldBeEquivalentTo(dummyAnchorLinks);
                })
                .AndNoModelErrors();

            A.CallTo(() => pageContentServiceFake.GetJobProfileSections(A<IEnumerable<JobProfileSectionFilter>>._)).MustHaveHappened();
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).MustNotHaveHappened();
        }
    }
}