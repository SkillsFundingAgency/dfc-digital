using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DFC.Digital.Data.Model;
using DFC.Digital.Repository.ONET.Helper;
using DFC.Digital.Repository.ONET.Impl;
using DFC.Digital.Core;

namespace DFC.Digital.Repository.ONET
{
    using DataModel;

    public class OnetRepository : IOnetRepository
    {
        readonly IObjectContextFactory<OnetRepositoryDbContext> context;
        readonly IApplicationLogger logger;
        readonly IMapper mapper;
        public OnetRepository(IObjectContextFactory<OnetRepositoryDbContext> context, IMapper mapper, IApplicationLogger logger)
        {
            this.context = context;
            this.mapper = mapper;
            this.logger = logger;
        }

        #region Implementation of IDisposable

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
        #endregion
        #region SkillsFramework Repository Implemetation
        public async Task<IEnumerable<T>> GetAllSocMappingsAsync<T>() where T : OnetEntity
        {
            using (var context = this.context.GetContext())
            {
                context.Database.Log = s => logger.Info(s);
                var ret = await context.Set<DFC_SocMappings>()
                    .AsQueryable()
                    .ProjectToListAsync<T>(mapper.ConfigurationProvider)
                    .ConfigureAwait(false);
                return mapper.Map<IEnumerable<T>>(ret);
            }
        }

        public async Task<IEnumerable<T>> GetAllTranslationsAsync<T>() where T : OnetEntity
        {
            IEnumerable<T> ret=null;
            using (var context = this.context.GetContext())
            {
                 ret = await context.DFC_GDSTranlations
                    .AsQueryable()
                    .ProjectToListAsync<T>(mapper.ConfigurationProvider)
                    .ConfigureAwait(false);
               
            }
            return mapper.Map<IEnumerable<T>>(ret);
        }

        public async Task<IEnumerable<T>> GetAttributesValuesAsync<T>(string socCode)
        {
            IQueryable<DfcOnetAttributesData> attributes = null;
            IList<T> dfcGdsAttributesDatas = null;
            using(var context = this.context.GetContext())
            {
                // just transforming Stored procedure to Linq . Will optimize with specification and predicates
                await Task.Factory.StartNew(() =>
                {
                    attributes = (KnowledgeSource(socCode, context))
                        .Union(SkillSource(socCode, context))
                        .Union(AbilitiesSource(socCode, context))
                        .Union(WorkStyleSource(socCode, context))
                        .OrderByDescending(x => x.Value);
                }).ConfigureAwait(false);
                var updatedAttributes = CheckAndUpdateForMathematics(attributes);
                 dfcGdsAttributesDatas = (IList<T>) DfcGdsUpdatedAttributesDatas(updatedAttributes);
            }
            return (IEnumerable<T>)dfcGdsAttributesDatas;
        }
        public async Task<T> GetDigitalSkillsAsync<T>(string socCode) where T : OnetEntity
        {
            IQueryable<DfcOnetToolsAndTechnology> dfcToolsandTech = null;
            DfcOnetDigitalSkills digitalSkills = new DfcOnetDigitalSkills();
            using (var context = this.context.GetContext())
            {
                await Task.Factory.StartNew(() =>
                {
                    dfcToolsandTech = from o in context?.tools_and_technology
                        join od in context?.unspsc_reference on o.commodity_code equals od.commodity_code
                        where o.onetsoc_code == socCode
                        orderby o.t2_type, od.class_title
                        select new DfcOnetToolsAndTechnology
                        {
                            ClassTitle = od.class_title,
                            T2Example = o.t2_example,
                            T2Type = o.t2_type,
                            SocCode = socCode,
                            OnetSocCode = o.onetsoc_code
                        };
                }).ConfigureAwait(false);
                 digitalSkills = new DfcOnetDigitalSkills
                {
                    DigitalSkillsCollection = dfcToolsandTech,
                    DigitalSkillsCount = dfcToolsandTech.Count()
                };
            }
            return (T)Convert.ChangeType(digitalSkills, typeof(T));
        }

        public async Task<T> GetDigitalSkillsRankAsync<T>(string socCode) where T : struct
        {
            var count = 0;
            using (var context = this.context.GetContext())
            {
                await Task.Factory.StartNew(() =>
                {
                    count = (from o in context.tools_and_technology
                        join od in context.unspsc_reference on o.commodity_code equals od.commodity_code
                        where o.onetsoc_code == socCode
                        orderby o.t2_type, od.class_title
                        select o).Count();
                }).ConfigureAwait(false);
            }
            return (T)(object)count;
        }
        #endregion

