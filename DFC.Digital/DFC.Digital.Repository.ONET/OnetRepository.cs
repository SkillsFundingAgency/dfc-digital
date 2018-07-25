//using AutoMapper;
//using DFC.Digital.Core;
//using DFC.Digital.Data.Model;
//using DFC.Digital.Repository.ONET.DataModel;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Threading.Tasks;

//namespace DFC.Digital.Repository.ONET
//{
//    public class OnetRepository : IOnetRepository
//    {
//        #region Fields

//        private readonly OnetSkillsFramework dbContext;
//        private readonly IMapper mapper;
//        private readonly IApplicationLogger logger;

//        #endregion Fields

//        #region ctor

//        public OnetRepository(OnetSkillsFramework dbContext, IMapper mapper, IApplicationLogger logger)
//        {
//            this.dbContext = dbContext;
//            this.mapper = mapper;
//            this.logger = logger;

//            dbContext.Database.Log = s => logger.Info(s);
//        }

//        #endregion ctor

//        #region SkillsFramework Repository Implemetation

//        public async Task<IEnumerable<T>> GetAllSocMappingsAsync<T>() where T : OnetEntity
//        {
//            var ret = await dbContext.DFC_SocMappings
//                .ProjectToListAsync<T>(mapper.ConfigurationProvider)
//                .ConfigureAwait(false);

//            return mapper.Map<IEnumerable<T>>(ret);
//        }

//        public async Task<IEnumerable<T>> GetAllTranslationsAsync<T>() where T : OnetEntity
//        {
//            var ret = await dbContext.DFC_GDSTranlations
//               .ProjectToListAsync<T>(mapper.ConfigurationProvider)
//               .ConfigureAwait(false);

//            return mapper.Map<IEnumerable<T>>(ret);
//        }

//        public async Task<IEnumerable<T>> GetAttributesValuesAsync<T>(string socCode)
//        {
//            IQueryable<OnetAttribute> attributes = null;
//            IList<T> dfcGdsAttributesDatas = null;

//            // just transforming Stored procedure to Linq . Will optimize with specification and predicates
//            await Task.Factory.StartNew(() =>
//            {
//                attributes = (KnowledgeSource(socCode, dbContext))
//                    .Union(SkillSource(socCode, dbContext))
//                    .Union(AbilitiesSource(socCode, dbContext))
//                    .Union(WorkStyleSource(socCode, dbContext))
//                    .OrderByDescending(x => x.Score);
//            }).ConfigureAwait(false);
//            var updatedAttributes = CheckAndUpdateForMathematics(attributes);
//            dfcGdsAttributesDatas = (IList<T>)DfcGdsUpdatedAttributesDatas(updatedAttributes);
//            return (IEnumerable<T>)dfcGdsAttributesDatas;
//        }

//        public async Task<T> GetDigitalSkillsAsync<T>(string socCode) where T : OnetEntity
//        {
//            IQueryable<DigitalToolsAndTechnology> dfcToolsandTech = null;
//            DigitalSkill digitalSkills = new DigitalSkill();
//            await Task.Factory.StartNew(() =>
//            {
//                dfcToolsandTech = from o in dbContext?.tools_and_technology
//                                  join od in dbContext?.unspsc_reference on o.commodity_code equals od.commodity_code
//                                  where o.onetsoc_code == socCode
//                                  orderby o.t2_type, od.class_title
//                                  select new DigitalToolsAndTechnology
//                                  {
//                                      ClassTitle = od.class_title,
//                                      T2Example = o.t2_example,
//                                      T2Type = o.t2_type,
//                                      SocCode = socCode,
//                                      OnetSocCode = o.onetsoc_code
//                                  };
//            }).ConfigureAwait(false);
//            digitalSkills = new DigitalSkill
//            {
//                DigitalSkillsCollection = dfcToolsandTech,
//                DigitalSkillsCount = dfcToolsandTech.Count()
//            };
//            return (T)Convert.ChangeType(digitalSkills, typeof(T));
//        }

//        public async Task<T> GetDigitalSkillsRankAsync<T>(string socCode) where T : struct
//        {
//            var count = 0;
//            await Task.Factory.StartNew(() =>
//            {
//                count = (from o in dbContext.tools_and_technology
//                         join od in dbContext.unspsc_reference on o.commodity_code equals od.commodity_code
//                         where o.onetsoc_code == socCode
//                         orderby o.t2_type, od.class_title
//                         select o).Count();
//            }).ConfigureAwait(false);
//            return (T)(object)count;
//        }

