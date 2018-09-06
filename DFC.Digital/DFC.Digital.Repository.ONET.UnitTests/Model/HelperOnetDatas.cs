using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;

namespace DFC.Digital.Repository.ONET.UnitTests
{
    public class HelperOnetDatas : HelperEfOnetDatas
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
        private static readonly IEnumerable<FrameworkSkill> TranslatedData = new List<FrameworkSkill>
        {

            new FrameworkSkill
            {
                ONetElementId = "1.A.1.a",
                Title="Mathematics",
                Description = "excellent verbal communication skills and mathematics knowledge"
            },
            new FrameworkSkill
            {
                ONetElementId = "1.A.1a",
                Title="abilties",
                Description = "problem-solving skills and working characterstics"
            },
            new FrameworkSkill
            {
                ONetElementId = "1.A.1.b",
                Title = "workstyle",
                Description = "thinking and reasoning skills and adventourus workstlye"
            },
            new FrameworkSkill
            {
                ONetElementId = "1.A.1.c",
                Title = "knowledge",
                Description = "maths skills  knowledge and algorithms knowledge"
            },
            new FrameworkSkill
            {
                ONetElementId = "1.A.1.d",
                Title = "Mathematics",
                Description = "a good memory and mathematics solving skills"
            },
            new FrameworkSkill
            {
                ONetElementId = "1.A.1.g",
                Title = "skills 3",
                Description = "concentration skills"
            },
            new FrameworkSkill
            {
                ONetElementId = "1.A.1.h",
                Title="skills 4",
                Description = "the ability to work well with your hands"
            },
            new FrameworkSkill
            {
                ONetElementId = "1.A.1.e",
                Title = "skills 2",
                Description = "great leadership and behavioural characterstics"
            },
            new FrameworkSkill()
            {
                ONetElementId = "C1",
                Title = "Hello Unit1",
                Description = "Hello Unit Test"
            },
            new FrameworkSkill()
            {
                ONetElementId = "C2",
                Title = "Hello Unit2",
                Description = "Hello Unit Test 2"
            },
        };
        public static IEnumerable<object[]> GetAllSocMappingData()
        {
            yield return new object[]
            {
                DfcSocMappings,
                SocMappingsData
            };
        }
        public static IEnumerable<object[]> GetSocMappingStatusData()
        {
            yield return new object[]
            {
                MixedCombination(),
                new List<int> {AwaitingUpdateDfcSocMappings.Count(), SelectedForUpdateDfcSocMappings.Count(), UpdateCompletedDfcSocMappings.Count() } 
            };
        }

        public static IEnumerable<object[]> GetSocsAwaitingUpdateData()
        {
            yield return new object[]
            {
                MixedCombination(),
                AwaitingUpdateDfcSocMappings
                    .Select(soc => new SocCode {SOCCode = soc.SocCode, ONetOccupationalCode = soc.ONetCode})
                    .AsQueryable()
            };
        }

        public static IEnumerable<object[]> GetSocsSelectedForUpdateData()
        {

            yield return new object[]
            {
                MixedCombination(),
                SelectedForUpdateDfcSocMappings
                    .Select(soc => new SocCode {SOCCode = soc.SocCode, ONetOccupationalCode = soc.ONetCode})
                    .AsQueryable()
            };

        }

        public static ReadOnlyCollection<DFC_SocMappings> MixedCombination()
        {
            return AwaitingUpdateDfcSocMappings.Concat(SelectedForUpdateDfcSocMappings).Concat(UpdateCompletedDfcSocMappings).ToList().AsReadOnly();
        }

        public static IEnumerable<object[]> GetByIdSocMappingData()
        {
            yield return new object[]
            {
                DfcSocMappings,
                SocMappingsData.ToList()[0],
                "1150"
            };
            yield return new object[]
            {
                DfcSocMappings,
                SocMappingsData.ToList()[1],
                "1223"
            };
            yield return new object[]
            {
                DfcSocMappings,
                SocMappingsData.ToList()[2],
                "2112"
            };
            yield return new object[]
            {
                DfcSocMappings,
                SocMappingsData.ToList()[3],
                "2122"
            };

        }
        public static IEnumerable<object[]> GetManySocMappingData()
        {
            yield return new object[]
            {
                DfcSocMappings,
                new List<SocCode>()
                {
                SocMappingsData.ToList()[0],
                SocMappingsData.ToList()[1],
                },
                "1150",
                "1223"
            };
            yield return new object[]
            {
                DfcSocMappings,
                new List<SocCode>()
                {
                    SocMappingsData.ToList()[1],
                    SocMappingsData.ToList()[2],
                },
                "1223",
                "2112"
            };
            yield return new object[]
            {
                DfcSocMappings,
                new List<SocCode>()
                {
                    SocMappingsData.ToList()[2],
                    SocMappingsData.ToList()[3],
                },
                "2112",
                "2122"
            };
            yield return new object[]
            {
                DfcSocMappings,
                new List<SocCode>()
                {
                    SocMappingsData.ToList()[1],
                    SocMappingsData.ToList()[3],
                },
                "1223",
                "2122"
            };

        }
        public static IEnumerable<object[]> OnetGetAllTranslationsData()
        {
            yield return new object[]
            {
                DfcTranslations,
                OnetContentModelReference,
                DfcCombination,
                TranslatedData,
            };
            yield return new object[]
            {
                DfcTranslations,
                OnetContentModelReference,
                DfcCombination,
                TranslatedData,
            };

        }
        public static IEnumerable<object[]> OnetFrameworkSkillTranslationData()
        {
            yield return new object[]
            {
                DfcTranslations,
                OnetContentModelReference,
                DfcCombination,
                TranslatedData.ToList()[0],
                "1.A.1.a"
            };
            yield return new object[]
            {
                DfcTranslations,
                OnetContentModelReference,
                DfcCombination,
                TranslatedData.ToList()[2],
                "1.A.1.b"
            };
            yield return new object[]
            {
                DfcTranslations,
                OnetContentModelReference,
                DfcCombination,
                TranslatedData.ToList()[3],
                "1.A.1.c"
            };

        }
        public static IEnumerable<object[]> OnetWhatitTakesManyData()
        {
            yield return new object[]
            {
                DfcTranslations,
                OnetContentModelReference,
                DfcCombination,
                new List<FrameworkSkill>()
                {
                    TranslatedData.ToList()[0],
                    TranslatedData.ToList()[2],
                },
                "1.A.1.a",
                "1.A.1.b"
            };

        }
    }
}