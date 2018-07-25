using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface ISkillsFrameworkService
    {
        IEnumerable<SocCode> GetAllSocMappings();

        IEnumerable<WhatItTakesSkill> GetAllTranslations();

        int GetDigitalSkillRank(string onetOccupationalCode);

        IEnumerable<RelatedSkillMapping> GetRelatedSkillMapping(string onetOccupationalCode);
    }
}