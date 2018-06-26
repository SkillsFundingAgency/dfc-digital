using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
     public class SkillsFramework
    {
        public string SocCode { get; set; }

        public string JobProfileName { get; set; }

        public ICollection<AttributesData> SkillsAttributes { get; set; }
    }
}
