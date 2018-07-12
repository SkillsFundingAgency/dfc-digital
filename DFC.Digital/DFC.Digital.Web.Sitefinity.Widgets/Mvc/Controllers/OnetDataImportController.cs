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

        #endregion Private Members

        #region Constructors

        public OnetDataImportController(IApplicationLogger applicationLogger, IImportOnetDataService importOnetDataService) : base(applicationLogger)
        {
            this.importOnetDataService = importOnetDataService;
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
            string resultText = string.Empty;

            if (IsUserAdministrator())
            {
                try
                {
                    switch (importMode?.ToUpperInvariant().Trim())
                    {
                        case "IMPORTSKILLS":
                            var result = importOnetDataService.ImportOnetSkills();
                            break;
                        case "UPDATESOCOCCUPATIONALCODES":
                            resultText = "Success - Sitefinity was restarted in DATABASE Model Reset Mode.";
                            break;
                        case "UPDATEJPDIGITALSKILLS":
                            resultText = "Success - Sitefinity was restarted in FULL Restart Mode.";
                            break;
                        case "BUILDSOCMATRIX":
                            resultText = "Success - Sitefinity was restarted in DATABASE Model Reset Mode.";
                            break;
                        case "UPDATEJPSKILLS":
                            resultText = "Success - Sitefinity was restarted in DATABASE Model Reset Mode.";
                            break;
                    }
                   }
                catch (Exception ex)
                {
                    resultText = ex.Message + "<br />" + ex.InnerException + "<br />" + ex.StackTrace;
                }
            }
            else
            {
                resultText = NotAllowedMessage;
            }

            return Json(new { Result = resultText }, JsonRequestBehavior.AllowGet);
        }

        #endregion Actions

        #region Non Action Methods
        private static bool IsUserAdministrator()
        {
            var userAdminRole = ClaimsManager.GetCurrentIdentity().Roles.Where(x => x.Name == "Administrators").FirstOrDefault();
            return userAdminRole != null ? true : false;
        }

        #endregion Non Action Methods
    }
}