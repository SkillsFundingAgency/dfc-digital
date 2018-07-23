using DFC.Digital.Core;
using DFC.Digital.Core.Logging;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using FakeItEasy;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public class JobProfileRelatedSkillConverterTests
    {
        private readonly IDynamicContentExtensions fakeDynamicContentExtensions;
        private readonly DynamicContent fakeDynamicContentItem;

        public JobProfileRelatedSkillConverterTests()
        {
            fakeDynamicContentExtensions = A.Fake<IDynamicContentExtensions>();
            fakeDynamicContentItem = A.Dummy<DynamicContent>();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ConvertFromTest(bool hasValidSkill)
        {
            //set up calls
            var jobProfileRelatedSkillConverter = new JobProfileRelatedSkillConverter(fakeDynamicContentExtensions);
            var expectedSkillDescription = "TestSkillDescription";

            var dummysRelatedSkills = hasValidSkill ? A.CollectionOfDummy<DynamicContent>(1).AsEnumerable().AsQueryable() : Enumerable.Empty<DynamicContent>().AsQueryable();
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, A<string>._, A<int>._)).Returns(dummysRelatedSkills);

            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, A<string>._))
               .Returns("DummyText");

            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, nameof(WhatItTakesSkill.Description)))
                .Returns(hasValidSkill ? expectedSkillDescription : null);

            var relatedSkill = jobProfileRelatedSkillConverter.ConvertFrom(fakeDynamicContentItem);

            //Assert
            if (hasValidSkill)
            {
                relatedSkill.Description.Should().BeEquivalentTo(expectedSkillDescription);
            }
            else
            {
                relatedSkill.Should().BeNull();
            }
        }

        [Fact]
        public void ConvertFromLoggedExceptionTest()
        {
            //set up calls
            var jobProfileRelatedSkillConverter = new JobProfileRelatedSkillConverter(fakeDynamicContentExtensions);
            var dummysRelatedSkills = A.CollectionOfDummy<DynamicContent>(1).AsEnumerable().AsQueryable();
            A.CallTo(() => fakeDynamicContentExtensions.GetRelatedItems(A<DynamicContent>._, A<string>._, A<int>._)).Returns(dummysRelatedSkills);
            A.CallTo(() => fakeDynamicContentExtensions.GetFieldValue<Lstring>(A<DynamicContent>._, nameof(WhatItTakesSkill.Description))).Throws(new LoggedException());

            var relatedSkill = jobProfileRelatedSkillConverter.ConvertFrom(fakeDynamicContentItem);

            //Assert
            {
                relatedSkill.Should().BeNull();
            }
        }

        private List<DynamicContent> GetRelatedContentSkills()
        {
            var dummyRelatedSkills = new List<DynamicContent>() { new DynamicContent(System.Guid.NewGuid(), "TestApp") };
            return dummyRelatedSkills;
        }
    }
}
