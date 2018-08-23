using FakeItEasy;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class JobProfileOverloadForWhatItTakesConverterTests
    {
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;
        private readonly DynamicContent fakeDynamicContentItem;

        public JobProfileOverloadForWhatItTakesConverterTests()
        {
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            fakeDynamicContentItem = A.Dummy<DynamicContent>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ConvertFromTest(bool hasRelatedSocs)
        {
            // Arrange
            SetupCalls(hasRelatedSocs);
            var jobprofileConverter = new JobProfileOverloadForWhatItTakesConverter(fakeDynamicContentExtensions);

            var dummyRelatedItems = A.CollectionOfDummy<string>(1).AsEnumerable().AsQueryable();
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedContentUrl(A<DynamicContent>._, A<string>._))
                .Returns(dummyRelatedItems);

            //Act
            var jobProfile = jobprofileConverter.ConvertFrom(fakeDynamicContentItem);

            //Assert
            jobProfile.HasRelatedSocSkillMatrices.Should().Be(hasRelatedSocs);
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldChoiceValue(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, A<string>._, A<int>._))
                .MustHaveHappened();
        }

        private void SetupCalls(bool hasRelatedSocs)
        {
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .Returns("test");
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldChoiceValue(A<DynamicContent>._, A<string>._))
                .Returns("test");
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, A<string>._, A<int>._))
                .Returns(hasRelatedSocs ? new EnumerableQuery<DynamicContent>(new List<DynamicContent> { fakeDynamicContentItem }) : Enumerable.Empty<DynamicContent>().AsQueryable());
        }
    }
}