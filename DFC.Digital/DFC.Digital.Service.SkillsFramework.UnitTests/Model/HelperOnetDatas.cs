using System;
using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Service.SkillsFramework.UnitTests.Model
{
    using Repository.ONET.UnitTests;

    public class HelperOnetDatas : HelperEfOnetDatas
    {
        private static readonly IEnumerable<FrameworkSkill> FrameWorkTranslatedData = new List<FrameworkSkill>
        {
            new FrameworkSkill
            {
                ONetElementId = "1.A.1",
                Description = "problem-solving skills",
                Title = "Problem Skills",
            },
            new FrameworkSkill
            {
                ONetElementId = "1.A.1.a",
                Description = "excellent verbal communication skills",
                Title = "communication skill",

            },
            new FrameworkSkill
            {
                ONetElementId = "1.A.1.b",
                Description = "thinking and reasoning skills",
                Title = "Reasoing",
            },
            new FrameworkSkill
            {
                ONetElementId = "1.A.1.c",
                Description = "maths skills",
                Title = "maths",
            },
            new FrameworkSkill
            {
                ONetElementId = "1.A.1.d",
                Description = "a good memory",
                Title = "memory",
            },
            new FrameworkSkill
            {
                ONetElementId = "1.A.1.g",
                Description = "concentration skills",
                Title = "concentrate",
            },
            new FrameworkSkill
            {
                ONetElementId = "1.A.2",
                Description = "physcial skills like movement, coordination, dexterity and grace",
                Title = "phyisical skill",
            },
            new FrameworkSkill
            {
                ONetElementId = "1.A.2.a",
                Description = "the ability to work well with your hands",
                Title = "ability hand",
            },
        };
        private static readonly IEnumerable<SocCode> SocMappingsData = new List<SocCode>
        {
            new SocCode
            {
                ONetOccupationalCode = "11-3031.02",
                SOCCode = "1150",

            },
            new SocCode
            {
                ONetOccupationalCode = "11-9051.00",
                SOCCode = "1223",

            },
            new SocCode
            {
                ONetOccupationalCode = "19-4092.00",
                SOCCode = "2112",

            },
            new SocCode
            {
                ONetOccupationalCode = "17-2141.00",
                SOCCode = "2122",

            },

        };
        private static readonly IEnumerable<WhatItTakesSkill> TranslatedData = new List<WhatItTakesSkill>
        {
            new WhatItTakesSkill
            {
                Title = "1.A.1",
                Description = "problem-solving skills",
                Contextualised = null,
            },
            new WhatItTakesSkill
            {
                Title = "1.A.1.a",
                Description = "excellent verbal communication skills",
                Contextualised = null,

            },
            new WhatItTakesSkill
            {
                Title = "1.A.1.b",
                Description = "thinking and reasoning skills",
                Contextualised = null,
            },
            new WhatItTakesSkill
            {
                Title = "1.A.1.c",
                Description = "maths skills",
                Contextualised = null,
            },
            new WhatItTakesSkill
            {
                Title = "1.A.1.d",
                Description = "a good memory",
                Contextualised = null,
            },
            new WhatItTakesSkill
            {
                Title = "1.A.1.g",
                Description = "concentration skills",
                Contextualised = null,
            },
            new WhatItTakesSkill
            {
                Title = "1.A.2",
                Description = "physcial skills like movement, coordination, dexterity and grace",
                Contextualised = null,
            },
            new WhatItTakesSkill
            {
                Title = "1.A.2.a",
                Description = "the ability to work well with your hands",
                Contextualised = null,
            },
        };
        public static IEnumerable<object[]> FrameworkTranslationData()
        {
            yield return new object[]
            {
                FrameWorkTranslatedData
            };
        }
        public static IEnumerable<object[]> GetAllSocMappingsData()
        {
            yield return new object[]
            {
                SocMappingsData
            };
        }
    
    }
}