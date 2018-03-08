using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Core.Base;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Security;
using Telerik.Sitefinity.Security.Claims;
using Telerik.Sitefinity.Services;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for Admin Panel
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.Base.BaseDfcController" />
    [ControllerToolboxItem(Name = "AdminPanel", Title = "Admin Panel", SectionName = SitefinityConstants.CustomAdminWidgetSection)]
    public class AdminPanelController : BaseDfcController
    {
        #region Constructors

        public AdminPanelController(IApplicationLogger applicationLogger) : base(applicationLogger)
        {
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
        public string PageTitle { get; set; } = "Admin Panel";

        /// <summary>
        /// Gets or sets the First Paragraph.
        /// </summary>
        /// <value>
        /// The Page Title.
        /// </value>
        [DisplayName("First Paragraph")]
        public string FirstParagraph { get; set; } = "Select the mode and Restart Sitefinity application.";

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
            var model = new AdminPanelViewModel();
            model.PageTitle = PageTitle;
            model.FirstParagraph = FirstParagraph;
            model.NotAllowedMessage = NotAllowedMessage;
            model.IsAdmin = IsUserAdministrator();

            return View(model);
        }

        [HttpPost]
        public ActionResult RestartSitefinity(string restartMode)
        {
            string resultText = string.Empty;

            if (IsUserAdministrator())
            {
                try
                {
                    var systemRestartFlag = SystemRestartFlags.Default;

                    switch (restartMode?.ToUpperInvariant().Trim())
                    {
                        case "ATTEMPTFULLRESTART":
                            resultText = "Success - Sitefinity was restarted in FULL Restart Mode.";
                            systemRestartFlag = SystemRestartFlags.AttemptFullRestart;
                            break;
                        case "RESETMODEL":
                            resultText = "Success - Sitefinity was restarted in DATABASE Model Reset Mode.";
                            systemRestartFlag = SystemRestartFlags.ResetModel;
                            break;
                        default:
                            resultText = "Success - Sitefinity was restarted in SOFT Restart Mode.";
                            break;
                    }

                    SystemManager.RestartApplication("Restart invoked through the Sitefinity CMS API", systemRestartFlag, true);
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
        #endregion Methods
    }
}