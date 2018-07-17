using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Security.Claims;
using Telerik.Sitefinity.Services;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for Admin Panel
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "OnetDataImport", Title = "Onet Data Import Widget", SectionName = SitefinityConstants.CustomAdminWidgetSection)]
    public class OnetDataImportController : BaseDfcController
    {
        #region Private Members

        private readonly IImportOnetDataService importOnetDataService;
        private readonly IReportAuditRepository reportAuditRepository;

        #endregion Private Members

        #region Constructors

        public OnetDataImportController(IApplicationLogger applicationLogger, IImportOnetDataService importOnetDataService, IReportAuditRepository reportAuditRepository) : base(applicationLogger)
        {
            this.importOnetDataService = importOnetDataService;
            this.reportAuditRepository = reportAuditRepository;
        }

        #endregion Constructors

        #region Public Properties

        /// <summary>
        /// Gets or sets the Page Title.
        /// </summary>
        /// <value>
        /// The Page Title.
        /// </value>
        [DisplayName("Page Title")]
        public string PageTitle { get; set; } = "Onet Data Import";

        /// <summary>
        /// Gets or sets the First Paragraph.
        /// </summary>
        /// <value>
        /// The Page Title.
        /// </value>
        [DisplayName("First Paragraph")]
        public string FirstParagraph { get; set; } = "Select the Onet data import process you want to perform.";

        /// <summary>
        /// Gets or sets the First Paragraph.
        /// </summary>
        /// <value>
        /// The Page Title.
        /// </value>
        [DisplayName("Not Allowed Message")]
        public string NotAllowedMessage { get; set; } = "You are not allowed to use this functionality. Only Administrators are.";

        #endregion Public Properties

        #region Actions

        // GET: AdminPanel
        public ActionResult Index()
        {
            var model = new OnetDataImportViewModel
            {
                PageTitle = PageTitle,
                FirstParagraph = FirstParagraph,
                NotAllowedMessage = NotAllowedMessage,
                IsAdmin = IsUserAdministrator()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(string importMode)
        {
            var importResult = new OnetImportResultsViewModel
            {
                PageTitle = PageTitle,
                FirstParagraph = FirstParagraph,
                NotAllowedMessage = NotAllowedMessage,
                IsAdmin = IsUserAdministrator()
            };

            var otherMessage = string.Empty;

            if (IsUserAdministrator())
            {
                try
                {
                    switch (importMode?.ToUpperInvariant().Trim())
                    {
                        case "IMPORTSKILLS":
                            var result = importOnetDataService.ImportOnetSkills();
                            importResult.ActionCompleted = "Import Onet Skills";
                            break;
                        case "UPDATESOCOCCUPATIONALCODES":
                            var updateResult = importOnetDataService.UpdateSocCodesOccupationalCode();
                            importResult.ActionCompleted = "Update Soc Codes with Onet Occupational Codes";
                            importResult.SummaryDetails = updateResult.SummaryDetails;
                            importResult.ErrorMessages = updateResult.ErrorMessages;
                            importResult.ImportDetails = updateResult.ActionDetails;
                            break;
                        case "UPDATEJPDIGITALSKILLS":
                          var updatejpDigiResult = importOnetDataService.UpdateJobProfilesDigitalSkills();
                            importResult.ActionCompleted = "Update Job Profiles With Digital Skill levels";
                            importResult.SummaryDetails = updatejpDigiResult.SummaryDetails;
                            importResult.ErrorMessages = updatejpDigiResult.ErrorMessages;
                            importResult.ImportDetails = updatejpDigiResult.ActionDetails;
                            break;
                        case "BUILDSOCMATRIX":
                            var buildsocResult = importOnetDataService.BuildSocMatrixData();
                            importResult.ActionCompleted = "Build Soc Skill Matrix";
                            importResult.SummaryDetails = buildsocResult.SummaryDetails;
                            importResult.ErrorMessages = buildsocResult.ErrorMessages;
                            importResult.ImportDetails = buildsocResult.ActionDetails;
                            break;
                        case "UPDATEJPSKILLS":
                            var upjpsocResult = importOnetDataService.UpdateJpSocSkillMatrix();
                            importResult.ActionCompleted = "Update Job Profiles with related Soc sklii Matrices";
                            importResult.SummaryDetails = upjpsocResult.SummaryDetails;
                            importResult.ErrorMessages = upjpsocResult.ErrorMessages;
                            importResult.ImportDetails = upjpsocResult.ActionDetails;
                            break;
                    }
                   }
                catch (Exception ex)
                {
                    otherMessage = ex.Message + "<br />" + ex.InnerException + "<br />" + ex.StackTrace;
                }
            }
            else
            {
                otherMessage = NotAllowedMessage;
            }

            importResult.AuditRecords = reportAuditRepository.GetAllAuditRecords();
            importResult.OtherMessage = otherMessage;

            return View("ImportResults", importResult);
        }

        #endregion Actions

        #region Non Action Methods

        //CodeReview: This should be in ISitefinityContext to enable us unit test the controller.
        private static bool IsUserAdministrator()
        {
            var userAdminRole = ClaimsManager.GetCurrentIdentity().Roles.Where(x => x.Name == "Administrators").FirstOrDefault();
            return userAdminRole != null ? true : false;
        }

        #endregion Non Action Methods
    }
}