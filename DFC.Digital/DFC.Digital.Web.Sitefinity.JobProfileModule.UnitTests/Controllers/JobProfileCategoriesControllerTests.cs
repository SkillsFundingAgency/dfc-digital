using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    /// <summary>
    /// Job Profile Details Controller tests
    /// </summary>
    public class JobProfileCategoriesControllerTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IndexTest(bool inContentAuthoringSite)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileCategoryRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());

            var dummyjobProfileCategories = new EnumerableQuery<JobProfileCategory>(new List<JobProfileCategory>
            {
                new JobProfileCategory
                {
                    Description = nameof(JobProfileCategory.Description),
                    Title = nameof(JobProfileCategory.Title),
                    Url = nameof(JobProfileCategory.Url),
                    Name = nameof(JobProfileCategory.Name)
                }
            });

            // Set up calls
            A.CallTo(() => repositoryFake.GetJobProfileCategories()).Returns(dummyjobProfileCategories);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);

            //Instantiate & Act
            var jobprofileController =
                new JobProfileCategoriesController(repositoryFake, webAppContextFake, loggerFake);

            //Act
            var indexMethodCall = jobprofileController.WithCallTo(c => c.Index());

            //Assert
            indexMethodCall
                .ShouldRenderDefaultView()
                .WithModel<JobProfileCategoriesViewModel>(vm =>
                {
                    vm.IsContentAuthoring.Should().Be(webAppContextFake.IsContentAuthoringSite);
                })
                .AndNoModelErrors();

            A.CallTo(() => repositoryFake.GetJobProfileCategories()).MustHaveHappened();
        }

        [Theory]
        [InlineData(true, false)]
        [InlineData(false, true)]
        public void IndexSideDisplayTest(bool inContentAuthoringSite, bool sideDisplay)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileCategoryRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());

            var dummyjobProfileCategories = new EnumerableQuery<JobProfileCategory>(new List<JobProfileCategory>
            {
                new JobProfileCategory
                {
                    Description = nameof(JobProfileCategory.Description),
                    Title = nameof(JobProfileCategory.Title),
                    Url = nameof(JobProfileCategory.Url),
                    Name = nameof(JobProfileCategory.Name)
                }
            });

            // Set up calls
            A.CallTo(() => repositoryFake.GetJobProfileCategories()).Returns(dummyjobProfileCategories);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);

            //Instantiate & Act
            var jobprofileController = new JobProfileCategoriesController(repositoryFake, webAppContextFake, loggerFake) { SidePageDisplay = sideDisplay };

            //Act
            var indexMethodCall = jobprofileController.WithCallTo(c => c.Index());

            //Assert
            if (sideDisplay)
            {
                indexMethodCall
                    .ShouldRenderView("RelatedJobCategories")
                    .WithModel<RelatedJobProfileCategoriesViewModel>(vm =>
                    {
                        vm.IsContentAuthoring.Should().Be(webAppContextFake.IsContentAuthoringSite);
                    })
                    .AndNoModelErrors();
            }
            else
            {
                indexMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileCategoriesViewModel>(vm =>
                    {
                        vm.IsContentAuthoring.Should().Be(webAppContextFake.IsContentAuthoringSite);
                    })
                    .AndNoModelErrors();
            }

            A.CallTo(() => repositoryFake.GetJobProfileCategories()).MustHaveHappened();
        }

        [Theory]
        [InlineData(true, "newCat")]
        [InlineData(false, "test")]
        public void IndexUrlNameTest(bool inContentAuthoringSite, string urlName)
        {
            //Setup the fakes and dummies
            var repositoryFake = A.Fake<IJobProfileCategoryRepository>(ops => ops.Strict());
            var loggerFake = A.Fake<IApplicationLogger>();
            var webAppContextFake = A.Fake<IWebAppContext>(ops => ops.Strict());

            var dummyjobProfileCategories = new EnumerableQuery<JobProfileCategory>(new List<JobProfileCategory>
            {
                new JobProfileCategory
                {
                    Description = nameof(JobProfileCategory.Description),
                    Title = nameof(JobProfileCategory.Title),
                    Url = nameof(JobProfileCategory.Url),
                    Name = nameof(JobProfileCategory.Name)
                },
                new JobProfileCategory
                {
                    Description = nameof(JobProfileCategory.Description),
                    Title = nameof(JobProfileCategory.Title),
                    Url = urlName,
                    Name = nameof(JobProfileCategory.Name)
                }
            });

            var filterJpCategories =
                dummyjobProfileCategories.Where(
                    jpCat => !jpCat.Url.Equals(urlName, StringComparison.InvariantCultureIgnoreCase));

            // Set up calls
            A.CallTo(() => repositoryFake.GetJobProfileCategories())
                .Returns(dummyjobProfileCategories);
            A.CallTo(() => webAppContextFake.IsContentAuthoringSite).Returns(inContentAuthoringSite);

            //Instantiate & Act
            var jobprofileController = new JobProfileCategoriesController(repositoryFake, webAppContextFake, loggerFake);

            //Act
            var indexUrlNameMethodCall = jobprofileController.WithCallTo(c => c.Index(urlName));

            //Assert
            indexUrlNameMethodCall
                .ShouldRenderView("RelatedJobCategories")
                .WithModel<RelatedJobProfileCategoriesViewModel>(vm =>
                {
                    vm.IsContentAuthoring.Should().Be(webAppContextFake.IsContentAuthoringSite);
                    vm.JobProfileCategories.Should().BeEquivalentTo(filterJpCategories);
                })
                .AndNoModelErrors();

            A.CallTo(() => repositoryFake.GetJobProfileCategories()).MustHaveHappened();
        }
    }
}