using System.Collections.Generic;
using DFC.Digital.Data.Model;

namespace DFC.Digital.Service.OnetService.Services.Contracts
{
    public class OnetService : IOnetService
    {
        public OnetSkillsImportRequest GetOnetSkills()
        {
            return new OnetSkillsImportRequest
            {
                OnetSkillsList = new List<OnetSkill>
                {
                    new OnetSkill{ Description = $"{nameof(OnetSkill.Description)} updated", OnetElementId = nameof(OnetSkill.OnetElementId), Title = $"{nameof(OnetSkill.Title)} - 1" },
                    new OnetSkill{ Description = nameof(OnetSkill.Description), OnetElementId = nameof(OnetSkill.OnetElementId), Title = $"{nameof(OnetSkill.Title)} - 2" },
                    new OnetSkill{ Description = nameof(OnetSkill.Description), OnetElementId = nameof(OnetSkill.OnetElementId), Title = $"{nameof(OnetSkill.Title)} - 3" }
                }
            };
        }
    }
}
