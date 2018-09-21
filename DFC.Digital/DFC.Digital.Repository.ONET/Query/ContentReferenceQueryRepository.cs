using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using System;
using System.Linq;

namespace DFC.Digital.Repository.ONET
{
    public class ContentReferenceQueryRepository : IQueryRepository<FrameworkContent>
    {
        private readonly OnetSkillsFramework onetDbContext;
  
        public ContentReferenceQueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        public FrameworkContent Get(System.Linq.Expressions.Expression<Func<FrameworkContent, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameworkContent> GetAll()
        {
            var result = (from c in onetDbContext.content_model_reference
                          select new FrameworkContent()
                          {
                              ONetElementId = c.element_id,
                              Title = c.element_name
                          });

            return result;
        }

        public FrameworkContent GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameworkContent> GetMany(System.Linq.Expressions.Expression<Func<FrameworkContent, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
