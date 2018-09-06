using AutoMapper;
using DFC.Digital.Core;
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
      
        public DigitalSkillsQueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
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
            var applicationCount = (from o in onetDbContext.tools_and_technology
                                    join od in onetDbContext.unspsc_reference on o.commodity_code equals od.commodity_code
                                    where o.onetsoc_code == onetOccupationalCode && o.t2_type == Constants.Technology
                                    orderby o.t2_type, od.class_title
                                    select o).Count();
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