using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Repository.ONET.Query
{
    public class SuppressionsQueryRepository : IQueryRepository<FrameWorkSkill>
    {
        private readonly OnetSkillsFramework onetDbContext;
  
        public SuppressionsQueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        public FrameWorkSkill Get(System.Linq.Expressions.Expression<Func<FrameWorkSkill, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameWorkSkill> GetAll()
        {
            var result = (from s in onetDbContext.DFC_GlobalAttributeSuppression
                          select new FrameWorkSkill()
                          {
                              ONetElementId = s.onet_element_id            
                          });

            return result;
        }

        public FrameWorkSkill GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameWorkSkill> GetMany(System.Linq.Expressions.Expression<Func<FrameWorkSkill, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
