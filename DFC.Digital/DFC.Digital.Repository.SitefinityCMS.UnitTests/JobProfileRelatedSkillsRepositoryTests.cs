using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS.Modules;
using FakeItEasy;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.GenericContent.Model;
using Xunit;

namespace DFC.Digital.Repository.SitefinityCMS.UnitTests
{
    public class JobProfileRelatedSkillsRepositoryTests
    {
        private IDynamicModuleConverter<WhatItTakesSkill> fakeConverter;
        private IDynamicModuleRepository<WhatItTakesSkill> fakeRepository;

        public JobProfileRelatedSkillsRepositoryTests()
        {
            fakeConverter = A.Fake<IDynamicModuleConverter<WhatItTakesSkill>>();
            fakeRepository = A.Fake<IDynamicModuleRepository<WhatItTakesSkill>>();
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void GetContextualisedSkillsByIdTests(bool vaidSkillsIdCollection)
        {
            var dummyWhatItTakesSkill = A.Dummy<WhatItTakesSkill>();
            A.CallTo(() => fakeConverter.ConvertFrom(A<DynamicContent>._)).Returns(dummyWhatItTakesSkill);

            var jobProfileRelatedSkillsRepository = new JobProfileRelatedSkillsRepository(fakeRepository, fakeConverter);

            if (vaidSkillsIdCollection)
            {
              jobProfileRelatedSkillsRepository.GetContextualisedSkillsById(new List<string>() { "dummyRelatedSkillId" }).ToList();
                A.CallTo(() => fakeConverter.ConvertFrom(A<DynamicContent>._)).MustHaveHappened();
            }
            else
            {
               jobProfileRelatedSkillsRepository.GetContextualisedSkillsById(null).ToList();
                A.CallTo(() => fakeConverter.ConvertFrom(A<DynamicContent>._)).MustNotHaveHappened();
            }
        }
    }
}
