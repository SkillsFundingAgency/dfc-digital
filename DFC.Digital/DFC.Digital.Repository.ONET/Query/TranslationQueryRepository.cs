﻿using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.DataModel;

namespace DFC.Digital.Repository.ONET.Query
{
    
    public class TranslationQueryRepository: IQueryRepository<FrameworkSkill>
    {
        // CodeReview: TK;  Please remove unused private fields
        private readonly OnetSkillsFramework onetDbContext;
        private readonly IMapper autoMapper;

        public TranslationQueryRepository(OnetSkillsFramework onetDbContext,IMapper autoMapper)
        {
            this.onetDbContext = onetDbContext;
            this.autoMapper = autoMapper;
        }

        #region Implementation of IQueryRepository<FrameworkSkill>

        // CodeReview: Tk: is this being used by the translations processes, if not why we do have it
        public FrameworkSkill GetById(string id)
        {

            var result = (from trans in onetDbContext.DFC_GDSTranlations
                join el in onetDbContext.content_model_reference on trans.onet_element_id equals el.element_id
                where el.element_id == id
                          orderby trans.onet_element_id
                select new FrameworkSkill
                {
                    ONetElementId = trans.onet_element_id,
                    Title = el.element_name,
                    Description = trans.translation

                }).Single();

            return result;
        }

        public FrameworkSkill Get(Expression<Func<FrameworkSkill, bool>> where)
        {
            return GetAll().Single(where);
        }

        public IQueryable<FrameworkSkill> GetAll()
        {
            var result = (from trans in onetDbContext.DFC_GDSTranlations
                join el in onetDbContext.content_model_reference on trans.onet_element_id equals el.element_id
                where el.element_id == trans.onet_element_id
                orderby trans.onet_element_id
                select new FrameworkSkill
                {
                    ONetElementId = trans.onet_element_id,
                    Title = el.element_name,
                    Description = trans.translation

                }).Concat(from comb in onetDbContext.DFC_GDSCombinations
                          orderby comb.combined_element_id
                          select new FrameworkSkill
                          {
                              ONetElementId = comb.combined_element_id,
                              Title = comb.element_name,
                              Description = comb.description
                          });

            return result;
        }

        public IQueryable<FrameworkSkill> GetMany(Expression<Func<FrameworkSkill, bool>> where)
        {
            return GetAll().Where(where);
        }

        #endregion
    }
}
