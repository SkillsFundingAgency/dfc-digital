using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.UnitTests;
using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions.Tests
{
    public class JobProfileReportRepositoryTests
    {
        private readonly IDynamicModuleRepository<JobProfile> fakeJobProfileRepository;
        private readonly IDynamicModuleConverter<JobProfileApprenticeshipVacancyReport> fakeJobProfileApprenticeshipVacancyReportConverter;
        private readonly IDynamicModuleRepository<ApprenticeVacancy> fakeApprenticeVacancyRepository;
        private readonly IDynamicModuleConverter<ApprenticeshipVacancyReport> fakeApprenticeVacancyConverter;
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;

        public JobProfileReportRepositoryTests()
        {
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>(ops => ops.Strict());
            fakeJobProfileRepository = A.Fake<IDynamicModuleRepository<JobProfile>>(ops => ops.Strict());
            fakeApprenticeVacancyConverter = A.Fake<IDynamicModuleConverter<ApprenticeshipVacancyReport>>(ops => ops.Strict());
            fakeApprenticeVacancyRepository = A.Fake<IDynamicModuleRepository<ApprenticeVacancy>>(ops => ops.Strict());
            fakeJobProfileApprenticeshipVacancyReportConverter = A.Fake<IDynamicModuleConverter<JobProfileApprenticeshipVacancyReport>>(ops => ops.Strict());
        }

        [Fact]
        public void GetJobProfileApprenticeshipVacancyReport()
        {
            //Arrange
            SetupCalls();
            var jobProfileReportRepository = new JobProfileReportRepository(fakeJobProfileRepository, fakeJobProfileApprenticeshipVacancyReportConverter, fakeApprenticeVacancyRepository, fakeApprenticeVacancyConverter, fakeDynamicContentExtensions);

            // Act
            jobProfileReportRepository.GetJobProfileApprenticeshipVacancyReport();

            // Assert
            A.CallTo(() => fakeJobProfileRepository.GetAll())
                .MustHaveHappened();
            A.CallTo(() => fakeApprenticeVacancyRepository.GetAll())
                .MustHaveHappened();
            A.CallTo(() => fakeDynamicContentExtensions.SetRelatedDataSourceContext(A<IQueryable<DynamicContent>>._)).MustHaveHappened();
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeJobProfileRepository.GetAll()).Returns(new EnumerableQuery<DynamicContent>(new List<DynamicContent> { new DynamicContent() }).AsQueryable());
            A.CallTo(() => fakeApprenticeVacancyRepository.GetAll()).Returns(new EnumerableQuery<DynamicContent>(new List<DynamicContent> { new DynamicContent() }).AsQueryable());
            A.CallTo(() => fakeApprenticeVacancyConverter.ConvertFrom(A<DynamicContent>._)).Returns(new ApprenticeshipVacancyReport());
            A.CallTo(() => fakeJobProfileApprenticeshipVacancyReportConverter.ConvertFrom(A<DynamicContent>._)).Returns(new JobProfileApprenticeshipVacancyReport());
            A.CallTo(() => fakeDynamicContentExtensions.SetRelatedDataSourceContext(A<IQueryable<DynamicContent>>._)).DoesNothing();
        }
    }
}