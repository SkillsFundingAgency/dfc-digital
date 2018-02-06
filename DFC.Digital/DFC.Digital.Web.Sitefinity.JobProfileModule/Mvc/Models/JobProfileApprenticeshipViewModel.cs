using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    /// <summary>
    /// Job Profile Apprenticeship View Model
    /// </summary>
    public class JobProfileApprenticeshipViewModel
    {
        /// <summary>
        /// Gets or sets the apprentice vacancies.
        /// </summary>
        /// <value>
        /// The apprentice vacancies.
        /// </value>
        public IEnumerable<ApprenticeVacancy> ApprenticeVacancies { get; set; } = Enumerable.Empty<ApprenticeVacancy>();

        /// <summary>
        /// Gets or sets the section title.
        /// </summary>
        /// <value>
        /// The section title.
        /// </value>
        public string ApprenticeshipSectionTitle { get; set; }

        /// <summary>
        /// Gets or sets the location details.
        /// </summary>
        /// <value>
        /// The location details.
        /// </value>
        public string LocationDetails { get; set; }

        /// <summary>
        /// Gets or sets the find apprenticeship link.
        /// </summary>
        /// <value>
        /// The find apprenticeship link.
        /// </value>
        public string FindApprenticeshipLink { get; set; }

        /// <summary>
        /// Gets or sets the find apprenticeship text.
        /// </summary>
        /// <value>
        /// The find apprenticeship text.
        /// </value>
        public string FindApprenticeshipText { get; set; }

        /// <summary>
        /// Gets or sets the big section title and number.
        /// </summary>
        /// <value>
        /// The big section title and number.
        /// </value>
        public string MainSectionTitle { get; set; }

        /// <summary>
        /// Gets or sets the no vacancy text.
        /// </summary>
        /// <value>
        /// The no vacancy text.
        /// </value>
        public string NoVacancyText { get; set; }

        /// <summary>
        /// Gets a value indicating whether [apprentice ships available].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [apprentice ships available]; otherwise, <c>false</c>.
        /// </value>
        public bool ApprenticeshipsAvailable => ApprenticeVacancies != null && ApprenticeVacancies.Any();

        /// <summary>
        /// Gets the section identifier.
        /// </summary>
        /// <value>
        /// The section identifier.
        /// </value>
        public string SectionId { get; internal set; }
    }
}