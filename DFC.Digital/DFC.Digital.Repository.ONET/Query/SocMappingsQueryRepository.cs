using AutoMapper;
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
            return onetDbContext.DFC_SocMappings.ProjectToQueryable<SocCode>(autoMapper.ConfigurationProvider);
        }

        public SocCode GetById(string id)
        {
            return autoMapper.Map<SocCode>(onetDbContext.DFC_SocMappings.Single(m => m.SocCode == id));
        }

        public IQueryable<SocCode> GetMany(Expression<Func<SocCode, bool>> where)
        {
            return GetAll().Where(where);
        }
    }
}