//        #endregion SkillsFramework Repository Implemetation

//        #region Private helper function for the interface

//        private static IQueryable<OnetAttribute> AbilitiesSource(string socCode, OnetSkillsFramework context)
//        {
//            return from ablty in context.abilities
//                   join cmr in context.content_model_reference on ablty.element_id.Substring(0, 7) equals
//                       cmr.element_id
//                   where ablty.recommend_suppress != "Y" && ablty.not_relevant != "Y" &&
//                         ablty.onetsoc_code == socCode
//                   group ablty by new
//                   {
//                       cmr.element_name,
//                       cmr.description,
//                       ablty.element_id,
//                       ablty.onetsoc_code
//                   }
//                into abilityGroup
//                   select new OnetAttribute
//                   {
//                       OnetOccupationalCode = abilityGroup.Key.onetsoc_code,
//                       Id = abilityGroup.Key.element_id,
//                       Description = abilityGroup.Key.description,
//                       Name = abilityGroup.Key.element_name,
//                       Attribute = @Attributes.Abilities,
//                       Score = abilityGroup.Sum(x => x.data_value) / 2
//                   };
//        }

//        private static IQueryable<OnetAttribute> CheckAndUpdateForMathematics(IQueryable<OnetAttribute> attributes)
//        {
//            var mathsKnowledge = attributes.ToList().FindAll(x => x.Name == "Mathematics").ToList();
//            IQueryable<OnetAttribute> newAtt = null;
//            if (mathsKnowledge.Count > 1)
//            {
//                var mathsKnowledgeValue = attributes.ToList().FirstOrDefault(x =>
//                    x.Name == "Mathematics" && x.Score == mathsKnowledge.ToList().Min(x1 => x1.Score));

//                newAtt = attributes;
//                //Remove the Min Total Value for Mathematics from attributes Skills or Knowledge
//                var suc = attributes.ToList().RemoveAll(x => x.Id.All(c =>
//                    x.Id.Contains(attributes?.ToList()?.FirstOrDefault(x2 =>
//                            x2.Name == "Mathematics" && x2.Score == mathsKnowledge?.ToList()?.Min(x3 => x3.Score))
//                        ?.Id)));

//                var update = mathsKnowledge.RemoveAll(x => x.Id == mathsKnowledgeValue?.Id);
//                // Add 110% to the Total Value in the Skills or Knowledge for Mathematics
//                var val = mathsKnowledge.Select(c =>
//                {
//                    c.Score = c.Score * (decimal)1.1;
//                    return c;
//                }).ToList();
//                //Add the Mathematics back to the Complete Attribute List
//                foreach (var k in mathsKnowledge)
//                {
//                    newAtt.ToList().RemoveAll(x => x.Id == k.Id);
//                    newAtt.ToList().AddRange(val);
//                }
//            }
//            return newAtt;
//        }

//        private static IList<OnetAttribute> DfcGdsUpdatedAttributesDatas(IQueryable<OnetAttribute> newAtt)
//        {
//            var elementAttribute = newAtt?.ToList()?.OrderByDescending(x => x.Score)
//                .Where(x => x.Attribute == Attributes.WorkStyles)
//                .OrderByDescending(z => z.Score).ThenByDescending(y => y.Attribute).Select(x => new OnetAttribute
//                {
//                    Score = x.Score,
//                    Attribute = x.Attribute,
//                    Id = x.Id,
//                    Description = x.Description,
//                    OnetOccupationalCode = x.OnetOccupationalCode,
//                    SocCode = x.SocCode
//                }).Take(5)
//                .Union(newAtt?.ToList()?.OrderByDescending(x => x.Score).Where(x => x.Attribute == Attributes.Skills)
//                    .OrderByDescending(z => z.Score).ThenByDescending(y => y.Attribute).Select(x => new OnetAttribute
//                    {
//                        Score = x.Score,
//                        Attribute = x.Attribute,
//                        Id = x.Id,
//                        Description = x.Description,
//                        OnetOccupationalCode = x.OnetOccupationalCode,
//                        SocCode = x.SocCode
//                    }).Take(5))
//                .Union(newAtt?.ToList()?.OrderByDescending(x => x.Score).Where(x => x.Attribute == Attributes.Abilities)
//                    .OrderByDescending(z => z.Score).ThenByDescending(y => y.Attribute).Select(x => new OnetAttribute
//                    {
//                        Score = x.Score,
//                        Attribute = x.Attribute,
//                        Id = x.Id,
//                        Description = x.Description,
//                        OnetOccupationalCode = x.OnetOccupationalCode,
//                        SocCode = x.SocCode
//                    }).Take(5))
//                .Union(newAtt?.ToList()?.OrderByDescending(x => x.Score).Where(x => x.Attribute == Attributes.Knowledge)
//                    .OrderByDescending(z => z.Score).ThenByDescending(y => y.Attribute).Select(x => new OnetAttribute
//                    {
//                        Score = x.Score,
//                        Attribute = x.Attribute,
//                        Id = x.Id,
//                        Description = x.Description,
//                        OnetOccupationalCode = x.OnetOccupationalCode,
//                        SocCode = x.SocCode
//                    }).Take(5));
//            var dfcGdsAttributesDatas = elementAttribute as IList<OnetAttribute> ?? elementAttribute?.ToList();
//            return dfcGdsAttributesDatas;
//        }

