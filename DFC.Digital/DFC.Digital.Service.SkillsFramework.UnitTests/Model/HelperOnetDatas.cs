namespace DFC.Digital.Repository.ONET.Tests.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data.Model;
    using DataModel;
    using Service.SkillsFramework;

    public class HelperOnetDatas : HelperEFOnetDatas
    {
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

        //private static readonly IEnumerable<WhatItTakesSkill> AttributeData = new List<DfcOnetAttributesData>
        //{
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Abilities,
        //        ElementDescription = "Enduring attributes of the individual that influence performance",
        //        ElementId = "1.A",
        //        ElementName = "Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Abilities,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "1.A.1",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Abilities,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "1.A.2",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Abilities,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "1.A.3",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Abilities,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "1.A.4",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Knowledge,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "2.A.1",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Knowledge,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "2.A.2",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Knowledge,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "2.A.3",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Knowledge,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "2.A.4",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Knowledge,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "2.A.5",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.WorkStyles,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "3.A.1",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.WorkStyles,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "3.A.2",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.WorkStyles,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "3.A.3",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.WorkStyles,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "3.A.4",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.WorkStyles,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "3.A.5",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Skills,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "4.A.1",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Skills,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "4.A.2",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Skills,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "4.A.3",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Skills,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "4.A.4",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //    new DfcOnetAttributesData()
        //    {
        //        Attribute = Attributes.Skills,
        //        ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
        //        ElementId = "4.A.5",
        //        ElementName = "Cognitive Abilities",
        //        OnetSocCode = "17-2071.00",
        //        SocCode = "1150",
        //        Value = (decimal) 4.58
        //    },
        //};

        //private static readonly DigitalSkill DigitalSkills1150 = new DigitalSkill
        //{
        //    DigitalSkillsCollection = new List<DigitalToolsAndTechnology>
        //    {
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Personal Communication Device",
        //            ElementId = "",
        //            OnetSocCode = "11-10011.00",
        //            SocCode = "1150",
        //            T2Example = "Smartphones",
        //            T2Type = "Tools"
        //        },
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Media storage devices",
        //            ElementId = "",
        //            OnetSocCode = "11-10011.00",
        //            SocCode = "1150",
        //            T2Example = "Universal serial bus USB flash drives",
        //            T2Type = "Tools"
        //        },
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Computers",
        //            ElementId = "",
        //            OnetSocCode = "11-10011.00",
        //            SocCode = "1150",
        //            T2Example = "Laptop computers",
        //            T2Type = "Tools"
        //        },
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Computers",
        //            ElementId = "",
        //            OnetSocCode = "11-10011.00",
        //            SocCode = "1150",
        //            T2Example = "Personal Digital Assistance PDA",
        //            T2Type = "Tools"
        //        },

        //    },
        //    DigitalSkillsCount = Convert.ToInt32(RangeChecker.FirstRange)
        //};

        //private static readonly DigitalSkill DigitalSkills1223 = new DigitalSkill
        //{
        //    DigitalSkillsCollection = new List<DigitalToolsAndTechnology>
        //    {
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Computer data input devices",
        //            ElementId = "",
        //            OnetSocCode = "11-2011.01",
        //            SocCode = "1223",
        //            T2Example = "Computer data input scanners",
        //            T2Type = "Tools"
        //        },
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Computers",
        //            ElementId = "",
        //            OnetSocCode = "11-2011.01",
        //            SocCode = "1223",
        //            T2Example = "Desktop computers",
        //            T2Type = "Tools"
        //        },
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Computer data input devices",
        //            ElementId = "",
        //            OnetSocCode = "11-2011.01",
        //            SocCode = "1223",
        //            T2Example = "Handheld computers",
        //            T2Type = "Tools"
        //        },
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Computers",
        //            ElementId = "",
        //            OnetSocCode = "11-2011.01",
        //            SocCode = "1223",
        //            T2Example = "Laptop computers",
        //            T2Type = "Tools"
        //        },
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Media storage devices",
        //            ElementId = "",
        //            OnetSocCode = "11-2011.01",
        //            SocCode = "1223",
        //            T2Example = "Universal serial bus USB flash drives",
        //            T2Type = "Tools"
        //        },
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Duplicating machines",
        //            ElementId = "",
        //            OnetSocCode = "11-2011.01",
        //            SocCode = "1223",
        //            T2Example = "Laser facsimile machines",
        //            T2Type = "Tools"
        //        },
        //    },
        //    DigitalSkillsCount = Convert.ToInt32(RangeChecker.SecondRange)
        //};

        //private static readonly DigitalSkill DigitalSkills2112 = new DigitalSkill
        //{
        //    DigitalSkillsCollection = new List<DigitalToolsAndTechnology>
        //    {
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Computer data input devices",
        //            ElementId = "",
        //            OnetSocCode = "41-3031.03",
        //            SocCode = "2112",
        //            T2Example = "Computer data input scanners",
        //            T2Type = "Tools"
        //        },
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Computers",
        //            ElementId = "",
        //            OnetSocCode = "41-3031.03",
        //            SocCode = "2112",
        //            T2Example = "Desktop computers",
        //            T2Type = "Tools"
        //        },
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Computers",
        //            ElementId = "",
        //            OnetSocCode = "41-3031.03",
        //            SocCode = "2112",
        //            T2Example = "Laptop computers",
        //            T2Type = "Tools"
        //        },
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Duplicating machines",
        //            ElementId = "",
        //            OnetSocCode = "41-3031.03",
        //            SocCode = "2112",
        //            T2Example = "Laser facsimile machines",
        //            T2Type = "Tools"
        //        },
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Personal communication devices",
        //            ElementId = "",
        //            OnetSocCode = "41-3031.03",
        //            SocCode = "2112",
        //            T2Example = "Multi-line telephone systems",
        //            T2Type = "Tools"
        //        },
        //        new DigitalToolsAndTechnology
        //        {
        //            ClassTitle = "Finance accounting and enterprise resource planning ERP software",
        //            ElementId = "",
        //            OnetSocCode = "41-3031.03",
        //            SocCode = "2112",
        //            T2Example = "Bloomberg Professional",
        //            T2Type = "Technology"
        //        },
        //    },
        //    DigitalSkillsCount = Convert.ToInt32(RangeChecker.ThirdRange)
        //};

        //public static IEnumerable<object[]> SocMappings()
        //{
        //    yield return new object[]
        //    {
        //        SocMappingsData
        //    };
        //}

        public static IEnumerable<object[]> TranslationData()
        {
            yield return new object[]
            {
                TranslatedData
            };
        }
        public static IEnumerable<object[]> FrameworkTranslationData()
        {
            yield return new object[]
            {
                FrameWorkTranslatedData
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
              //  DigitalSkills1150,
                "11-10011.00",
                Convert.ToInt32(RangeChecker.FirstRange),
                1,
            };
            yield return new object[]
            {
             //   DigitalSkills1223,
                "11-2011.01",
                Convert.ToInt32(RangeChecker.SecondRange),
                2
            };
            yield return new object[]
            {
                //DigitalSkills2112,
                "41-3031.03",
                Convert.ToInt32(RangeChecker.ThirdRange),
                3
            };
        }

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


        public static IEnumerable<object[]> GetSocMappingsData()
        {
            yield return new object[]
            {
                DfcSocMappings,
                SocMappingsData
            };
        }
        public static IEnumerable<object[]> GetAllSocMappingsData()
        {
            yield return new object[]
            {
                SocMappingsData
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

        public static IEnumerable<object[]> OnetDigitalSkillsRankData()
        {
            yield return new object[]
            {
                DfcToolsandTechnology,
                DfcUnspcsReference,
                "11-3071.03"
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
    }

    public class HelperEFOnetDatas
    {
        protected static readonly IEnumerable<DFC_SocMappings> DfcSocMappings = new List<DFC_SocMappings>
        {
            new DFC_SocMappings
            {
                ONetCode = "11-3031.02",
                SocCode = "1150",

            },
            new DFC_SocMappings
            {
                ONetCode = "11-9051.00",
                SocCode = "1223",

            },
            new DFC_SocMappings
            {
                ONetCode = "19-4092.00",
                SocCode = "2112",

            },
            new DFC_SocMappings
            {
                ONetCode = "17-2141.00",
                SocCode = "2122",

            },

        };


        protected static readonly IEnumerable<DFC_GDSTranlations> DfcTranslations = new List<DFC_GDSTranlations>
        {
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1",
                translation = "problem-solving skills"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.a",
                translation = "excellent verbal communication skills"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.b",
                translation = "thinking and reasoning skills"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.c",
                translation = "maths skills "
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.d",
                translation = "a good memory"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.1.g",
                translation = "concentration skills"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.2",
                translation = "physcial skills like movement, coordination, dexterity and grace"
            },
            new DFC_GDSTranlations
            {
                datetimestamp = DateTime.Today,
                onet_element_id = "1.A.2.a",
                translation = "the ability to work well with your hands"
            },

        };

        protected static readonly IEnumerable<tools_and_technology> DfcToolsandTechnology = new List<tools_and_technology>
        {
            new tools_and_technology()
            {
                commodity_code = 44101805,
                hot_technology = "Y",
                occupation_data = new occupation_data(),
                onetsoc_code = "11-3071.03",
                t2_type = "Tools",
                t2_example = "Laptop small"
            },
            new tools_and_technology()
            {
                commodity_code = 44101806,
                hot_technology = "Y",
                occupation_data = new occupation_data(),
                onetsoc_code = "11-3071.04",
                t2_type = "Tools",
                t2_example = "Laptop big"
            },
            new tools_and_technology()
            {
                commodity_code = 44101807,
                hot_technology = "Y",
                occupation_data = new occupation_data(),
                onetsoc_code = "11-3071.05",
                t2_type = "Tools",
                t2_example = "Laptop medium"
            },
            new tools_and_technology()
            {
                commodity_code = 44101808,
                hot_technology = "Y",
                occupation_data = new occupation_data(),
                onetsoc_code = "11-3071.06",
                t2_type = "Technology",
                t2_example = "tablet C"
            },
            new tools_and_technology()
            {
                commodity_code = 44101819,
                hot_technology = "Y",
                occupation_data = new occupation_data(),
                onetsoc_code = "11-3071.07",
                t2_type = "Technology",
                t2_example = "Laptop B"
            },

        };

        protected static readonly IEnumerable<unspsc_reference> DfcUnspcsReference = new List<unspsc_reference>
        {
            new unspsc_reference()
            {
                class_code = 44101800,
                class_title = "Processing machine",
                commodity_code = 44101805,
                commodity_title = "computing machine",
                family_code = 44100000,
                family_title = "office machine",
                segment_code = 4400000,
                segment_title = "office equipment",
                tools_and_technology = DfcToolsandTechnology.ToList()
            },
            new unspsc_reference()
            {
                class_code = 44101800,
                class_title = "Processing machine",
                commodity_code = 44101806,
                commodity_title = "computing machine",
                family_code = 44100000,
                family_title = "office machine",
                segment_code = 4400000,
                segment_title = "office equipment",
                tools_and_technology = DfcToolsandTechnology.ToList()
            },
            new unspsc_reference()
            {
                class_code = 44101800,
                class_title = "Processing machine",
                commodity_code = 44101807,
                commodity_title = "computing machine",
                family_code = 44100000,
                family_title = "office machine",
                segment_code = 4400000,
                segment_title = "office equipment",
                tools_and_technology = DfcToolsandTechnology.ToList()
            },
            new unspsc_reference()
            {
                class_code = 44101800,
                class_title = "Processing machine",
                commodity_code = 44101808,
                commodity_title = "computing machine",
                family_code = 44100000,
                family_title = "office machine",
                segment_code = 4400000,
                segment_title = "office equipment",
                tools_and_technology = DfcToolsandTechnology.ToList()
            },
            new unspsc_reference()
            {
                class_code = 44101800,
                class_title = "Processing machine",
                commodity_code = 44101819,
                commodity_title = "computing machine",
                family_code = 44100000,
                family_title = "office machine",
                segment_code = 4400000,
                segment_title = "office equipment",
                tools_and_technology = DfcToolsandTechnology.ToList()
            }
        };

        protected static readonly IEnumerable<content_model_reference> OnetContentModelReference =
            new List<content_model_reference>()
            {
                new content_model_reference()
                {
                    element_id = "1.A.1.a",
                    element_name = "Mathematics",
                    description = "Worker Characteristics"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1a",
                    element_name = "abilties",
                    description = "Worker Characteristics"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1.b",
                    element_name = "workstyle",
                    description = "Workestyle Characteristics aa"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1.c",
                    element_name = "knowledge",
                    description = "Worker Characteristics a"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1.d",
                    element_name = "Mathematics",
                    description = "skills b"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1.e",
                    element_name = "skills",
                    description = "Worker Characteristics d"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1.g",
                    element_name = "skills",
                    description = "Worker Characteristics"
                },
                new content_model_reference()
                {
                    element_id = "1.A.1.h",
                    element_name = "skills ",
                    description = "Worker Characteristics e"
                },
            };
        protected static readonly IEnumerable<knowledge> OnetKnowledges = new List<knowledge>()
        {
            new knowledge()
            {
                content_model_reference = OnetContentModelReference.ToList()[0],
                data_value = (decimal)4.7,
                onetsoc_code = "11-1011.00",
                element_id = "1.A.1.a",
                not_relevant = "N",
                recommend_suppress = "N",
                occupation_data = new occupation_data(),
                scale_id = "IM"
            },
            new knowledge()
            {
                content_model_reference = OnetContentModelReference.ToList()[1],
                data_value = (decimal)4.7,
                onetsoc_code = "11-1011.00",
                element_id = "1.A.1.a",
                not_relevant = "N",
                recommend_suppress = "N",
                occupation_data = new occupation_data(),
                scale_id = "IM"
            },
            new knowledge()
            {
            content_model_reference = OnetContentModelReference.ToList()[2],
            data_value = (decimal)4.7,
            onetsoc_code = "11-1011.00",
            element_id = "1.A.1.a",
            not_relevant = "N",
            recommend_suppress = "N",
            occupation_data = new occupation_data(),
            scale_id = "IM"
            },
            new knowledge()
            {
                content_model_reference = OnetContentModelReference.ToList()[3],
                data_value = (decimal)4.7,
                onetsoc_code = "11-1011.00",
                element_id = "1.A.1.a",
                not_relevant = "N",
                recommend_suppress = "N",
                occupation_data = new occupation_data(),
                scale_id = "IM"
            },
            new knowledge()
            {
                content_model_reference = OnetContentModelReference.ToList()[4],
                data_value = (decimal)4.7,
                onetsoc_code = "11-1011.00",
                element_id = "1.A.1.a",
                not_relevant = "N",
                recommend_suppress = "N",
                occupation_data = new occupation_data(),
                scale_id = "IM"
            },
            new knowledge()
            {
                content_model_reference = OnetContentModelReference.ToList()[5],
                data_value = (decimal)4.7,
                onetsoc_code = "11-1011.00",
                element_id = "1.A.1.a",
                not_relevant = "N",
                recommend_suppress = "N",
                occupation_data = new occupation_data(),
                scale_id = "IM"
            }
        };

        protected static readonly IEnumerable<skill> OnetSkills = new List<skill>
        {
            new skill()
            {
                content_model_reference = OnetContentModelReference.ToList()[0],
                data_value = (decimal)4.7,
                date_updated = DateTime.Today,
                element_id = "1.A.1.a",
                not_relevant = "N",
                recommend_suppress = "N",
                onetsoc_code = "11-1011.00",
                scale_id = "IM"
            },
            new skill()
            {
                content_model_reference = OnetContentModelReference.ToList()[0],
                data_value = (decimal)4.7,
                date_updated = DateTime.Today,
                element_id = "1.A.1.d",
                not_relevant = "N",
                recommend_suppress = "N",
                onetsoc_code = "11-1011.00",
                scale_id = "LV"
            },
        };

        protected static readonly IEnumerable<ability> OnetAbilities = new List<ability>()
        {
            new ability()
            {
                content_model_reference = OnetContentModelReference.ToList()[0],
                data_value = (decimal)4.7,
                date_updated = DateTime.Today,
                element_id = "1.A.1.a",
                not_relevant = "N",
                recommend_suppress = "N",
                scale_id = "IM",
                onetsoc_code = "11-1011.00",
                occupation_data = new occupation_data(),
            },
            new ability()
            {
                content_model_reference = OnetContentModelReference.ToList()[0],
                data_value = (decimal)4.7,
                date_updated = DateTime.Today,
                element_id = "1.A.1.d",
                not_relevant = "N",
                recommend_suppress = "N",
                scale_id = "LV",
                onetsoc_code = "11-1011.00",
                occupation_data = new occupation_data(),
            },
        };

        protected static readonly IEnumerable<work_styles> OnetWorkStyles = new List<work_styles>()
        {
            new work_styles()
            {
                content_model_reference = OnetContentModelReference.ToList()[0],
                data_value = (decimal)4.7,
                date_updated = DateTime.Today,
                element_id = "1.A.1.a",
                recommend_suppress = "N",
                scale_id = "IM",
                onetsoc_code = "11-1011.00",
                occupation_data = new occupation_data(),
            },
            new work_styles()
            {
                content_model_reference = OnetContentModelReference.ToList()[0],
                data_value = (decimal)4.7,
                date_updated = DateTime.Today,
                element_id = "1.A.1.d",
                recommend_suppress = "N",
                scale_id = "LV",
                onetsoc_code = "11-1011.00",
                occupation_data = new occupation_data(),
            },
        };

    }
}