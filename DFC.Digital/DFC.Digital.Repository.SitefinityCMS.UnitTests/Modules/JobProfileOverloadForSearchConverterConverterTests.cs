using FakeItEasy;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class JobProfileOverloadForSearchConverterConverterTests
    {
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;
        private readonly DynamicContent fakeDynamicContentItem;

        public JobProfileOverloadForSearchConverterConverterTests()
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
            var jobprofileConverter = new JobProfileOverloadForSearchConverter(fakeDynamicContentExtensions);

            var dummyRelatedItems = A.CollectionOfDummy<string>(1).AsEnumerable().AsQueryable();
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedContentUrl(A<DynamicContent>._, A<string>._))
                .Returns(dummyRelatedItems);

            //Act
            jobprofileConverter.ConvertFrom(fakeDynamicContentItem);

            //Assert
            if (hasRelatedSocs)
            {
                A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                    .MustHaveHappened(Repeated.Exactly.Times(7));
            }
            else
            {
                A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                    .MustHaveHappened(Repeated.Exactly.Times(5));
            }

            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<IList<Guid>>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedSearchItems(A<DynamicContent>._, A<string>._, A<int>._))
                .MustHaveHappened();
        }

        private void SetupCalls(bool hasRelatedSocs)
        {
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .Returns("test");
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<IList<Guid>>(A<DynamicContent>._, A<string>._))
                .Returns(new List<Guid>());
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedSearchItems(A<DynamicContent>._, A<string>._, A<int>._))
                .Returns(hasRelatedSocs ? new EnumerableQuery<DynamicContent>(new List<DynamicContent> { fakeDynamicContentItem }) : Enumerable.Empty<DynamicContent>().AsQueryable());
        }
    }
}