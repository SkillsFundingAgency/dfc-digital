using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Service.SkillsFramework.UnitTests.Model
{
    using Repository.ONET;

    public class HelperOnetDatas
    {
        private static readonly IEnumerable<DfcOnetSocMappings> SocMappingsData = new List<DfcOnetSocMappings>
        {
            new DfcOnetSocMappings
            {
                JobProfile = "Bank Manager",
                OnetSocCode = "11-3031.02",
                QualityRating = 2,
                SocCode = "1150"
            },
            new DfcOnetSocMappings
            {
                JobProfile = "Restaurant manager",
                OnetSocCode = "11-9051.00",
                QualityRating = 2,
                SocCode = "1223"
            },
            new DfcOnetSocMappings
            {
                JobProfile = "Forensic scientist",
                OnetSocCode = "19-4092.00",
                QualityRating = 2,
                SocCode = "2112"
            },
            new DfcOnetSocMappings
            {
                JobProfile = "Mechanical engineerr",
                OnetSocCode = "17-2141.00",
                QualityRating = 2,
                SocCode = "2122"
            },
            new DfcOnetSocMappings
            {
                JobProfile = "Electrical engineer",
                OnetSocCode = "17-2071.00",
                QualityRating = 2,
                SocCode = "2123"
            },

        };

        private static readonly IEnumerable<DfcOnetTranslation> TranslatedData = new List<DfcOnetTranslation>
        {
            new DfcOnetTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.1",
                Translation = "problem-solving skills"
            },
            new DfcOnetTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.1.a",
                Translation = "excellent verbal communication skills"
            },
            new DfcOnetTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.1.b",
                Translation = "thinking and reasoning skills"
            },
            new DfcOnetTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.1.c",
                Translation = "maths skills"
            },
            new DfcOnetTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.1.d",
                Translation = "a good memory"
            },
            new DfcOnetTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.1.g",
                Translation = "concentration skills"
            },
            new DfcOnetTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.2",
                Translation = "physcial skills like movement, coordination, dexterity and grace"
            },
            new DfcOnetTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.2.a",
                Translation = "the ability to work well with your hands"
            },
        };

        private static readonly IEnumerable<DfcOnetAttributesData> AttributeData = new List<DfcOnetAttributesData>
        {
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Abilities,
                ElementDescription = "Enduring attributes of the individual that influence performance",
                ElementId = "1.A",
                ElementName = "Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Abilities,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "1.A.1",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Abilities,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "1.A.2",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Abilities,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "1.A.3",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Abilities,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "1.A.4",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Knowledge,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "2.A.1",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Knowledge,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "2.A.2",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Knowledge,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "2.A.3",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Knowledge,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "2.A.4",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Knowledge,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "2.A.5",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.WorkStyles,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "3.A.1",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.WorkStyles,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "3.A.2",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.WorkStyles,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "3.A.3",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.WorkStyles,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "3.A.4",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.WorkStyles,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "3.A.5",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Skills,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "4.A.1",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Skills,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "4.A.2",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Skills,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "4.A.3",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Skills,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "4.A.4",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
            new DfcOnetAttributesData()
            {
                Attribute = Attributes.Skills,
                ElementDescription = "Abilities that influence the acquisition and application of knowledge in problem solving",
                ElementId = "4.A.5",
                ElementName = "Cognitive Abilities",
                OnetSocCode = "17-2071.00",
                SocCode = "1150",
                Value = (decimal) 4.58
            },
        };

        private static readonly DfcOnetDigitalSkills DigitalSkills1150 = new DfcOnetDigitalSkills
        {
            DigitalSkillsCollection = new List<DfcOnetToolsAndTechnology>
            {
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Personal Communication Device",
                    ElementId = "",
                    OnetSocCode = "11-10011.00",
                    SocCode = "1150",
                    T2Example = "Smartphones",
                    T2Type = "Tools"
                },
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Media storage devices",
                    ElementId = "",
                    OnetSocCode = "11-10011.00",
                    SocCode = "1150",
                    T2Example = "Universal serial bus USB flash drives",
                    T2Type = "Tools"
                },
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Computers",
                    ElementId = "",
                    OnetSocCode = "11-10011.00",
                    SocCode = "1150",
                    T2Example = "Laptop computers",
                    T2Type = "Tools"
                },
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Computers",
                    ElementId = "",
                    OnetSocCode = "11-10011.00",
                    SocCode = "1150",
                    T2Example = "Personal Digital Assistance PDA",
                    T2Type = "Tools"
                },

            },
            DigitalSkillsCount = Convert.ToInt32(RangeChecker.FirstRange)
        };

        private static readonly DfcOnetDigitalSkills DigitalSkills1223 = new DfcOnetDigitalSkills
        {
            DigitalSkillsCollection = new List<DfcOnetToolsAndTechnology>
            {
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Computer data input devices",
                    ElementId = "",
                    OnetSocCode = "11-2011.01",
                    SocCode = "1223",
                    T2Example = "Computer data input scanners",
                    T2Type = "Tools"
                },
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Computers",
                    ElementId = "",
                    OnetSocCode = "11-2011.01",
                    SocCode = "1223",
                    T2Example = "Desktop computers",
                    T2Type = "Tools"
                },
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Computer data input devices",
                    ElementId = "",
                    OnetSocCode = "11-2011.01",
                    SocCode = "1223",
                    T2Example = "Handheld computers",
                    T2Type = "Tools"
                },
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Computers",
                    ElementId = "",
                    OnetSocCode = "11-2011.01",
                    SocCode = "1223",
                    T2Example = "Laptop computers",
                    T2Type = "Tools"
                },
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Media storage devices",
                    ElementId = "",
                    OnetSocCode = "11-2011.01",
                    SocCode = "1223",
                    T2Example = "Universal serial bus USB flash drives",
                    T2Type = "Tools"
                },
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Duplicating machines",
                    ElementId = "",
                    OnetSocCode = "11-2011.01",
                    SocCode = "1223",
                    T2Example = "Laser facsimile machines",
                    T2Type = "Tools"
                },
            },
            DigitalSkillsCount = Convert.ToInt32(RangeChecker.SecondRange)
        };

        private static readonly DfcOnetDigitalSkills DigitalSkills2112 = new DfcOnetDigitalSkills
        {
            DigitalSkillsCollection = new List<DfcOnetToolsAndTechnology>
            {
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Computer data input devices",
                    ElementId = "",
                    OnetSocCode = "41-3031.03",
                    SocCode = "2112",
                    T2Example = "Computer data input scanners",
                    T2Type = "Tools"
                },
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Computers",
                    ElementId = "",
                    OnetSocCode = "41-3031.03",
                    SocCode = "2112",
                    T2Example = "Desktop computers",
                    T2Type = "Tools"
                },
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Computers",
                    ElementId = "",
                    OnetSocCode = "41-3031.03",
                    SocCode = "2112",
                    T2Example = "Laptop computers",
                    T2Type = "Tools"
                },
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Duplicating machines",
                    ElementId = "",
                    OnetSocCode = "41-3031.03",
                    SocCode = "2112",
                    T2Example = "Laser facsimile machines",
                    T2Type = "Tools"
                },
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Personal communication devices",
                    ElementId = "",
                    OnetSocCode = "41-3031.03",
                    SocCode = "2112",
                    T2Example = "Multi-line telephone systems",
                    T2Type = "Tools"
                },
                new DfcOnetToolsAndTechnology
                {
                    ClassTitle = "Finance accounting and enterprise resource planning ERP software",
                    ElementId = "",
                    OnetSocCode = "41-3031.03",
                    SocCode = "2112",
                    T2Example = "Bloomberg Professional",
                    T2Type = "Technology"
                },
            },
            DigitalSkillsCount = Convert.ToInt32(RangeChecker.ThirdRange)
        };

        public static IEnumerable<object[]> SocMappings()
        {
            yield return new object[]
            {
                SocMappingsData
            };
        }

        public static IEnumerable<object[]> TranslationData()
        {
            yield return new object[]
            {
                TranslatedData
            };
        }

        public static IEnumerable<object[]> DigitalSkillsData()
        {
            yield return new object[]
            {
                DigitalSkills1150,
                "11-10011.00",
                Convert.ToInt32(RangeChecker.FirstRange),
            };
            yield return new object[]
            {
                DigitalSkills1223,
                "11-2011.01",
                Convert.ToInt32(RangeChecker.SecondRange),
            };
            yield return new object[]
            {
                DigitalSkills2112,
                "41-3031.03",
                Convert.ToInt32(RangeChecker.ThirdRange),
            };
        }
        public static IEnumerable<object[]> DigitalSkillsRank()
        {
            yield return new object[]
            {
                DigitalSkills1150,
                "11-10011.00",
                Convert.ToInt32(RangeChecker.FirstRange),
                1,
            };
            yield return new object[]
            {
                DigitalSkills1223,
                "11-2011.01",
                Convert.ToInt32(RangeChecker.SecondRange),
                2
            };
            yield return new object[]
            {
                DigitalSkills2112,
                "41-3031.03",
                Convert.ToInt32(RangeChecker.ThirdRange),
                3
            };
        }

        public static IEnumerable<object[]> AttributeDataSet()
        {
            yield return new object[]
            {
                AttributeData,
                "11-10011.00",
                20
            };
            yield return new object[]
            {
                AttributeData,
                "17-2071.00",
                20
            };
        }
    }
}
