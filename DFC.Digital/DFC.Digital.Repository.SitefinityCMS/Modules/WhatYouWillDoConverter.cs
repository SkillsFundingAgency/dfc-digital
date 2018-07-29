using DFC.Digital.Data.Model;
using DFC.Digital.Repository.SitefinityCMS;
using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Sitefinity.DynamicModules.Model;
using Telerik.Sitefinity.Model;

namespace DFC.Digital.Repository.SitefinityCMS.Modules
{
    public class WhatYouWillDoConverter : IContentPropertyConverter<WhatYouWillDo>
    {
        #region Fields

        private const string RelatedLocationsField = "RelatedLocations";
        private const string DescriptionField = "Description";
        private const string RelatedEnvironmentsField = "RelatedEnvironments";
        private const string RelatedUniformsField = "RelatedUniforms";
        private const string DailyTasksField = "WYDDayToDayTasks";
        private const string IntroductionField = "WYDIntroduction";
        private const string IsCadReadyField = "IsWYDCadReady";
        private readonly IDynamicContentExtensions dynamicContentExtensions;

        #endregion Fields

        #region Ctor

        public WhatYouWillDoConverter(IDynamicContentExtensions dynamicContentExtensions)
        {
            this.dynamicContentExtensions = dynamicContentExtensions;
        }

        #endregion Ctor

        #region Public methods

        public WhatYouWillDo ConvertFrom(DynamicContent content)
        {
            var isCadReady =
                dynamicContentExtensions.GetFieldValue<bool>(content, IsCadReadyField);
            return !isCadReady
                ? new WhatYouWillDo()
                : new WhatYouWillDo
                {
                    IsCadReady = true,
                    Introduction =
                        dynamicContentExtensions.GetFieldValue<Lstring>(content, IntroductionField),
                    DailyTasks =
                        dynamicContentExtensions.GetFieldValue<Lstring>(content, DailyTasksField),
                    Locations = GetRelatedItemDescription(content, RelatedLocationsField),
                    Uniforms = GetRelatedItemDescription(content, RelatedUniformsField),
                    Environments = GetRelatedItemDescription(content, RelatedEnvironmentsField)
                };
        }

        #endregion Public methods

        #region private methods

        private IEnumerable<string> GetRelatedItemDescription(DynamicContent content, string relatedField)
        {
            var items = dynamicContentExtensions.GetRelatedItems(content, relatedField);

            if (items != null && items.Any())
            {
                foreach (var item in items)
                {
                    yield return dynamicContentExtensions.GetFieldValue<Lstring>(item, DescriptionField);
                }
            }
        }

        #endregion private methods
    }
}