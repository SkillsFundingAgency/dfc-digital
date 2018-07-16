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
                    Location = GetRelatedItemDescription(content, RelatedLocationsField),
                    Uniform = GetRelatedItemDescription(content, RelatedUniformsField),
                    Environment = GetRelatedItemDescription(content, RelatedEnvironmentsField)
                };
        }

        #endregion Public methods

        #region private methods

        private string GetRelatedItemDescription(DynamicContent content, string relatedField)
        {
            var item = dynamicContentExtensions.GetRelatedItems(content, relatedField).FirstOrDefault();
            if (item != null)
            {
                return dynamicContentExtensions.GetFieldValue<Lstring>(item, DescriptionField);
            }

            return null;
        }

        #endregion private methods
    }
}