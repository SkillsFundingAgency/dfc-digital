using FakeItEasy;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class WhatYouWillDoConverterTests
    {
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;
        private readonly DynamicContent fakeDynamicContentItem;

        public WhatYouWillDoConverterTests()
        {
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>(ops => ops.Strict());
            fakeDynamicContentItem = A.Dummy<DynamicContent>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ConvertFromTest(bool cadReady)
        {
            //Assign
            SetupCalls(cadReady);
            var whatYouWillDoConverter =
                new WhatYouWillDoConverter(fakeDynamicContentExtensions);

            //Act
            whatYouWillDoConverter.ConvertFrom(fakeDynamicContentItem);

            if (cadReady)
            {
                //Assert
                A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                    .MustHaveHappened();
                A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, A<string>._, A<int>._))
                    .MustHaveHappened();
            }
            else
            {
                //Assert
                A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                    .MustNotHaveHappened();
            }
        }

        private void SetupCalls(bool cadReady)
        {
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .Returns("test");

            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<bool>(A<DynamicContent>._, A<string>._))
                .Returns(cadReady);
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, A<string>._, A<int>._))
                .Returns(new EnumerableQuery<DynamicContent>(new List<DynamicContent> { fakeDynamicContentItem }));
        }
    }
}