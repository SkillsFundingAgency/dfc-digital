using DFC.Digital.Data.Model;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class SocSkillsMatrixConverter : IDynamicModuleConverter<SocSkillMatrix>
    {
        #region Fields
        private const string SocField = "RelatedSOC";
        private const string SkillField = "RelatedSkill";
        private readonly IDynamicContentExtensions dynamicContentExtensions;
        #endregion Fields

        #region Ctor
        public SocSkillsMatrixConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }
        #endregion Ctor

        public SocSkillMatrix ConvertFrom(DynamicContent content)
        {
            var socSkillsMatrix = new SocSkillMatrix
            {
                Title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocSkillMatrix.Title)),
                Contextualised = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocSkillMatrix.Contextualised)),
                ONetAttributeType = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(SocSkillMatrix.ONetAttributeType)),
                ONetRank = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(SocSkillMatrix.ONetRank)),
                Rank = dynamicContentExtensions.GetFieldValue<decimal?>(content, nameof(SocSkillMatrix.Rank))
            };

            var socItem = dynamicContentExtensions.GetRelatedItems(content, SocField, 1).FirstOrDefault();
            if (socItem != null)
            {
                socSkillsMatrix.SocCode = dynamicContentExtensions.GetFieldValue<Lstring>(socItem, nameof(JobProfile.SOCCode));
            }

            var skillItem = dynamicContentExtensions.GetRelatedItems(content, SkillField, 1).FirstOrDefault();
            if (skillItem != null)
            {
                socSkillsMatrix.ONetElementId = dynamicContentExtensions.GetFieldValue<Lstring>(skillItem, nameof(socSkillsMatrix.ONetElementId));
                socSkillsMatrix.Skill = dynamicContentExtensions.GetFieldValue<Lstring>(skillItem, nameof(socSkillsMatrix.Title));
            }

            return socSkillsMatrix;
        }
    }
}