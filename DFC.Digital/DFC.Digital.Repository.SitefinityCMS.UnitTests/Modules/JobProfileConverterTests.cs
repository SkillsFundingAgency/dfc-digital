using DFC.Digital.Core.Logging;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using FakeItEasy;
using FluentAssertions;
using System;
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
        private readonly IContentPropertyConverter<HowToBecome> htbContentPropertyConverter;
        private readonly IContentPropertyConverter<WhatYouWillDo> wywdPropertyConverter;

        public JobProfileConverterTests()
        {
            fakeRelatedClassificationsRepository = A.Fake<IRelatedClassificationsRepository>();
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            htbContentPropertyConverter = A.Fake<IContentPropertyConverter<HowToBecome>>();
            wywdPropertyConverter = A.Fake<IContentPropertyConverter<WhatYouWillDo>>();
            fakeDynamicContentItem = A.Dummy<DynamicContent>();
            SetupCalls();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ConvertFromTest(bool socAssigned)
        {
            //Assign
            SetupSocCall(socAssigned);
            var jobprofileConverter = new JobProfileConverter(fakeRelatedClassificationsRepository, fakeDynamicContentExtensions, htbContentPropertyConverter, wywdPropertyConverter);

            var dummyRelatedItems = A.CollectionOfDummy<string>(1).AsEnumerable().AsQueryable();
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedContentUrl(A<DynamicContent>._, A<string>._)).Returns(dummyRelatedItems);

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
          A.CallTo(() => htbContentPropertyConverter.ConvertFrom(A<DynamicContent>._)).MustHaveHappened();

            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, nameof(JobProfile.SalaryStarter)))
                   .MustHaveHappened();
            jobProfile.SalaryStarter.HasValue(10);

            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, nameof(JobProfile.SalaryExperienced)))
                  .MustHaveHappened();
            jobProfile.SalaryExperienced.HasValue(20);

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
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<decimal?>(A<DynamicContent>._, nameof(JobProfile.SalaryStarter)))
               .Returns(10);
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<decimal?>(A<DynamicContent>._, nameof(JobProfile.SalaryExperienced)))
              .Returns(20);
            A.CallTo(() => fakeRelatedClassificationsRepository.GetRelatedClassifications(A<DynamicContent>._, A<string>._, A<string>._)).Returns(new EnumerableQuery<string>(new List<string> { "test" }));
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, A<string>._, A<int>._))
                .Returns(new EnumerableQuery<DynamicContent>(new List<DynamicContent> { fakeDynamicContentItem }));
            A.CallTo(() => htbContentPropertyConverter.ConvertFrom(A<DynamicContent>._)).Returns(new HowToBecome());
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