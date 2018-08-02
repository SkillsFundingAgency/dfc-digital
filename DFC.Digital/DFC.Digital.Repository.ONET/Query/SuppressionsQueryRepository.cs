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
    public class SuppressionsQueryRepository : IQueryRepository<FrameworkSkillSuppression>
    {
        private readonly OnetSkillsFramework onetDbContext;
  
        public SuppressionsQueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        #region Implementation of IQueryRepository<FrameworkSkillSuppression>
        public FrameworkSkillSuppression Get(System.Linq.Expressions.Expression<Func<FrameworkSkillSuppression, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameworkSkillSuppression> GetAll()
        {
            var result = (from s in onetDbContext.DFC_GlobalAttributeSuppression
                          select new FrameworkSkillSuppression()
                          {
                              ONetElementId = s.onet_element_id            
                          });

            return result;
        }

        public FrameworkSkillSuppression GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameworkSkillSuppression> GetMany(System.Linq.Expressions.Expression<Func<FrameworkSkillSuppression, bool>> where)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