        #region Private helper function for the interface
        static IQueryable<DfcOnetAttributesData> AbilitiesSource(string socCode, OnetRepositoryDbContext context)
        {
            return from ablty in context.abilities
                   join cmr in context.content_model_reference on ablty.element_id.Substring(0, 7) equals
                       cmr.element_id
                   where ablty.recommend_suppress != "Y" && ablty.not_relevant != "Y" &&
                         ablty.onetsoc_code == socCode
                   group ablty by new
                   {
                       cmr.element_name,
                       cmr.description,
                       ablty.element_id,
                       ablty.onetsoc_code
                   }
                into abilityGroup
                   select new DfcOnetAttributesData
                   {
                       OnetSocCode = abilityGroup.Key.onetsoc_code,
                       ElementId = abilityGroup.Key.element_id,
                       ElementDescription = abilityGroup.Key.description,
                       ElementName = abilityGroup.Key.element_name,
                       Attribute = @Attributes.Abilities,
                       Value = abilityGroup.Sum(x => x.data_value) / 2
                   };
        }

        static IQueryable<DfcOnetAttributesData> CheckAndUpdateForMathematics(IQueryable<DfcOnetAttributesData> attributes)
        {
            var mathsKnowledge = attributes.ToList().FindAll(x => x.ElementName == "Mathematics").ToList();
            IQueryable<DfcOnetAttributesData> newAtt = null;
            if(mathsKnowledge.Count > 1)
            {
                var mathsKnowledgeValue = attributes.ToList().FirstOrDefault(x =>
                    x.ElementName == "Mathematics" && x.Value == mathsKnowledge.ToList().Min(x1 => x1.Value));

                newAtt = attributes;
                //Remove the Min Total Value for Mathematics from attributes Skills or Knowledge
                var suc = attributes.ToList().RemoveAll(x => x.ElementId.All(c =>
                    x.ElementId.Contains(attributes?.ToList()?.FirstOrDefault(x2 =>
                            x2.ElementName == "Mathematics" && x2.Value == mathsKnowledge?.ToList()?.Min(x3 => x3.Value))
                        ?.ElementId)));

                var update = mathsKnowledge.RemoveAll(x => x.ElementId == mathsKnowledgeValue?.ElementId);
                // Add 110% to the Total Value in the Skills or Knowledge for Mathematics
                var val = mathsKnowledge.Select(c =>
                {
                    c.Value = c.Value * (decimal)1.1;
                    return c;
                }).ToList();
                //Add the Mathematics back to the Complete Attribute List
                foreach(var k in mathsKnowledge)
                {
                    newAtt.ToList().RemoveAll(x => x.ElementId == k.ElementId);
                    newAtt.ToList().AddRange(val);
                }
            }
            return newAtt;
        }

        static IList<DfcOnetAttributesData> DfcGdsUpdatedAttributesDatas(IQueryable<DfcOnetAttributesData> newAtt)
        {
            var elementAttribute = newAtt?.ToList()?.OrderByDescending(x => x.Value)
                .Where(x => x.Attribute == Attributes.WorkStyles)
                .OrderByDescending(z => z.Value).ThenByDescending(y => y.Attribute).Select(x => new DfcOnetAttributesData
                {
                    Value = x.Value,
                    Attribute = x.Attribute,
                    ElementId = x.ElementId,
                    ElementDescription = x.ElementDescription,
                    OnetSocCode = x.OnetSocCode,
                    SocCode = x.SocCode
                }).Take(5)
                .Union(newAtt?.ToList()?.OrderByDescending(x => x.Value).Where(x => x.Attribute == Attributes.Skills)
                    .OrderByDescending(z => z.Value).ThenByDescending(y => y.Attribute).Select(x => new DfcOnetAttributesData
                    {
                        Value = x.Value,
                        Attribute = x.Attribute,
                        ElementId = x.ElementId,
                        ElementDescription = x.ElementDescription,
                        OnetSocCode = x.OnetSocCode,
                        SocCode = x.SocCode
                    }).Take(5))
                .Union(newAtt?.ToList()?.OrderByDescending(x => x.Value).Where(x => x.Attribute == Attributes.Abilities)
                    .OrderByDescending(z => z.Value).ThenByDescending(y => y.Attribute).Select(x => new DfcOnetAttributesData
                    {
                        Value = x.Value,
                        Attribute = x.Attribute,
                        ElementId = x.ElementId,
                        ElementDescription = x.ElementDescription,
                        OnetSocCode = x.OnetSocCode,
                        SocCode = x.SocCode
                    }).Take(5))
                .Union(newAtt?.ToList()?.OrderByDescending(x => x.Value).Where(x => x.Attribute == Attributes.Knowledge)
                    .OrderByDescending(z => z.Value).ThenByDescending(y => y.Attribute).Select(x => new DfcOnetAttributesData
                    {
                        Value = x.Value,
                        Attribute = x.Attribute,
                        ElementId = x.ElementId,
                        ElementDescription = x.ElementDescription,
                        OnetSocCode = x.OnetSocCode,
                        SocCode = x.SocCode
                    }).Take(5));
            var dfcGdsAttributesDatas = elementAttribute as IList<DfcOnetAttributesData> ?? elementAttribute?.ToList();
            return dfcGdsAttributesDatas;
        }

