using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Sitefinity.Core.Interface;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for Job Profile ApprenticeShips
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.Base.BaseDfcController" />
    /// <seealso cref="Web.Core.Base.BaseDfcController" />
    [ControllerToolboxItem(Name = "JobProfileApprenticeships", Title = "JobProfile Apprenticeships", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileApprenticeshipsController : BaseJobProfileWidgetController
    {
        #region Private Fields

        /// <summary>
        /// The job profile soc code repository
        /// </summary>
        private readonly IJobProfileSocCodeRepository jobProfileSocCodeRepository;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JobProfileApprenticeshipsController" /> class.
        /// </summary>
        /// <param name="jobProfileRepository">The repository.</param>
        /// <param name="webAppContext">The web application context.</param>
        /// <param name="jobProfileSocCodeRepository">The job profile soc code repository.</param>
        /// <param name="applicationLogger">The application logger.</param>
        /// <param name="sitefinityPage">Sitefinity page</param>
        public JobProfileApprenticeshipsController(IJobProfileRepository jobProfileRepository, IWebAppContext webAppContext, IJobProfileSocCodeRepository jobProfileSocCodeRepository, IApplicationLogger applicationLogger, ISitefinityPage sitefinityPage)
            : base(webAppContext, jobProfileRepository, applicationLogger, sitefinityPage)
        {
            this.jobProfileSocCodeRepository = jobProfileSocCodeRepository;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the big section title and number.
        /// </summary>
        /// <value>
        /// The big section title and number.
        /// </value>
        public string MainSectionTitle { get; set; }

        /// <summary>
        /// Gets or sets the section identifier.
        /// </summary>
        /// <value>
        /// The section identifier.
        /// </value>
        public string SectionId { get; set; } = "current-opportunities";

        /// <summary>
        /// Gets or sets the section title.
        /// </summary>
        /// <value>
        /// The section title.
        /// </value>
        public string ApprenticeshipSectionTitle { get; set; } = "Apprenticeships";

        /// <summary>
        /// Gets or sets the location details.
        /// </summary>
        /// <value>
        /// The location details.
        /// </value>
        public string ApprenticeshipLocationDetails { get; set; } = "In England";

        /// <summary>
        /// Gets or sets the apprenticeship text.
        /// </summary>
        /// <value>
        /// The apprenticeship text.
        /// </value>
        public string ApprenticeshipText { get; set; } = "<a href=\"https://www.findapprenticeship.service.gov.uk/apprenticeshipsearch\">Find apprenticeships near you</a>";

        /// <summary>
        /// Gets or sets the no vacancy text.
        /// </summary>
        /// <value>
        /// The no vacancy text.
        /// </value>
        public string NoVacancyText { get; set; } = "Right now there aren't any apprenticeships in this field of work. Consider exploring the related careers or <a href=\"https://www.findapprenticeship.service.gov.uk/apprenticeshipsearch\">find other apprenticeships.</a>";

        /// <summary>
        /// Gets or sets the maximum apprenticeship count.
        /// </summary>
        /// <value>
        /// The maximum apprenticeship count.
        /// </value>
        public int MaxApprenticeshipCount { get; set; } = 2;

        #endregion Public Properties

        #region Actions

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Action Result</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            return BaseIndex();
        }

        /// <summary>
        /// Indexes the specified urlname.
        /// </summary>
        /// <param name="urlName">The urlname.</param>
        /// <returns>Action Result</returns>
        [HttpGet]
        [RelativeRoute("{urlName}")]
        public ActionResult Index(string urlName)
        {
            return BaseIndex(urlName);
        }

        protected override ActionResult GetDefaultView()
        {
            IQueryable<ApprenticeVacancy> apprenticeshipVacancies = null;
            if (!string.IsNullOrWhiteSpace(CurrentJobProfile.SOCCode))
            {
                apprenticeshipVacancies = jobProfileSocCodeRepository.GetBySocCode(CurrentJobProfile.SOCCode)
                    ?.Where(x => !string.IsNullOrEmpty(x.Title)
                        && !string.IsNullOrEmpty(x.URL.OriginalString)
                        && !string.IsNullOrEmpty(x.WageUnitType)
                        && !string.IsNullOrEmpty(x.WageAmount)
                        && !string.IsNullOrEmpty(x.Location)
                        && !string.IsNullOrEmpty(x.VacancyId))
                    .Take(MaxApprenticeshipCount);
            }

            var model = new JobProfileApprenticeshipViewModel
            {
                ApprenticeVacancies = apprenticeshipVacancies,
                ApprenticeshipSectionTitle = ApprenticeshipSectionTitle,
                SectionId = SectionId,
                LocationDetails = ApprenticeshipLocationDetails,
                ApprenticeshipText = ApprenticeshipText.Replace("{jobtitle}", CurrentJobProfile.Title.ToLowerInvariant()),
                NoVacancyText = NoVacancyText.Replace("{jobtitle}", CurrentJobProfile.Title.ToLowerInvariant()),
                MainSectionTitle = MainSectionTitle,
            };

            return View("Index", model);
        }

        protected override ActionResult GetEditorView()
        {
            if (CurrentJobProfile == null)
            {
                var demoModel = new JobProfileApprenticeshipViewModel
                {
                    ApprenticeVacancies = new List<ApprenticeVacancy>(),
                    ApprenticeshipSectionTitle = ApprenticeshipSectionTitle,
                    SectionId = SectionId,
                    LocationDetails = ApprenticeshipLocationDetails,
                    ApprenticeshipText = ApprenticeshipText,
                    MainSectionTitle = MainSectionTitle,
                    NoVacancyText = NoVacancyText,
                };

                return View("Index", demoModel);
            }
            else
            {
                return GetDefaultView();
            }
        }

        #endregion Actions
    }
}