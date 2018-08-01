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
    public class CombinationsQueryRepository : IQueryRepository<FrameWorkSkillCombination>
    {
        private readonly OnetSkillsFramework onetDbContext;

        public CombinationsQueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        public FrameWorkSkillCombination Get(Expression<Func<FrameWorkSkillCombination, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameWorkSkillCombination> GetAll()
        {
            var result = (from c in onetDbContext.DFC_GDSCombinations
                          orderby c.application_order
                          select new FrameWorkSkillCombination()
                          {
                              OnetElementOneId = c.onet_element_one_id,
                              OnetElementTwoId = c.onet_element_two_id,
                              Title = c.element_name,
                              Description = c.description,
                              CombinedElementId = c.combined_element_id
                          });

            return result;

        }

        public FrameWorkSkillCombination GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameWorkSkillCombination> GetMany(Expression<Func<FrameWorkSkillCombination, bool>> where)
        {
            throw new NotImplementedException();
        }
    }
}
