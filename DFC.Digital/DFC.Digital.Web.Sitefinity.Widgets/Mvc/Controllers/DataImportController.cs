using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.Widgets.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
        /// Gets or sets the instructions text.
        /// </summary>
        /// <value>
        /// The instructions text.
        /// </value>
        [DisplayName("Instructions Text")]
        public string InstructionsText { get; set; } = "Please upload a data source for b profiles you have marked";

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
                InstructionsText = InstructionsText,
                IsAdmin = IsUserAdministrator()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DataImportViewModel model)
        {
            if (IsUserAdministrator())
            {
                var markedJobPrilesUrlnames = new List<string>();

                var mappingDictionary = GetPropertyMappingDictionary();
                var csvreader = new StreamReader(model.JobProfileImportDataFile.InputStream);

                while (!csvreader.EndOfStream)
                {
                    var line = csvreader.ReadLine();
                    markedJobPrilesUrlnames.Add(line);
                }

                var sourceJobProfiles = asyncHelper.Synchronise(GetAllJobProfilesBySourcePropertiesAsync);
                var markedJobProfiles =
                    manageBauJobProfilesService.SelectMarkedJobProfiles(sourceJobProfiles, markedJobPrilesUrlnames);

                bool errorOccurred = false;
                foreach (var bauJobProfile in markedJobProfiles)
                {
                    try
                    {
                        jobProfileRepository.AddOrUpdateJobProfileByProperties(bauJobProfile, mappingDictionary);
                    }
                    catch (Exception ex)
                    {
                        errorOccurred = true;
                        model.ResultText += ex.Message + "<br />" + ex.InnerException + "<br />" + ex.StackTrace;
                    }
                }

                if (!errorOccurred)
                {
                    model.ResultText = "Import was completed successfully";
                }
                else
                {
                    model.ResultText += "There was a problem with the import";
                }
            }
            else
            {
                model.ResultText = NotAllowedMessage;
            }

            model.PageTitle = PageTitle;
            return View(model);
        }

        #endregion Actions

        #region Non Action Methods

        private static bool IsUserAdministrator()
        {
            var userAdminRole = ClaimsManager.GetCurrentIdentity().Roles.FirstOrDefault(x => x.Name == "Administrators");
            return userAdminRole != null;
        }

        private async Task<IEnumerable<BauJobProfile>> GetAllJobProfilesBySourcePropertiesAsync()
        {
            return await bauJobProfileRepository.GetAllJobProfilesBySourcePropertiesAsync();
        }

        private Dictionary<string, string> GetPropertyMappingDictionary()
        {
            var dictList = new Dictionary<string, string>();
            var mappings = SourceToDestinationPropertyMapping.Split(',');
            foreach (var mapping in mappings)
            {
                var keyValue = mapping.Split(':');

                if (keyValue.Length == 2)
                {
                    dictList.Add(keyValue[0], keyValue[1]);
                }
            }

            return dictList;
        }
        #endregion Non Action Methods
    }
}