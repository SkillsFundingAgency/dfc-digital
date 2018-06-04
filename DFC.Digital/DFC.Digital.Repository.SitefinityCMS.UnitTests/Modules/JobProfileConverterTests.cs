using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using DFC.Digital.Repository.SitefinityCMS.Base;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class JobProfileConverterTests
    {
        private const string SocField = "SOC";
        private readonly IRelatedClassificationsRepository fakeRelatedClassificationsRepository;
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;
        private readonly DynamicContent fakeDynamicContentItem;

        public JobProfileConverterTests()
        {
            fakeRelatedClassificationsRepository = A.Fake<IRelatedClassificationsRepository>();
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            fakeDynamicContentItem = A.Dummy<DynamicContent>();
            SetupCalls();
        }

        [Theory]
        [InlineData("fieldOne")]
        [InlineData("fieldTwo")]
        public void GetRelatedContentUrlTest(string relatedField)
        {
            //Assign
            var jobprofileConverter = new JobProfileConverter(fakeRelatedClassificationsRepository, fakeDynamicContentExtensions);

            //Act
            jobprofileConverter.GetRelatedContentUrl(fakeDynamicContentItem, relatedField);

            //Assert
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, relatedField, A<int>._))
                .MustHaveHappened();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ConvertFromTest(bool socAssigned)
        {
            //Assign
            SetupSocCall(socAssigned);
            var jobprofileConverter = new JobProfileConverter(fakeRelatedClassificationsRepository, fakeDynamicContentExtensions);

            //Act
            var jobProfile = jobprofileConverter.ConvertFrom(fakeDynamicContentItem);

            //Assert
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<bool>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<decimal?>(A<DynamicContent>._, A<string>._))
                .MustHaveHappened();
            A.CallTo(() => fakeRelatedClassificationsRepository.GetRelatedClassifications(A<DynamicContent>._, A<string>._, A<string>._)).MustHaveHappened();

            if (socAssigned)
            {
                A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, nameof(JobProfile.SOCCode)))
                    .MustHaveHappened();
                jobProfile.SOCCode.Should().NotBeNullOrEmpty();
            }
            else
            {
                A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, nameof(JobProfile.SOCCode)))
                    .MustNotHaveHappened();
                jobProfile.SOCCode.Should().BeNullOrEmpty();
            }
        }

        private void SetupCalls()
        {
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
                .Returns("test");
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<bool>(A<DynamicContent>._, A<string>._))
                .Returns(false);
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<decimal?>(A<DynamicContent>._, A<string>._))
                .Returns(10);
            A.CallTo(() => fakeRelatedClassificationsRepository.GetRelatedClassifications(A<DynamicContent>._, A<string>._, A<string>._)).Returns(new EnumerableQuery<string>(new List<string> { "test" }));
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, A<string>._, A<int>._))
                .Returns(new EnumerableQuery<DynamicContent>(new List<DynamicContent> { fakeDynamicContentItem }));
        }

        private void SetupSocCall(bool socAssigned)
        {
            var socResults =
                new EnumerableQuery<DynamicContent>(new List<DynamicContent>
                {
                    socAssigned ? fakeDynamicContentItem : null
                });
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, SocField, A<int>._))
                .Returns(socResults);
        }
    }
}