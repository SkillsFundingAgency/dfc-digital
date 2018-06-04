using FakeItEasy;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class HowToBecomeConverterTests
    {
        private readonly IRelatedClassificationsRepository fakeRelatedClassificationsRepository;
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;
        private readonly DynamicContent fakeDynamicContentItem;

        public HowToBecomeConverterTests()
        {
            fakeRelatedClassificationsRepository = A.Fake<IRelatedClassificationsRepository>();
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            fakeDynamicContentItem = A.Dummy<DynamicContent>();
            SetupCalls();
        }

        [Fact]
        public void ConvertFromTest()
        {
            //Assign
            SetupCalls();
            var howToBecomeConverter =
                new HowToBecomeConverter(fakeRelatedClassificationsRepository, fakeDynamicContentExtensions);

            //Act
            howToBecomeConverter.ConvertFrom(fakeDynamicContentItem);

            //Assert
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => fakeRelatedClassificationsRepository.GetRelatedClassifications(A<DynamicContent>._, A<string>._, A<string>._)).MustHaveHappened();
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .Returns("test");
            A.CallTo(() => fakeRelatedClassificationsRepository.GetRelatedClassifications(A<DynamicContent>._, A<string>._, A<string>._)).Returns(new EnumerableQuery<string>(new List<string> { "test" }));
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, A<string>._, A<int>._))
                .Returns(new EnumerableQuery<DynamicContent>(new List<DynamicContent> { fakeDynamicContentItem }));
        }
    }
}