using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.OnetService.Services.Contracts;

namespace DFC.Digital.Service.OnetService.Services
{
    public class SkillsFrameworkService : Contracts.SkillsFrameworkService
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
                Title = $"{nameof(FrameworkSkill.Title)} - 1",
                OnetRank = 7
            };
            yield return new OccupationOnetSkill
            {
                Title = $"{nameof(FrameworkSkill.Title)} - 2",
                OnetRank = 4
            };
            yield return new OccupationOnetSkill
            {
                Title = $"{nameof(FrameworkSkill.Title)} - 3",
                OnetRank = 2
            };
        }

        public FrameworkSkillsImportRequest GetOnetSkills()
        {
            return new FrameworkSkillsImportRequest
            {
                OnetSkillsList = new List<FrameworkSkill>
                {
                    new FrameworkSkill { Description = $"{nameof(FrameworkSkill.Description)} updated", OnetElementId = nameof(FrameworkSkill.OnetElementId), Title = $"{nameof(FrameworkSkill.Title)} - 1" },
                    new FrameworkSkill { Description = nameof(FrameworkSkill.Description), OnetElementId = nameof(FrameworkSkill.OnetElementId), Title = $"{nameof(FrameworkSkill.Title)} - 2" },
                    new FrameworkSkill { Description = nameof(FrameworkSkill.Description), OnetElementId = nameof(FrameworkSkill.OnetElementId), Title = $"{nameof(FrameworkSkill.Title)} - 3" }
                }
            };
        }

        public string GetSocOccupationalCode(string socCode)
        {
            return DateTime.Now.Ticks.ToString();
        }
    }
}
