using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Sitefinity.DynamicModules.Model;

namespace DFC.Digital.Repository.SitefinityCMS.CMSExtensions
{
    public class StandardAndFrameworkReportConverter : IDynamicModuleConverter<StandardAndFrameworkReport>
    {
        private const string ApprenticeshipFrameworks = "apprenticeshipframeworks";
        private const string ApprenticeshipFrameworksTaxonomyName = "apprenticeship-frameworks";
        private const string ApprenticeshipStandardsRelatedField = "apprenticeshipstandards";
        private const string ApprenticeshipStandardsTaxonomyName = "apprenticeship-standards";

        private readonly IDynamicModuleConverter<SocCode> socCodeConverter;
        private readonly IRelatedClassificationsRepository relatedClassificationsRepository;

        public StandardAndFrameworkReportConverter(
            IDynamicModuleConverter<SocCode> socCodeConverter,
            IRelatedClassificationsRepository relatedClassificationsRepository,
            IDynamicContentExtensions dynamicContentExtensions)
        {
            this.socCodeConverter = socCodeConverter;
            this.relatedClassificationsRepository = relatedClassificationsRepository;
        }

        public StandardAndFrameworkReport ConvertFrom(DynamicContent content)
        {
            var socCode = socCodeConverter.ConvertFrom(content);
            return new StandardAndFrameworkReport
            {
                SocCode = socCode,
                Frameworks = relatedClassificationsRepository.GetRelatedClassifications(content, ApprenticeshipFrameworks, ApprenticeshipFrameworksTaxonomyName),
                Standards = relatedClassificationsRepository.GetRelatedClassifications(content, ApprenticeshipStandardsRelatedField, ApprenticeshipStandardsTaxonomyName),
            };
        }
    }
}
