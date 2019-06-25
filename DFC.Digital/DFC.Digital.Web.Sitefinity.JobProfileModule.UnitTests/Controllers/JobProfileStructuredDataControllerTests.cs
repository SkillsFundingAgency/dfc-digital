using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FakeItEasy;
using FluentAssertions;
using TestStack.FluentMVCTesting;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests.Controllers
{
    public class JobProfileStructuredDataControllerTests
    {
        #region private fields
        private readonly IStructuredDataInjectionRepository fakeStructuredDataInjectionRepository;
        private readonly IMapper mapperCfg;
        private readonly JobProfileStructuredDataController jobProfileStructuredDataController;
        private readonly IApplicationLogger fakeApplicationLogger;
        private readonly ISitefinityPage fakeSitefinityPage;
        #endregion

        #region Ctor

        public JobProfileStructuredDataControllerTests()
        {
            fakeStructuredDataInjectionRepository = A.Fake<IStructuredDataInjectionRepository>(ops => ops.Strict());
            fakeApplicationLogger = A.Fake<IApplicationLogger>(ops => ops.Strict());
            mapperCfg = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<JobProfilesAutoMapperProfile>();
            }).CreateMapper();
            fakeSitefinityPage = A.Fake<ISitefinityPage>(ops => ops.Strict());
            jobProfileStructuredDataController = GetController();
        }
        #endregion

        #region Tests

        [Fact]
        public void IndexTest()
        {
            //Assign
            A.CallTo(() => fakeSitefinityPage.GetDefaultJobProfileToUse(A<string>._)).Returns("test");
            A.CallTo(() => fakeStructuredDataInjectionRepository.GetByJobProfileUrlName(A<string>._))
                .Returns(
                    new StructuredDataInjection
                    {
                        Title = nameof(StructuredDataInjection.Title),
                        Script = nameof(StructuredDataInjection.Script),
                        DataType = nameof(StructuredDataInjection.DataType),
                        JobProfileLinkName = nameof(StructuredDataInjection.JobProfileLinkName)
                    });

            //Act
            var indexMethodCall = jobProfileStructuredDataController.WithCallTo(c => c.Index());

            //Assert
            indexMethodCall
                .ShouldRenderDefaultView()
                .WithModel<JobProfileStructuredDataViewModel>(vm =>
                {
                    vm.Title.Should().BeEquivalentTo(nameof(StructuredDataInjection.Title));
                    vm.Script.Should().BeEquivalentTo(nameof(StructuredDataInjection.Script));
                    vm.DataType.Should().BeEquivalentTo(nameof(StructuredDataInjection.DataType));
                    vm.JobProfileLinkName.Should().BeEquivalentTo(nameof(StructuredDataInjection.JobProfileLinkName));
                })
                .AndNoModelErrors();

            A.CallTo(() => fakeSitefinityPage.GetDefaultJobProfileToUse(A<string>._)).MustHaveHappened();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IndexUrlNameTest(bool validUrlName)
        {
            //Assign
            var jobProfileUrl = nameof(StructuredDataInjection.JobProfileLinkName);
            A.CallTo(() => fakeStructuredDataInjectionRepository.GetByJobProfileUrlName(A<string>._))
                .Returns(validUrlName
                    ? new StructuredDataInjection
                    {
                        Title = nameof(StructuredDataInjection.Title),
                        Script = nameof(StructuredDataInjection.Script),
                        DataType = nameof(StructuredDataInjection.DataType),
                        JobProfileLinkName = jobProfileUrl
                    }
                    : null);

            //Act
            var indexMethodCall = jobProfileStructuredDataController.WithCallTo(c => c.Index(jobProfileUrl));

            //Assert
            if (validUrlName)
            {
                indexMethodCall
                    .ShouldRenderDefaultView()
                    .WithModel<JobProfileStructuredDataViewModel>(vm =>
                    {
                        vm.Title.Should().BeEquivalentTo(nameof(StructuredDataInjection.Title));
                        vm.Script.Should().BeEquivalentTo(nameof(StructuredDataInjection.Script));
                        vm.DataType.Should().BeEquivalentTo(nameof(StructuredDataInjection.DataType));
                        vm.JobProfileLinkName.Should().BeEquivalentTo(jobProfileUrl);
                    })
                    .AndNoModelErrors();
            }
            else
            {
                indexMethodCall
                    .ShouldReturnEmptyResult();
            }

            A.CallTo(() => fakeStructuredDataInjectionRepository.GetByJobProfileUrlName(A<string>._))
                .MustHaveHappened();
        }

        #endregion

        #region helpers

        private JobProfileStructuredDataController GetController()
        {
            return new JobProfileStructuredDataController(fakeSitefinityPage, mapperCfg, fakeStructuredDataInjectionRepository, fakeApplicationLogger);
        }
        #endregion
    }
}
