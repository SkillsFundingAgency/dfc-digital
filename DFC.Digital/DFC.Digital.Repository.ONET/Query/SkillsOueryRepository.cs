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

    public class SkillsOueryRepository : ISkillsRepository
    {
        private readonly OnetSkillsFramework onetDbContext;

        public SkillsOueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        public IQueryable<OnetAttribute> GetSkillsForONetOccupationCode(string oNetOccupationCode)
        {
            var attributes = from skill in onetDbContext.skills
                             join reference in onetDbContext.content_model_reference on skill.element_id equals reference.element_id
                             where skill.recommend_suppress != "Y"
                                        && skill.not_relevant != "Y"
                                        && skill.onetsoc_code == oNetOccupationCode
                                    select new OnetAttribute
                                    {
                                        OnetOccupationalCode = skill.onetsoc_code,
                                        Id = skill.element_id,
                                        Type = AttributeType.Skill,
                                        Score = skill.data_value,
                                        Name = reference.element_name
                                    };
            return attributes;
        }
    }
}
