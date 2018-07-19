using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;

namespace DFC.Digital.Service.SkillsFramework.UnitTests.Model
{
    public class HelperOnetDatas
    {
        private static readonly IEnumerable<DfcGdsSocMappings> SocMappingsData = new List<DfcGdsSocMappings>
        {
            new DfcGdsSocMappings
            {
                JobProfile = "Bank Manager",
                OnetSocCode = "11-3031.02",
                QualityRating = 2,
                SocCode = "1150"
            },
            new DfcGdsSocMappings
            {
                JobProfile = "Restaurant manager",
                OnetSocCode = "11-9051.00",
                QualityRating = 2,
                SocCode = "1223"
            },
            new DfcGdsSocMappings
            {
                JobProfile = "Forensic scientist",
                OnetSocCode = "19-4092.00",
                QualityRating = 2,
                SocCode = "2112"
            },
            new DfcGdsSocMappings
            {
                JobProfile = "Mechanical engineerr",
                OnetSocCode = "17-2141.00",
                QualityRating = 2,
                SocCode = "2122"
            },
            new DfcGdsSocMappings
            {
                JobProfile = "Electrical engineer",
                OnetSocCode = "17-2071.00",
                QualityRating = 2,
                SocCode = "2123"
            },

        };

        private static readonly IEnumerable<DfcGdsTranslation> TranslatedData = new List<DfcGdsTranslation>
        {
            new DfcGdsTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.1",
                Translation = "problem-solving skills"
            },
            new DfcGdsTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.1.a",
                Translation = "excellent verbal communication skills"
            },
            new DfcGdsTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.1.b",
                Translation = "thinking and reasoning skills"
            },
            new DfcGdsTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.1.c",
                Translation = "maths skills"
            },
            new DfcGdsTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.1.d",
                Translation = "a good memory"
            },
            new DfcGdsTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.1.g",
                Translation = "concentration skills"
            },
            new DfcGdsTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.2",
                Translation = "physcial skills like movement, coordination, dexterity and grace"
            },
            new DfcGdsTranslation
            {
                DateTimeStamp = DateTime.UtcNow,
                ElementId = "1.A.2.a",
                Translation = "the ability to work well with your hands"
            },
        };

        private static readonly DfcGdsDigitalSkills DigitalSkills1150 = new DfcGdsDigitalSkills
        {
            DigitalSkillsCollection = new List<DfcGdsToolsAndTechnology>
            {
                new DfcGdsToolsAndTechnology
                {
                    ClassTitle = "Personal Communication Device",
                    ElementId = "",
                    OnetSocCode = "11-10011.00",
                    SocCode = "1150",
                    T2Example = "Smartphones",
                    T2Type = "Tools"
                },
                new DfcGdsToolsAndTechnology
                {
                    ClassTitle = "Media storage devices",
                    ElementId = "",
                    OnetSocCode = "11-10011.00",
                    SocCode = "1150",
                    T2Example = "Universal serial bus USB flash drives",
                    T2Type = "Tools"
                },
                new DfcGdsToolsAndTechnology
                {
                    ClassTitle = "Computers",
                    ElementId = "",
                    OnetSocCode = "11-10011.00",
                    SocCode = "1150",
                    T2Example = "Laptop computers",
                    T2Type = "Tools"
                },
                new DfcGdsToolsAndTechnology
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

        private static readonly DfcGdsDigitalSkills DigitalSkills1223 = new DfcGdsDigitalSkills
        {
            DigitalSkillsCollection = new List<DfcGdsToolsAndTechnology>
            {
                new DfcGdsToolsAndTechnology
                {
                    ClassTitle = "Computer data input devices",
                    ElementId = "",
                    OnetSocCode = "11-2011.01",
                    SocCode = "1223",
                    T2Example = "Computer data input scanners",
                    T2Type = "Tools"
                },
                new DfcGdsToolsAndTechnology
                {
                    ClassTitle = "Computers",
                    ElementId = "",
                    OnetSocCode = "11-2011.01",
                    SocCode = "1223",
                    T2Example = "Desktop computers",
                    T2Type = "Tools"
                },
                new DfcGdsToolsAndTechnology
                {
                    ClassTitle = "Computer data input devices",
                    ElementId = "",
                    OnetSocCode = "11-2011.01",
                    SocCode = "1223",
                    T2Example = "Handheld computers",
                    T2Type = "Tools"
                },
                new DfcGdsToolsAndTechnology
                {
                    ClassTitle = "Computers",
                    ElementId = "",
                    OnetSocCode = "11-2011.01",
                    SocCode = "1223",
                    T2Example = "Laptop computers",
                    T2Type = "Tools"
                },
                new DfcGdsToolsAndTechnology
                {
                    ClassTitle = "Media storage devices",
                    ElementId = "",
                    OnetSocCode = "11-2011.01",
                    SocCode = "1223",
                    T2Example = "Universal serial bus USB flash drives",
                    T2Type = "Tools"
                },
                new DfcGdsToolsAndTechnology
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

        private static readonly DfcGdsDigitalSkills DigitalSkills2112 = new DfcGdsDigitalSkills
        {
            DigitalSkillsCollection = new List<DfcGdsToolsAndTechnology>
            {
                new DfcGdsToolsAndTechnology
                {
                    ClassTitle = "Computer data input devices",
                    ElementId = "",
                    OnetSocCode = "41-3031.03",
                    SocCode = "2112",
                    T2Example = "Computer data input scanners",
                    T2Type = "Tools"
                },
                new DfcGdsToolsAndTechnology
                {
                    ClassTitle = "Computers",
                    ElementId = "",
                    OnetSocCode = "41-3031.03",
                    SocCode = "2112",
                    T2Example = "Desktop computers",
                    T2Type = "Tools"
                },
                new DfcGdsToolsAndTechnology
                {
                    ClassTitle = "Computers",
                    ElementId = "",
                    OnetSocCode = "41-3031.03",
                    SocCode = "2112",
                    T2Example = "Laptop computers",
                    T2Type = "Tools"
                },
                new DfcGdsToolsAndTechnology
                {
                    ClassTitle = "Duplicating machines",
                    ElementId = "",
                    OnetSocCode = "41-3031.03",
                    SocCode = "2112",
                    T2Example = "Laser facsimile machines",
                    T2Type = "Tools"
                },
                new DfcGdsToolsAndTechnology
                {
                    ClassTitle = "Personal communication devices",
                    ElementId = "",
                    OnetSocCode = "41-3031.03",
                    SocCode = "2112",
                    T2Example = "Multi-line telephone systems",
                    T2Type = "Tools"
                },
                new DfcGdsToolsAndTechnology
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
    }
}