//        private static IQueryable<OnetAttribute> KnowledgeSource(string socCode, OnetSkillsFramework context)
//        {
//            return from knwldg in context.knowledges
//                   join cmr in context.content_model_reference on knwldg.element_id.Substring(0, 7) equals
//                       cmr.element_id
//                   where knwldg.recommend_suppress != "Y" && knwldg.not_relevant != "Y" &&
//                         knwldg.onetsoc_code == socCode
//                   group knwldg by new
//                   {
//                       cmr.element_name,
//                       cmr.description,
//                       knwldg.element_id,
//                       knwldg.onetsoc_code
//                   }
//                into knwldgGroup
//                   select new OnetAttribute
//                   {
//                       OnetOccupationalCode = knwldgGroup.Key.onetsoc_code,
//                       Id = knwldgGroup.Key.element_id,
//                       Description = knwldgGroup.Key.description,
//                       Name = knwldgGroup.Key.element_name,
//                       Attribute = Attributes.Knowledge.ToString(),
//                       Score = knwldgGroup.Sum(v => v.data_value) / 2
//                   };
//        }

//        private static IQueryable<OnetAttribute> SkillSource(string socCode, OnetSkillsFramework context)
//        {
//            return from skl in context.skills
//                   join cmr in context.content_model_reference on skl.element_id.Substring(0, 7) equals cmr
//                       .element_id
//                   where skl.recommend_suppress != "Y" && skl.not_relevant != "Y" &&
//                         skl.onetsoc_code == socCode
//                   group skl by new
//                   {
//                       skl.onetsoc_code,
//                       cmr.element_name,
//                       cmr.description,
//                       skl.element_id
//                   }
//                into skillGroup
//                   select new OnetAttribute
//                   {
//                       OnetOccupationalCode = skillGroup.Key.onetsoc_code,
//                       Id = skillGroup.Key.element_id,
//                       Description = skillGroup.Key.description,
//                       Name = skillGroup.Key.element_name,
//                       Attribute = Attributes.Skills,
//                       Score = skillGroup.Sum(x => x.data_value) / 2
//                   };
//        }

//        private static IQueryable<OnetAttribute> WorkStyleSource(string socCode, OnetSkillsFramework context)
//        {
//            return from wkstyl in context.work_styles
//                   join cmr in context.content_model_reference on wkstyl.element_id.Substring(0, 7) equals
//                       cmr.element_id
//                   where wkstyl.recommend_suppress != "Y" && wkstyl.onetsoc_code == socCode
//                   group wkstyl by new
//                   {
//                       wkstyl.onetsoc_code,
//                       cmr.element_name,
//                       cmr.description,
//                       wkstyl.element_id
//                   }
//                into workStyleGroup
//                   select new OnetAttribute
//                   {
//                       OnetOccupationalCode = workStyleGroup.Key.onetsoc_code,
//                       Id = workStyleGroup.Key.element_id,
//                       Description = workStyleGroup.Key.description,
//                       Name = workStyleGroup.Key.element_name,
//                       Attribute = Attributes.WorkStyles,
//                       Score = workStyleGroup.Sum(x => x.data_value) / 2
//                   };
//        }

//        #endregion Private helper function for the interface
//    }
//}