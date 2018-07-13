using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.OnetService.Services.Contracts;

namespace DFC.Digital.Service.OnetService.Services
{
    public class OnetService : IOnetService
    {
        public string GetDigitalSkillLevel(string onetOccupationalCode)
        {
            var random = new Random();
            return Convert.ToString(random.Next(1, 4));
        }

        public IEnumerable<OccupationOnetSkill> GetOccupationalCodeSkills(string onetOccupationalCode)
        {
            yield return new OccupationOnetSkill
            {
                Title = $"{nameof(OnetSkill.Title)} - 1",
                OnetRank = 7
            };
            yield return new OccupationOnetSkill
            {
                Title = $"{nameof(OnetSkill.Title)} - 2",
                OnetRank = 4
            };
            yield return new OccupationOnetSkill
            {
                Title = $"{nameof(OnetSkill.Title)} - 3",
                OnetRank = 2
            };
        }

        public OnetSkillsImportRequest GetOnetSkills()
        {
            return new OnetSkillsImportRequest
            {
                OnetSkillsList = new List<OnetSkill>
                {
                    new OnetSkill { Description = $"{nameof(OnetSkill.Description)} updated", OnetElementId = nameof(OnetSkill.OnetElementId), Title = $"{nameof(OnetSkill.Title)} - 1" },
                    new OnetSkill { Description = nameof(OnetSkill.Description), OnetElementId = nameof(OnetSkill.OnetElementId), Title = $"{nameof(OnetSkill.Title)} - 2" },
                    new OnetSkill { Description = nameof(OnetSkill.Description), OnetElementId = nameof(OnetSkill.OnetElementId), Title = $"{nameof(OnetSkill.Title)} - 3" }
                }
            };
        }

        public string GetSocOccupationalCode(string socCode)
        {
            return DateTime.Now.Ticks.ToString();
        }
    }
}
