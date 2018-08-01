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
    public class ContentReferenceQueryRepository : IQueryRepository<FrameworkSkill>
    {
        private readonly OnetSkillsFramework onetDbContext;
  
        public ContentReferenceQueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        public FrameworkSkill Get(System.Linq.Expressions.Expression<Func<FrameworkSkill, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameworkSkill> GetAll()
        {
            var result = (from c in onetDbContext.content_model_reference
                          select new FrameworkSkill()
                          {
                              ONetElementId = c.element_id,
                              Title = c.element_name
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
