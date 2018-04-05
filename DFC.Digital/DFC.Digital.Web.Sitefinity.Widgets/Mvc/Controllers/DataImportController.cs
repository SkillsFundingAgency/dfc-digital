using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Security.Claims;

namespace DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for Data Import Tool
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "DataImport", Title = "Data Import Tool", SectionName = SitefinityConstants.CustomAdminWidgetSection)]
    public class DataImportController : BaseDfcController
    {
        #region Private members

        private readonly IBauJobProfileOdataRepository bauJobProfileRepository;
        private readonly IManageBauJobProfilesService manageBauJobProfilesService;
        private readonly IJobProfileRepository jobProfileRepository;
        private readonly IAsyncHelper asyncHelper;

        #endregion

        #region Constructors

        public DataImportController(IApplicationLogger applicationLogger, IBauJobProfileOdataRepository bauJobProfileRepository, IManageBauJobProfilesService manageBauJobProfilesService, IJobProfileRepository jobProfileRepository, IAsyncHelper asyncHelper)
            : base(applicationLogger)
        {
            this.bauJobProfileRepository = bauJobProfileRepository;
            this.manageBauJobProfilesService = manageBauJobProfilesService;
            this.jobProfileRepository = jobProfileRepository;
            this.asyncHelper = asyncHelper;
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
        public string PageTitle { get; set; } = "BAU JP Data Import";

        /// <summary>
        /// Gets or sets the source to destination property mapping.
        /// </summary>
        /// <value>
        /// The source to destination property mapping.
        /// </value>
        public string SourceToDestinationPropertyMapping { get; set; }

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
            var model = new DataImportViewModel
            {
                PageTitle = PageTitle,
                NotAllowedMessage = NotAllowedMessage,
                IsAdmin = IsUserAdministrator()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DataImportViewModel viewModel)
        {
            if (IsUserAdministrator())
            {
                try
                {
                    var mappingDictionary = GetPropertyMappingDictionary();
                    var sourceJobProfiles = asyncHelper.Synchronise(GetAllJobProfilesBySourcePropertiesAsync);

                    var markedJobProfiles = manageBauJobProfilesService.SelectMarkedJobProfiles(sourceJobProfiles, new List<string>());

                    var result = jobProfileRepository.AddOrUpdateJobProfileByProperties(markedJobProfiles, mappingDictionary);

                    if (result)
                    {
                        viewModel.ResultText = "Import was complete successfully";
                    }
                    else
                    {
                        viewModel.ResultText = "There was a problem with the import";
                    }
                }
                catch (Exception ex)
                {
                    viewModel.ResultText = ex.Message + "<br />" + ex.InnerException + "<br />" + ex.StackTrace;
                }
            }
            else
            {
                viewModel.ResultText = NotAllowedMessage;
            }

            return View(viewModel);
        }

        #endregion Actions

        #region Non Action Methods

        private static Dictionary<string, string> GetPropertyMappingDictionary()
        {
            return new Dictionary<string, string>();
        }

        private static bool IsUserAdministrator()
        {
            var userAdminRole = ClaimsManager.GetCurrentIdentity().Roles.FirstOrDefault(x => x.Name == "Administrators");
            return userAdminRole != null;
        }

        private async Task<IEnumerable<BauJobProfile>> GetAllJobProfilesBySourcePropertiesAsync()
        {
            return await bauJobProfileRepository.GetAllJobProfilesBySourcePropertiesAsync();
        }
        #endregion Non Action Methods
    }
}