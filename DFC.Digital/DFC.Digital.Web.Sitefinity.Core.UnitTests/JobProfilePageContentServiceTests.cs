using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Telerik.Sitefinity.Mvc.Proxy;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.Core.Utility.Tests
{
    public class JobProfilePageContentServiceTests
    {
        [Fact]
        public void GetJobProfileSectionsTest()
        {
            //Fakes and dummies
            var dummyJobProfileSectionFilters = new JobProfileSectionFilter[]
            {
                new JobProfileSectionFilter
                {
                    ContentFieldMember = "ContentField",
                    SectionCaption = "Section",
                    TitleMember = "Title"
                },
                new JobProfileSectionFilter
                {
                    ContentFieldMember = "AnotherContentField",
                    SectionCaption = "AnotherSection",
                    TitleMember = "AnotherTitle",
                    SubFilters = new[] { "filter1", "filter2" }
                },
            };

            var expectedJobProfileSection = new JobProfileSection[]
            {
                new JobProfileSection
                {
                    ContentField = "ContentField",
                    Title = "Title"
                },
                new JobProfileSection
                {
                    ContentField = "AnotherContentField",
                    Title = "AnotherTitle"
                },
            };

            var dummyValues = new Dictionary<string, object>();
            dummyValues.Add("Title", "Title");
            dummyValues.Add("ContentField", "ContentField");
            dummyValues.Add("AnotherTitle", "AnotherTitle");
            dummyValues.Add("AnotherContentField", "AnotherContentField");

            var fakeSettings = A.Fake<DummySettings>();
            A.CallTo(() => fakeSettings.Values).Returns(dummyValues);

            var mvcControllerFake = A.Fake<MvcControllerProxy>();
            A.CallTo(() => mvcControllerFake.Settings).Returns(fakeSettings);

            var dummyWidgets = new KeyValuePair<string, MvcControllerProxy>[]
            {
                new KeyValuePair<string, MvcControllerProxy>("Section", mvcControllerFake),
                new KeyValuePair<string, MvcControllerProxy>("AnotherSection", mvcControllerFake),
            };

            Guid one = Guid.NewGuid(), two = Guid.NewGuid();
            var fakeSitefinityPage = A.Fake<ISitefinityPage>(o => o.Strict());
            A.CallTo(() => fakeSitefinityPage.GetControlsInOrder(A<IEnumerable<string>>._)).Returns(new Guid[] { one, two });
            A.CallTo(() => fakeSitefinityPage.GetControlOnPageByCaption(A<IEnumerable<string>>._)).Returns(new Guid[] { one, two });
            A.CallTo(() => fakeSitefinityPage.GetWidgets(A<IEnumerable<Guid>>._)).Returns(dummyWidgets);

            //Instantiate and act
            var jpContentService = new JobProfilePageContentService(fakeSitefinityPage);
            var result = jpContentService.GetJobProfileSections(dummyJobProfileSectionFilters);

            //Assert
            expectedJobProfileSection.Should().BeEquivalentTo(result, opt => opt.WithStrictOrdering());
            A.CallTo(() => fakeSettings.Values).MustHaveHappened();
            A.CallTo(() => mvcControllerFake.Settings).MustHaveHappened();
            A.CallTo(() => fakeSitefinityPage.GetControlsInOrder(A<IEnumerable<string>>._)).MustHaveHappened();
            A.CallTo(() => fakeSitefinityPage.GetControlOnPageByCaption(A<IEnumerable<string>>._)).MustHaveHappened();
            A.CallTo(() => fakeSitefinityPage.GetWidgets(A<IEnumerable<Guid>>._)).MustHaveHappened();
        }
    }
}