using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DFC.Digital.Repository.ONET.Query
{
    public class SocMappingsQueryRepository : IQueryRepository<SocCode>
    {
        private readonly OnetSkillsFramework onetDbContext;
        private readonly IMapper autoMapper;

        public SocMappingsQueryRepository(OnetSkillsFramework onetDbContext, IMapper autoMapper)
        {
            this.onetDbContext = onetDbContext;
            this.autoMapper = autoMapper;
        }

        public SocCode Get(Expression<Func<SocCode, bool>> where)
        {
            return GetAll().Single(where);
        }
        public IQueryable<SocCode> GetAll()
        {
            var result = (from soc in onetDbContext.DFC_SocMappings
                orderby soc.SocCode
                select new SocCode()
                {
                    Id = Guid.Empty,
                    ONetOccupationalCode = soc.ONetCode,
                    SOCCode = soc.SocCode,
                    Title = null
                });
            return result;
        }

        public SocCode GetById(string id)
        {
            return onetDbContext.DFC_SocMappings.ProjectToSingle<DFC_SocMappings,SocCode>(m => m.SocCode == id,autoMapper.ConfigurationProvider);
        }

        public IQueryable<SocCode> GetMany(Expression<Func<SocCode, bool>> where)
        {
            return GetAll().Where(where);
        }
    }
}
