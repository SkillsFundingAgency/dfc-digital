using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Repository.ONET.Query
{

    public class AbilitiesOueryRepository : ISkillsRepository
    {
        private readonly OnetSkillsFramework onetDbContext;

        public AbilitiesOueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        public IQueryable<OnetAttribute> GetSkillsForONetOccupationCode(string oNetOccupationCode)
        {
            var attributes = from ability in onetDbContext.abilities
                                    where ability.recommend_suppress != "Y"
                                        && ability.not_relevant != "Y"
                                        && ability.onetsoc_code == oNetOccupationCode
                                    select new OnetAttribute
                                    {
                                        OnetOccupationalCode = ability.onetsoc_code,
                                        Id = ability.element_id,
                                        Type = AttributeType.Ability,
                                        Score = ability.data_value
                                    };
            return attributes;
        }
    }
}
