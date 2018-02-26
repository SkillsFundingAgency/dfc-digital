﻿using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Base;
using FakeItEasy;
using FluentAssertions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules.Tests
{
    public class JobProfileConverterTests
    {
        private readonly IRelatedClassificationsRepository relatedClassificationsRepository;

        //[Fact]
        //Cannot Unit test as unable to fake the calls to dynamicContentFake . GetRelatedItems as it is a static extension on object!!! in sitefinity.
        public void GetRelatedContentIdAndUrlTest()
        {
            var dynamicContentFake = A.Fake<DynamicContent>();
            var jobProfileConverter = new JobProfileConverter(relatedClassificationsRepository);

            jobProfileConverter.GetRelatedContentIdAndUrl(dynamicContentFake, "something");
        }

        //[Fact()]
        //Cannot Unit test as unable to fake the calls to dynamicContentFake.GetValue  as its a stiatic in sitefinity.
        public void DynamicModuleRepositoryTest()
        {
            var dynamicContentFake = A.Fake<DynamicContent>();
            var jobProfileConverter = new JobProfileConverter(relatedClassificationsRepository);

            var expectedJobProfile = new JobProfile
            {
                Title = $"dummy{nameof(JobProfile.Title)}",
                AlternativeTitle = $"dummy{nameof(JobProfile.Title)}",
                SalaryRange = $"dummy{nameof(JobProfile.SalaryRange)}",
                Overview = $"dummy{nameof(JobProfile.Overview)}"
            };

            A.CallTo(() => dynamicContentFake.GetValue<Lstring>(nameof(JobProfile.Title))).Returns(expectedJobProfile.Title);
            A.CallTo(() => dynamicContentFake.GetValue<Lstring>(nameof(JobProfile.AlternativeTitle))).Returns(expectedJobProfile.AlternativeTitle);

            A.CallTo(() => dynamicContentFake.GetValue<Lstring>(nameof(JobProfile.SalaryRange))).Returns(expectedJobProfile.SalaryRange);
            A.CallTo(() => dynamicContentFake.GetValue<Lstring>(nameof(JobProfile.Overview))).Returns(expectedJobProfile.Overview);

            var returnedJobProfile = jobProfileConverter.ConvertFrom(dynamicContentFake);

            A.CallTo(() => dynamicContentFake.GetValue<Lstring>(A<Lstring>._)).MustHaveHappened(Repeated.Exactly.Times(3));
            returnedJobProfile.ShouldBeEquivalentTo(expectedJobProfile);
        }
    }
}