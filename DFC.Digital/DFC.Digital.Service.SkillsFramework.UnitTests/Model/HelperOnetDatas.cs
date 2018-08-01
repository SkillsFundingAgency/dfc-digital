using System;
using System.Collections.Generic;
using System.Linq;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Service.SkillsFramework.UnitTests.Model
{
    using Repository.ONET.UnitTests;

    public class HelperOnetDatas : HelperEfOnetDatas
    {
        internal HelperOnetDatas()
        {

        }
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
        public static IEnumerable<object[]> AttributeDataSet()
        {
            yield return new object[]
            {
               // AttributeData,
                "11-10011.00",
                20
            };
            yield return new object[]
            {
              //  AttributeData,
                "17-2071.00",
                20
            };
        }
        public static IEnumerable<object[]> DigitalSkillsData()
        {
            yield return new object[]
            {
            //    DigitalSkills1150,
                "11-10011.00",
                Convert.ToInt32(RangeChecker.FirstRange),
            };
            yield return new object[]
            {
                //DigitalSkills1223,
                "11-2011.01",
                Convert.ToInt32(RangeChecker.SecondRange),
            };
            yield return new object[]
            {
              //  DigitalSkills2112,
                "41-3031.03",
                Convert.ToInt32(RangeChecker.ThirdRange),
            };
        }
        public static IEnumerable<object[]> DigitalSkillsRank()
        {
            yield return new object[]
            {
                "11-10011.00",
                Convert.ToInt32(RangeChecker.FirstRange),
                1,
            };
            yield return new object[]
            {
                "11-2011.01",
                Convert.ToInt32(RangeChecker.SecondRange),
                2
            };
            yield return new object[]
            {
                "41-3031.03",
                Convert.ToInt32(RangeChecker.ThirdRange),
                3
            };
        }
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
        public static IEnumerable<object[]> GetSocMappingsData()
        {
            yield return new object[]
            {
                DfcSocMappings,
                SocMappingsData
            };
        }
        public static IEnumerable<object[]> OnetAttributesData()
        {
            yield return new object[]
            {
                OnetKnowledges,
                OnetAbilities,
                OnetSkills,
                OnetWorkStyles,
                OnetContentModelReference,
                DfcUnspcsReference,
                "11-1011.00"


            };
        }
        public static IEnumerable<object[]> OnetDigitalSkillsRankData()
        {
            yield return new object[]
            {
                DfcToolsandTechnology,
                DfcUnspcsReference,
                "11-3071.03"
            };
        }
        public static IEnumerable<object[]> OnetTranslationsData()
        {
            yield return new object[]
            {
                    DfcTranslations,
                    TranslatedData,
                    "1.A.1.a"
            };
            yield return new object[]
            {
                    DfcTranslations,
                    TranslatedData,
                    "1.A.1.b"
            };

        }
        public static IEnumerable<object[]> OnetWhatitTakesData()
        {
            yield return new object[]
            {
                DfcTranslations,
                TranslatedData.ToList()[0],
                "1.A.1"
            };
            yield return new object[]
            {
                DfcTranslations,
                TranslatedData.ToList()[1],
                "1.A.1.a"
            };
            yield return new object[]
            {
                DfcTranslations,
                TranslatedData.ToList()[2],
                "1.A.1.b"
            };

        }
        public static IEnumerable<object[]> OnetWhatitTakesManyData()
        {
            yield return new object[]
            {
                DfcTranslations,
                TranslatedData.ToList()[0],
                "1.A.1",
                "problem-solving skills"
            };
            yield return new object[]
            {
                DfcTranslations,
                TranslatedData.ToList()[1],
                "1.A.1.a",
                "excellent verbal communication skills"
            };
            yield return new object[]
            {
                DfcTranslations,
                TranslatedData.ToList()[2],
                "1.A.1.b",
                "thinking and reasoning skills"
            };

        }
        public static IEnumerable<object[]> TranslationData()
        {
            yield return new object[]
            {
                TranslatedData
            };
        }
    }
}