using DFC.Digital.Data.Model;
using FakeItEasy;
using System;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class JobProfileRepositoryTests
    {
        //Cannot Unit test as unable to fake the var dynamicContent = Get(item => item.UrlName == urlName);
        //call in the GetByUrlName method call.
        //[Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Justification = "Will be used when we implement tests for Sitefinity repositires")]
        private void GetByUrlNameTest()
        {
            var fakeJobProfileConverter = A.Fake<JobProfileConverter>();
            var fakeRepository = A.Fake<IDynamicModuleRepository<JobProfile>>();

            var dummyJobProfile = A.Dummy<JobProfile>();

            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyJobProfile);

            var jobProfileRepository = new JobProfileRepository(fakeRepository, fakeJobProfileConverter);

            jobProfileRepository.GetByUrlName("testURLName");

            A.CallTo(() => fakeRepository.Get(A<Expression<Func<DynamicContent, bool>>>._)).MustHaveHappened();
        }
    }
}