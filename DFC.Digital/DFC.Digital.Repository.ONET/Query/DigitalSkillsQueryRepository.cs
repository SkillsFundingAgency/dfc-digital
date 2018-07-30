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
    
        public DigitalSkillsQueryRepository(OnetSkillsFramework onetDbContext, IMapper autoMapper)
        {
            this.onetDbContext = onetDbContext;
            this.autoMapper = autoMapper;
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
            var applicationCount = onetDbContext.tools_and_technology.Count(toolsandtech => toolsandtech.onetsoc_code == onetOccupationalCode);
            return new DigitalSkill
            {
                ApplicationCount = applicationCount
            };
        }

        public IQueryable<DigitalSkill> GetMany(Expression<Func<DigitalSkill, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}