using Autofac;
using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace DFC.Digital.Repository.ONET.Query
{
    public class TranslationQueryRepository : IQueryRepository<FrameworkSkill>
    {
        private readonly OnetSkillsFramework onetDbContext;
        private readonly IMapper autoMapper;
        private readonly IApplicationLogger logger;
        private readonly ILifetimeScope lifetime;

        public TranslationQueryRepository(OnetSkillsFramework onetDbContext, IMapper autoMapper, IApplicationLogger logger, ILifetimeScope lifetime)
        {
            this.onetDbContext = onetDbContext;
            this.autoMapper = autoMapper;
            this.logger = logger;
            this.lifetime = lifetime;
        }

        #region Implementation of IQueryRepository<FrameworkSkill>

        public FrameworkSkill GetById(string id)
        {
            return autoMapper.Map<FrameworkSkill>(onetDbContext.DFC_GDSTranlations.Single(x => x.onet_element_id == id));
        }

        public FrameworkSkill Get(Expression<Func<FrameworkSkill, bool>> where)
        {
            return GetAll().Single(where);
        }

        public IQueryable<FrameworkSkill> GetAll()
        {
            logger.Info($"The lifetime is {lifetime.Tag.ToString()}");
            return onetDbContext.DFC_GDSTranlations.ProjectToQueryable<FrameworkSkill>(autoMapper.ConfigurationProvider);
        }

        public IQueryable<FrameworkSkill> GetMany(Expression<Func<FrameworkSkill, bool>> where)
        {
            return GetAll().Where(where);
        }

        #endregion Implementation of IQueryRepository<FrameworkSkill>
    }
}