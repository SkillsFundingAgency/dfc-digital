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
    public class SuppressionsQueryRepository : IQueryRepository<FrameworkSkill>
    {
        private readonly OnetSkillsFramework onetDbContext;
  
        public SuppressionsQueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        public FrameworkSkill Get(System.Linq.Expressions.Expression<Func<FrameworkSkill, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameworkSkill> GetAll()
        {
            var result = (from s in onetDbContext.DFC_GlobalAttributeSuppression
                          select new FrameworkSkill
                          {
                              ONetElementId = s.onet_element_id            
                          });

            return result;
        }

        public FrameworkSkill GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameworkSkill> GetMany(System.Linq.Expressions.Expression<Func<FrameworkSkill, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
