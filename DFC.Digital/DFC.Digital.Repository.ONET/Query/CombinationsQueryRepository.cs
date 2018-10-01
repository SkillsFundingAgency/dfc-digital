using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DFC.Digital.Repository.ONET
{
    public class CombinationsQueryRepository : IQueryRepository<FrameworkSkillCombination>
    {
        private readonly OnetSkillsFramework onetDbContext;

        public CombinationsQueryRepository(OnetSkillsFramework onetDbContext)
        {
            this.onetDbContext = onetDbContext;
        }

        #region Implementation of IQueryRepository<FrameWorkSkillCombination>
        public FrameworkSkillCombination Get(Expression<Func<FrameworkSkillCombination, bool>> where)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameworkSkillCombination> GetAll()
        {
            var result = (from c in onetDbContext.DFC_GDSCombinations
                          orderby c.application_order
                          select new FrameworkSkillCombination()
                          {
                              OnetElementOneId = c.onet_element_one_id,
                              OnetElementTwoId = c.onet_element_two_id,
                              Title = c.element_name,
                              Description = c.description,
                              CombinedElementId = c.combined_element_id
                          });

            return result;

        }

        public FrameworkSkillCombination GetById(string id)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FrameworkSkillCombination> GetMany(Expression<Func<FrameworkSkillCombination, bool>> where)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
