using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;

namespace DFC.Digital.Repository.ONET.Query
{
    
    public class TranslationQueryRepository: IQueryRepository<WhatItTakesSkill>
    {
        private readonly OnetSkillsFramework onetDbContext;
        private readonly IMapper autoMapper;

        public TranslationQueryRepository(OnetSkillsFramework onetDbContext,IMapper autoMapper)
        {
            this.onetDbContext = onetDbContext;
            this.autoMapper = autoMapper;
        }

        #region Implementation of IQueryRepository<WhatItTakesSkill>

        public WhatItTakesSkill GetById(string id)
        {
            var val=onetDbContext.DFC_GDSTranlations.ProjectToSingle<DFC_GDSTranlations,WhatItTakesSkill>(x => x.onet_element_id == id,
                autoMapper.ConfigurationProvider);
            return val;// autoMapper.Map<WhatItTakesSkill>(onetDbContext.DFC_GDSTranlations.Single(x=>x.onet_element_id==id));
        }

        public WhatItTakesSkill Get(Expression<Func<WhatItTakesSkill, bool>> @where)
        {
            return GetAll().Single(where);
        }

        public IQueryable<WhatItTakesSkill> GetAll()
        {
           
            return onetDbContext.DFC_GDSTranlations.ProjectToQueryable<WhatItTakesSkill>(autoMapper.ConfigurationProvider);
        }

        public IQueryable<WhatItTakesSkill> GetMany(Expression<Func<WhatItTakesSkill, bool>> @where)
        {
            return GetAll().Where(where);
        }

        #endregion
    }
}
