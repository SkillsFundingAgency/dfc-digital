using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using System;
using System.Linq;

namespace DFC.Digital.Repository.ONET.Query
{
    public class ContentReferenceQueryRepository : IQueryRepository<FrameWorkContent>
    {
        private readonly OnetSkillsFramework onetDbContext;
  
        public ContentReferenceQueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        public FrameWorkContent Get(System.Linq.Expressions.Expression<Func<FrameWorkContent, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameWorkContent> GetAll()
        {
            var result = (from c in onetDbContext.content_model_reference
                          select new FrameWorkContent()
                          {
                              ONetElementId = c.element_id,
                              Title = c.element_name
                          });

            return result;
        }

        public FrameWorkContent GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameWorkContent> GetMany(System.Linq.Expressions.Expression<Func<FrameWorkContent, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