        static IQueryable<DfcOnetAttributesData> KnowledgeSource(string socCode, OnetRepositoryDbContext context)
        {
            return from knwldg in context.knowledges
                   join cmr in context.content_model_reference on knwldg.element_id.Substring(0, 7) equals
                       cmr.element_id
                   where knwldg.recommend_suppress != "Y" && knwldg.not_relevant != "Y" &&
                         knwldg.onetsoc_code == socCode
                   group knwldg by new
                   {
                       cmr.element_name,
                       cmr.description,
                       knwldg.element_id,
                       knwldg.onetsoc_code
                   }
                into knwldgGroup
                   select new DfcOnetAttributesData
                   {
                       OnetSocCode = knwldgGroup.Key.onetsoc_code,
                       ElementId = knwldgGroup.Key.element_id,
                       ElementDescription = knwldgGroup.Key.description,
                       ElementName = knwldgGroup.Key.element_name,
                       Attribute = Attributes.Knowledge.ToString(),
                       Value = knwldgGroup.Sum(v => v.data_value) / 2
                   };
        }

        static IQueryable<DfcOnetAttributesData> SkillSource(string socCode, OnetRepositoryDbContext context)
        {
            return from skl in context.skills
                   join cmr in context.content_model_reference on skl.element_id.Substring(0, 7) equals cmr
                       .element_id
                   where skl.recommend_suppress != "Y" && skl.not_relevant != "Y" &&
                         skl.onetsoc_code == socCode
                   group skl by new
                   {
                       skl.onetsoc_code,
                       cmr.element_name,
                       cmr.description,
                       skl.element_id
                   }
                into skillGroup
                   select new DfcOnetAttributesData
                   {
                       OnetSocCode = skillGroup.Key.onetsoc_code,
                       ElementId = skillGroup.Key.element_id,
                       ElementDescription = skillGroup.Key.description,
                       ElementName = skillGroup.Key.element_name,
                       Attribute = Attributes.Skills,
                       Value = skillGroup.Sum(x => x.data_value) / 2
                   };
        }

        static IQueryable<DfcOnetAttributesData> WorkStyleSource(string socCode, OnetRepositoryDbContext context)
        {
            return from wkstyl in context.work_styles
                   join cmr in context.content_model_reference on wkstyl.element_id.Substring(0, 7) equals
                       cmr.element_id
                   where wkstyl.recommend_suppress != "Y" && wkstyl.onetsoc_code == socCode
                   group wkstyl by new
                   {
                       wkstyl.onetsoc_code,
                       cmr.element_name,
                       cmr.description,
                       wkstyl.element_id
                   }
                into workStyleGroup
                   select new DfcOnetAttributesData
                   {
                       OnetSocCode = workStyleGroup.Key.onetsoc_code,
                       ElementId = workStyleGroup.Key.element_id,
                       ElementDescription = workStyleGroup.Key.description,
                       ElementName = workStyleGroup.Key.element_name,
                       Attribute = Attributes.WorkStyles,
                       Value = workStyleGroup.Sum(x => x.data_value) / 2
                   };
        }
        #endregion
    }
}