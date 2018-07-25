using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DFC.Digital.Repository.ONET.Query
{
    public class DigitalSkillsQueryRepository : IQueryRepository<DigitalSkill>
    {
        private readonly OnetSkillsFramework onetDbContext;
        private readonly IMapper autoMapper;
        private readonly ISkillFrameworkBusinessRuleEngine businessRuleEngine;

        public DigitalSkillsQueryRepository(OnetSkillsFramework onetDbContext, IMapper autoMapper, ISkillFrameworkBusinessRuleEngine businessRuleEngine)
        {
            this.onetDbContext = onetDbContext;
            this.autoMapper = autoMapper;
            this.businessRuleEngine = businessRuleEngine;
        }

        public DigitalSkill Get(Expression<Func<DigitalSkill, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DigitalSkill> GetAll()
        {
            throw new NotImplementedException();
        }

        public DigitalSkill GetById(string onetOccupationalCode)
        {
            var count = onetDbContext.tools_and_technology.Count(toolsandtech => toolsandtech.onetsoc_code == onetOccupationalCode);
            return new DigitalSkill
            {
                Level = businessRuleEngine.GetDigitalSkillsLevel(count)
            };
        }

        public IQueryable<DigitalSkill> GetMany(Expression<Func<DigitalSkill, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}