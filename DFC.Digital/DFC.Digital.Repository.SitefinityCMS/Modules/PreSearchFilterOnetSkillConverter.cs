using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.OpenAccess;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;
using Telerik.Sitefinity.Taxonomies;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class PreSearchFilterOnetSkillConverter : IDynamicModuleConverter<PsfOnetSkill>
    {
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        public PreSearchFilterOnetSkillConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        public PsfOnetSkill ConvertFrom(DynamicContent content)
        {
            var isHidden = dynamicContentExtensions.GetFieldValue<bool>(content, "PSFHidden");
            if (isHidden)
            {
                return default;
            }
            else
            {
                var title = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.Title));
                var psfTitle = dynamicContentExtensions.GetFieldValue<Lstring>(content, "PSFLabel");
                var psfDescription = dynamicContentExtensions.GetFieldValue<Lstring>(content, $"PSF{nameof(PreSearchFilter.Description)}");
                PsfOnetSkill psfOnetSkill = new PsfOnetSkill();
                psfOnetSkill.Title = string.IsNullOrEmpty(psfTitle) ? title : psfTitle;
                psfOnetSkill.Description = !string.IsNullOrEmpty(psfDescription) ? psfDescription : dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.Description));
                psfOnetSkill.NotApplicable = dynamicContentExtensions.GetFieldValue<bool>(content, $"PSF{nameof(PreSearchFilter.NotApplicable)}");
                psfOnetSkill.Order = dynamicContentExtensions.GetFieldValue<decimal?>(content, $"PSF{nameof(PreSearchFilter.Order)}") ?? 0;
                psfOnetSkill.UrlName = dynamicContentExtensions.GetFieldValue<Lstring>(content, nameof(PreSearchFilter.UrlName));
                psfOnetSkill.PSFCategory = GetPSFCategoryClassification(content.GetValue<TrackedList<Guid>>("PSFCategories"));
                psfOnetSkill.Id = dynamicContentExtensions.GetFieldValue<Guid>(content, nameof(PreSearchFilter.Id));

                return psfOnetSkill;
            }
        }

        private string GetPSFCategoryClassification(TrackedList<Guid> classifications)
        {
            var classificationData = new List<Classification>();
            TaxonomyManager taxonomyManager = TaxonomyManager.GetManager();
            var category = classifications.FirstOrDefault();

            return category != Guid.Empty ? (string)taxonomyManager.GetTaxon(category).Title : null;
        }
    }
}