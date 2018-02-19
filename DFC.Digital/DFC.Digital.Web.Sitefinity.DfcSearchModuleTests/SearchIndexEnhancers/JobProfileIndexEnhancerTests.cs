using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.DfcSearchModule.SearchIndexEnhancers.Tests
{
    public class JobProfileIndexEnhancerTests
    {
        [Theory]
        [InlineData(false, "£1,000 to £2,000")]
        [InlineData(true, "£10 to £20")]
        public async Task GetSalaryRangeAsyncTestAsync(bool isSalaryOverriden, string expectedString)
        {
            var fakeJobProfileRepo = A.Fake<IJobProfileRepository>();
            var fakeJobProfileCategoryRepo = A.Fake<IJobProfileCategoryRepository>();
            var salaryService = A.Fake<ISalaryService>();
            var salaryCalculator = A.Fake<ISalaryCalculator>();
            var dummyJobProfileIndex = A.Dummy<JobProfileIndex>();
            var dummyJobProfile = new JobProfile
            {
                IsLmiSalaryFeedOverriden = isSalaryOverriden,
                SalaryStarter = 10,
                SalaryExperienced = 20,
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
            A.CallTo(() => salaryService.GetSalaryBySocAsync(A<string>._)).Returns(Task.FromResult(dummySalary));
            A.CallTo(() => salaryCalculator.GetStarterSalary(A<JobProfileSalary>._)).Returns(1000);
            A.CallTo(() => salaryCalculator.GetExperiencedSalary(A<JobProfileSalary>._)).Returns(2000);

            var enhancer = new JobProfileIndexEnhancer(fakeJobProfileRepo, fakeJobProfileCategoryRepo, salaryService, salaryCalculator);
            enhancer.Initialise(dummyJobProfileIndex);

            var result = await enhancer.GetSalaryRangeAsync();

            result.Should().Be(expectedString);
            A.CallTo(() => fakeJobProfileRepo.GetByUrlName(A<string>._)).MustHaveHappened();
            if (isSalaryOverriden)
            {
                A.CallTo(() => salaryService.GetSalaryBySocAsync(A<string>._)).MustNotHaveHappened();
                A.CallTo(() => salaryCalculator.GetStarterSalary(A<JobProfileSalary>._)).MustNotHaveHappened();
                A.CallTo(() => salaryCalculator.GetExperiencedSalary(A<JobProfileSalary>._)).MustNotHaveHappened();
            }
            else
            {
                A.CallTo(() => salaryService.GetSalaryBySocAsync(A<string>._)).MustHaveHappened();
                A.CallTo(() => salaryCalculator.GetStarterSalary(A<JobProfileSalary>._)).MustHaveHappened();
                A.CallTo(() => salaryCalculator.GetExperiencedSalary(A<JobProfileSalary>._)).MustHaveHappened();
            }
        }

        [Fact]
        public void GetRelatedFieldsWithUrlTest()
        {
            var fakeJobProfileRepo = A.Fake<IJobProfileRepository>();
            var fakeJobProfileCategoryRepo = A.Fake<IJobProfileCategoryRepository>();
            var salaryService = A.Fake<ISalaryService>();
            var salaryCalculator = A.Fake<ISalaryCalculator>();
            var dummyJobProfileIndex = A.Dummy<JobProfileIndex>();
            var dummyJobProfile = new JobProfile
            {
                RelatedInterests = new[] { "1|one", "2|two" }.AsQueryable(),
                RelatedEnablers = new[] { "1|one", "2|two" }.AsQueryable(),
                RelatedEntryQualifications = new[] { "1|one", "2|two" }.AsQueryable(),
                RelatedTrainingRoutes = new[] { "1|one", "2|two" }.AsQueryable(),
                RelatedPreferredTaskTypes = new[] { "1|one", "2|two" }.AsQueryable(),
                RelatedJobAreas = new[] { "1|one", "2|two" }.AsQueryable()
            };

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
            A.CallTo(() => fakeJobProfileCategoryRepo.GetByIds(A<IList<Guid>>._)).Returns(dummyCategories);

            var enhancer = new JobProfileIndexEnhancer(fakeJobProfileRepo, fakeJobProfileCategoryRepo, salaryService, salaryCalculator);
            enhancer.Initialise(dummyJobProfileIndex);
            enhancer.GetRelatedFieldsWithUrl(dummyJobProfileIndex);

            A.CallTo(() => fakeJobProfileRepo.GetByUrlName(A<string>._)).MustHaveHappened();
            A.CallTo(() => fakeJobProfileCategoryRepo.GetByIds(A<IList<Guid>>._)).MustHaveHappened();

            dummyJobProfileIndex.JobProfileCategoriesWithUrl.ShouldBeEquivalentTo(expectedCategories);
            dummyJobProfileIndex.Interests.ShouldBeEquivalentTo(dummyJobProfile.RelatedInterests);
            dummyJobProfileIndex.Enablers.ShouldBeEquivalentTo(dummyJobProfile.RelatedEnablers);
            dummyJobProfileIndex.EntryQualifications.ShouldBeEquivalentTo(dummyJobProfile.RelatedEntryQualifications);
            dummyJobProfileIndex.TrainingRoutes.ShouldBeEquivalentTo(dummyJobProfile.RelatedTrainingRoutes);
            dummyJobProfileIndex.JobAreas.ShouldBeEquivalentTo(dummyJobProfile.RelatedJobAreas);
            dummyJobProfileIndex.PreferredTaskTypes.ShouldBeEquivalentTo(dummyJobProfile.RelatedPreferredTaskTypes);
        }
    }
}