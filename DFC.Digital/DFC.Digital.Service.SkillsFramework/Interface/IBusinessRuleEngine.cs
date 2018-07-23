namespace DFC.Digital.Service.SkillsFramework.Interface
{
    using Data.Model;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IBusinessRuleEngine
    {
        IEnumerable<SocCode> GetAllSocMappings();

        IEnumerable<WhatItTakesSkill> GetAllTranslations();

        DigitalSkill GetDigitalSkills(string onetSocCode);

        //IEnumerable<DfcOnetAttributesData> GetBusinessRuleAttributes(string onetSocCode);
    }
}