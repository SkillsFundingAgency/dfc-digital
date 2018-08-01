using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.SkillsFramework
{
    public class SkillFrameworkBusinessRuleEngine : ISkillFrameworkBusinessRuleEngine
    {
        private readonly IMapper autoMapper;
        private readonly ISkillsRepository knowledgeQueryRepository;
        private readonly ISkillsRepository abilitiesOueryRepository;
        private readonly ISkillsRepository skillsOueryRepository;
        private readonly ISkillsRepository workStyleRepository;
        private readonly IList<FrameworkSkill> suppressions;
        private readonly IList<FrameWorkSkillCombination> combinations;

        private readonly string MathsTitle = "Mathematics";

        public SkillFrameworkBusinessRuleEngine(IMapper autoMapper, ISkillsRepository knowledgeQueryRepository, ISkillsRepository skillsOueryRepository, ISkillsRepository abilitiesOueryRepository,
          ISkillsRepository workStyleQueryRepository, IQueryRepository<FrameworkSkill> suppressionsQueryRepository, IQueryRepository<FrameWorkSkillCombination> combinationsQueryRepository)
        {
            this.autoMapper = autoMapper;
            this.knowledgeQueryRepository = knowledgeQueryRepository;
            this.abilitiesOueryRepository = abilitiesOueryRepository;
            this.skillsOueryRepository = skillsOueryRepository;
            this.workStyleRepository = workStyleQueryRepository;

            suppressions = suppressionsQueryRepository.GetAll().ToList();
            combinations = combinationsQueryRepository.GetAll().ToList(); 
        }

        public IEnumerable<OnetAttribute> AverageOutScoreScales(IEnumerable<OnetAttribute> attributes)
        {

            var averagedScales = attributes.GroupBy(a => new { a.Id, a.Type, a.OnetOccupationalCode, a.Name }).
            Select(c => new OnetAttribute { Id = c.Key.Id,
                Type = c.Key.Type,
                OnetOccupationalCode = c.Key.OnetOccupationalCode,
                Name = c.Key.Name,
                Score = (c.Sum(s => (s.Score)) / 2) });
           // var ret = averagedScales.ToList();
            return averagedScales;
        }


        public IEnumerable<OnetAttribute> BoostMathsSkills(IEnumerable<OnetAttribute> attributes)
        {
            OnetAttribute skillsMaths = attributes.Where(a => a.Name == MathsTitle && a.Type == AttributeType.Skill).FirstOrDefault();
            OnetAttribute knowledgeMaths = attributes.Where(a => a.Name == MathsTitle && a.Type == AttributeType.Knowledge).FirstOrDefault();

            //if we have both remove one
            if (skillsMaths != null && knowledgeMaths != null)
            {
                if (skillsMaths.Score > knowledgeMaths.Score)
                {
                    attributes = attributes.Where(a => a.Name != MathsTitle || a.Type != AttributeType.Knowledge);
                }
                else
                {
                    attributes = attributes.Where(a => a.Name != MathsTitle || a.Type != AttributeType.Skill);
                }

                var attributeList = attributes.ToList();
                for (int ii = 0; ii < attributeList.Count(); ii++)
                {
                    if (attributeList[ii].Name == MathsTitle)
                    {
                        attributeList[ii].Score = attributeList[ii].Score * 1.1m;
                    }
                }
                return attributeList;
            }
            return attributes;
        }

        public IEnumerable<OnetAttribute> CombineSimilarAttributes(IEnumerable<OnetAttribute> attributes)
        {
            foreach (var combination in combinations)
            {
                var topAttributes = SelectTopAttributes(attributes);

                OnetAttribute elementOne = topAttributes.Where(a => a.Id == combination.OnetElementOneId).FirstOrDefault();
                OnetAttribute elementTwo = topAttributes.Where(a => a.Id == combination.OnetElementTwoId).FirstOrDefault();

                //if we have both combine them
                if (elementOne != null && elementTwo != null)
                {
                    var scoreToUse = (elementOne.Score > elementTwo.Score) ? elementOne.Score : elementTwo.Score;

                    attributes = attributes.Where(a => a.Id != elementOne.Id && a.Id != elementTwo.Id);

                    attributes.ToList().Add(new OnetAttribute { Name = combination.Title, Id = combination.CombinedElementId, OnetOccupationalCode = elementOne.OnetOccupationalCode, Score = scoreToUse });
                }
            }

            return attributes;
        }

        public IQueryable<OnetAttribute> GetAllRawOnetSkillsForOccupation(string onetOccupationalCode)
        {

            var allSkillForOccupation = knowledgeQueryRepository.GetSkillsForONetOccupationCode(onetOccupationalCode)
                .Union(abilitiesOueryRepository.GetSkillsForONetOccupationCode(onetOccupationalCode)
                .Union(skillsOueryRepository.GetSkillsForONetOccupationCode(onetOccupationalCode))
                .Union(workStyleRepository.GetSkillsForONetOccupationCode(onetOccupationalCode)));
    
            return allSkillForOccupation;
        }

        public DigitalSkillsLevel GetDigitalSkillsLevel(int count)
        {
                var rankValue= count > 150 ? DigitalSkillsLevel.Level1
                     : count > 100 ? DigitalSkillsLevel.Level2
                     : count > 50 ? DigitalSkillsLevel.Level3
                     : DigitalSkillsLevel.Level4;
            return rankValue;
        }

        /// <summary>
        /// Any items that have a key length > 7 are at the bottom level so trim them to 7 to move up one node
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IEnumerable<OnetAttribute> MoveBottomLevelAttributesUpOneLevel(IEnumerable<OnetAttribute> attributes)
        {
            var bottomNodesUpOne = attributes.Select(a => { a.Id = (a.Id.Length < 7) ? a.Id : a.Id.Substring(0, 7); return a; });
            //var ret = bottomNodesUpOne.ToList();
            return bottomNodesUpOne;
        }

        public IEnumerable<OnetAttribute> RemoveDFCSuppressions(IEnumerable<OnetAttribute> attributes)
        {
            var suppressionsRemoved = attributes.Where(a => !suppressions.Any(s => s.OnetElementId == a.Id));
            return suppressionsRemoved;
        }

        /// <summary>
        /// Remove duplicate attributes, keeping the one with the highest score
        /// </summary>
        /// <param name="attributes"></param>
        /// <returns></returns>
        public IEnumerable<OnetAttribute> RemoveDuplicateAttributes(IEnumerable<OnetAttribute> attributes)
        {
            var duplicatesRemoved = attributes.GroupBy(a => a.Id).Select(b => b.OrderByDescending(c => c.Score).First());
            //var ret = duplicatesRemoved.ToList();
            return duplicatesRemoved;
        }

        public IEnumerable<OnetAttribute> SelectFinalAttributes(IEnumerable<OnetAttribute> attributes)
        {
            var finalAttributes = attributes.Where(a => a.Type == AttributeType.Combination)
                .Union(SelectTopAttributes(attributes));

            return finalAttributes.OrderByDescending(s => s.Score).Take(20);
        }

        private IEnumerable<OnetAttribute> SelectTopAttributes(IEnumerable<OnetAttribute> attributes)
        {
            var topAttributes = attributes.Where(a => a.Type == AttributeType.Ability).OrderByDescending(s => s.Score).Take(5)
                .Union(attributes.Where(a => a.Type == AttributeType.Knowledge).OrderByDescending(s => s.Score).Take(5))
                .Union(attributes.Where(a => a.Type == AttributeType.Skill).OrderByDescending(s => s.Score).Take(5))
                .Union(attributes.Where(a => a.Type == AttributeType.WorkStyle).OrderByDescending(s => s.Score).Take(5));

            return topAttributes;
        }



    }
}
