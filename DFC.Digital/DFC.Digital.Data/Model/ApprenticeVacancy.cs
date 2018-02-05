using DFC.Digital.Data.Interfaces;

namespace DFC.Digital.Data.Model
{
    /// <summary>
    /// Apprentice Vacancy Model
    /// </summary>
    public class ApprenticeVacancy : IDigitalDataModel
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the type of the wage unit.
        /// </summary>
        /// <value>
        /// The type of the wage unit.
        /// </value>
        public string WageUnitType { get; set; }

        /// <summary>
        /// Gets or sets the wage unit amount.
        /// </summary>
        /// <value>
        /// The wage unit amount.
        /// </value>
        public string WageAmount { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the vacancy identifier.
        /// </summary>
        /// <value>
        /// The vacancy identifier.
        /// </value>
        public string VacancyId { get; set; }
    }
}