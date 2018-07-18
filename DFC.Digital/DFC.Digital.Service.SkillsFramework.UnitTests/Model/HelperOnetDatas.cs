using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.SkillsFramework.UnitTests.Model
{
    public class HelperOnetDatas
    {
        private static readonly IEnumerable<DfcGdsSocMappings> SocMappingsData = new List<DfcGdsSocMappings>
        {
            new DfcGdsSocMappings()
            {
                JobProfile = "Bank Manager",
                OnetSocCode = "11-3031.02",
                QualityRating = 2,
                SocCode = "1150"
            },
            new DfcGdsSocMappings()
            {
                JobProfile = "Restaurant manager",
                OnetSocCode = "11-9051.00",
                QualityRating = 2,
                SocCode = "1223"
            },
            new DfcGdsSocMappings()
            {
                JobProfile = "Forensic scientist",
                OnetSocCode = "19-4092.00",
                QualityRating = 2,
                SocCode = "2112"
            },
            new DfcGdsSocMappings()
            {
                JobProfile = "Mechanical engineerr",
                OnetSocCode = "17-2141.00",
                QualityRating = 2,
                SocCode = "2122"
            },
            new DfcGdsSocMappings()
            {
                JobProfile = "Electrical engineer",
                OnetSocCode = "17-2071.00",
                QualityRating = 2,
                SocCode = "2123"
            },

        };

        public static IEnumerable<object[]> SocMappings()
        {
            yield return new object[]
            {
                SocMappingsData
            };
        }
    }
}
