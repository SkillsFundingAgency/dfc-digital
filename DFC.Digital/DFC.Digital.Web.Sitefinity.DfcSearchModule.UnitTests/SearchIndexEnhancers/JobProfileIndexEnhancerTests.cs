using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.DfcSearchModule.UnitTests
{
    public class JobProfileIndexEnhancerTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetSalaryRangeAsyncTestAsync(bool isPublishing)
        {
            var fakeJobProfileRepo = A.Fake<IJobProfileRepository>();
            var fakeJobProfileCategoryRepo = A.Fake<IJobProfileCategoryRepository>();
            var salaryService = A.Fake<ISalaryService>();
            var salaryCalculator = A.Fake<ISalaryCalculator>();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var dummyJobProfileIndex = A.Dummy<JobProfileIndex>();
            var dummyJobProfile = new JobProfileOverloadForSearch
            {
                SOCCode = nameof(JobProfile.SOCCode)
            };
            var dummySalary = new JobProfileSalary
            {
                Deciles = new Dictionary<int, decimal>
                {
                    { 10, 100 },
                    { 20, 200 }
                }
            };

            A.CallTo(() => fakeJobProfileRepo.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => fakeJobProfileRepo.GetByUrlNameForSearchIndex(A<string>._, A<bool>._)).Returns(dummyJobProfile);
            A.CallTo(() => salaryService.GetSalaryBySocAsync(A<string>._)).Returns(Task.FromResult(dummySalary));
            A.CallTo(() => salaryCalculator.GetStarterSalary(A<JobProfileSalary>._)).Returns(1000);
            A.CallTo(() => salaryCalculator.GetExperiencedSalary(A<JobProfileSalary>._)).Returns(2000);

            var enhancer = new JobProfileIndexEnhancer(fakeJobProfileRepo, fakeJobProfileCategoryRepo, salaryService, salaryCalculator, fakeLogger);
            enhancer.Initialise(dummyJobProfileIndex, isPublishing);

            var result = await enhancer.PopulateSalary("1", "2");

            result.StarterSalary.Should().Be(1000);
            result.SalaryExperienced.Should().Be(2000);

            A.CallTo(() => fakeJobProfileRepo.GetByUrlNameForSearchIndex(A<string>._, isPublishing)).MustHaveHappened();
            A.CallTo(() => fakeJobProfileRepo.GetByUrlName(A<string>._)).MustNotHaveHappened();

            A.CallTo(() => salaryService.GetSalaryBySocAsync(A<string>._)).MustHaveHappened();
            A.CallTo(() => salaryCalculator.GetStarterSalary(A<JobProfileSalary>._)).MustHaveHappened();
            A.CallTo(() => salaryCalculator.GetExperiencedSalary(A<JobProfileSalary>._)).MustHaveHappened();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void GetRelatedFieldsWithUrlTest(bool isPublishing)
        {
            var fakeJobProfileRepo = A.Fake<IJobProfileRepository>();
            var fakeJobProfileCategoryRepo = A.Fake<IJobProfileCategoryRepository>();
            var salaryService = A.Fake<ISalaryService>();
            var salaryCalculator = A.Fake<ISalaryCalculator>();
            var fakeLogger = A.Fake<IApplicationLogger>();
            var dummyJobProfileIndex = A.Dummy<JobProfileIndex>();
            var dummyJobProfile = A.Dummy<JobProfileOverloadForSearch>();
            var dummyCategories = new List<JobProfileCategory>
            {
                new JobProfileCategory
                {
                    Title = "one",
                    Url = "url1"
                },
                new JobProfileCategory
                {
                    Title = "two",
                    Url = "url2"
                }
            };

            var expectedCategories = dummyCategories.Select(c => $"{c.Title}|{c.Url}");

            A.CallTo(() => fakeJobProfileRepo.GetByUrlName(A<string>._)).Returns(dummyJobProfile);
            A.CallTo(() => fakeJobProfileRepo.GetByUrlNameForSearchIndex(A<string>._, A<bool>._)).Returns(dummyJobProfile);
            A.CallTo(() => fakeJobProfileCategoryRepo.GetByIds(A<IList<Guid>>._)).Returns(dummyCategories);

            var enhancer = new JobProfileIndexEnhancer(fakeJobProfileRepo, fakeJobProfileCategoryRepo, salaryService, salaryCalculator, fakeLogger);
            enhancer.Initialise(dummyJobProfileIndex, isPublishing);
            enhancer.PopulateRelatedFieldsWithUrl();

            A.CallTo(() => fakeJobProfileRepo.GetByUrlNameForSearchIndex(A<string>._, isPublishing)).MustHaveHappened();
            A.CallTo(() => fakeJobProfileRepo.GetByUrlName(A<string>._)).MustNotHaveHappened();

            A.CallTo(() => fakeJobProfileCategoryRepo.GetByIds(A<IList<Guid>>._)).MustHaveHappened();

            dummyJobProfileIndex.JobProfileCategoriesWithUrl.Should().BeEquivalentTo(expectedCategories);
        }
    }
}