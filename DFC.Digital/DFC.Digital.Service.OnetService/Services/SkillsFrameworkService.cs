using System;
using System.Collections.Generic;
using DFC.Digital.Data.Model;
using DFC.Digital.Service.SkillsFramework.Services.Contracts;

namespace DFC.Digital.Service.SkillsFramework.Services
{
    public class SkillsFrameworkService : ISkillsFrameworkService
    {
        public IDictionary<string, string> GetSocOccupationalCodeMappings()
        {
            var list = new Dictionary<string, string> { { "9272", $"ocCode-{DateTime.Now}" }, { "5433", $"ocCode-{DateTime.Now}" } , { "6222", $"ocCode-{DateTime.Now}" }, { "5432", $"ocCode-{DateTime.Now}" }, { "1234", $"ocCode-{DateTime.Now}" } };
            return list;
        }

        int ISkillsFrameworkService.GetDigitalSkillLevel(string onetOccupationalCode)
        {
            var random = new Random();
            return random.Next(1, 4);
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

        public IEnumerable<FrameworkSkill> GetOnetSkills()
        {
            return new List<FrameworkSkill>
            {
                new FrameworkSkill
                {
                    Description = $"{nameof(FrameworkSkill.Description)} updated",
                    ONetElementId = nameof(FrameworkSkill.ONetElementId),
                    Title = $"{nameof(FrameworkSkill.Title)}-1"
                },
                new FrameworkSkill
                {
                    Description = nameof(FrameworkSkill.Description),
                    ONetElementId = nameof(FrameworkSkill.ONetElementId),
                    Title = $"{nameof(FrameworkSkill.Title)}-2"
                },
                new FrameworkSkill
                {
                    Description = nameof(FrameworkSkill.Description),
                    ONetElementId = nameof(FrameworkSkill.ONetElementId),
                    Title = $"{nameof(FrameworkSkill.Title)}-3"
                }
            };
        }

        public string GetSocOccupationalCode(string socCode)
        {
            return DateTime.Now.Ticks.ToString();
        }
    }
}
