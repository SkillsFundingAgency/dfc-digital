using DFC.Digital.Data.Model;
using FakeItEasy;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class JobProfileRepositoryTests
    {
        //[Fact()]
        //Cannot Unit test as unable to fake the var dynamicContent = Get(item => item.UrlName == urlName);
        //call in the GetByUrlName method call.
        private void GetByUrlNameTest()
        {
            var fakeJobProfileConverter = A.Fake<JobProfileConverter>();

            var dummyContent = A.Dummy<DynamicContent>();
            var dummyJobProfile = A.Dummy<JobProfile>();

            A.CallTo(() => fakeJobProfileConverter.ConvertFrom(dummyContent)).Returns(dummyJobProfile);

            var jobProfileRepository = new JobProfileRepository(fakeJobProfileConverter);

            jobProfileRepository.GetByUrlName("testURLName");
        }
    }
}