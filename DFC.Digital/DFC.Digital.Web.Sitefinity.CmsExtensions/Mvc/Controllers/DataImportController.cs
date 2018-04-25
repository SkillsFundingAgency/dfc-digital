using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.CmsExtensions.Mvc.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.CmsExtensions.Mvc.Controllers
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
        private readonly IWebAppContext webAppContext;

        #endregion

        #region Constructors

        public DataImportController(IWebAppContext webAppContext, IApplicationLogger applicationLogger, IBauJobProfileOdataRepository bauJobProfileRepository, IManageBauJobProfilesService manageBauJobProfilesService, IJobProfileRepository jobProfileRepository, IAsyncHelper asyncHelper)
            : base(applicationLogger)
        {
            this.bauJobProfileRepository = bauJobProfileRepository;
            this.manageBauJobProfilesService = manageBauJobProfilesService;
            this.jobProfileRepository = jobProfileRepository;
            this.asyncHelper = asyncHelper;
            this.webAppContext = webAppContext;
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
        public string PageTitle { get; set; } = "BAU to BETA JobProfiles Data Import";

        /// <summary>
        /// Gets or sets the instructions text.
        /// </summary>
        /// <value>
        /// The instructions text.
        /// </value>
        [DisplayName("Instructions Text")]
        public string InstructionsText { get; set; } = "Please upload a data source for job profiles you have marked";

        /// <summary>
        /// Gets or sets the Change Comment.
        /// </summary>
        /// <value>
        /// The Change Comment.
        /// </value>
        [DisplayName("Change Comment")]
        public string ChangeComment { get; set; } = "Item was bulk imported";

        /// <summary>
        /// Gets or sets the source to destination property mapping.
        /// </summary>
        /// <value>
        /// The source to destination property mapping.
        /// </value>
        [DisplayName("Source-To-Destination Property Mapping")]
        public string SourceToDestinationPropertyMapping { get; set; }

        /// <summary>
        /// Gets or sets the Not Allowed Message.
        /// </summary>
        /// <value>
        /// The Not Allowed Message.
        /// </value>
        [DisplayName("Not Allowed Message")]
        public string NotAllowedMessage { get; set; } = "You are not allowed to use this functionality. Only Administrators are.";

        #endregion Public Properties

        #region Actions

        public ActionResult Index()
        {
            var model = new DataImportViewModel
            {
                PageTitle = PageTitle,
                NotAllowedMessage = NotAllowedMessage,
                InstructionsText = InstructionsText,
                IsAdmin = webAppContext.IsUserAdministrator,
                SourceToDestinationPropertyMapping = SourceToDestinationPropertyMapping,
                ChangeComment = ChangeComment
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Index(DataImportViewModel model, string importData, string importRelatedCareers)
        {
            if (webAppContext.IsUserAdministrator)
            {
                var markedJobProfiles = new List<JobProfileImporting>();

                // Get JP information (UrlName, CourseKeywords, etc.) from CSV file via widget property
                var csvreader = new StreamReader(model?.JobProfileImportDataFile.InputStream);
                while (!csvreader.EndOfStream)
                {
                    markedJobProfiles.Add(ConvertSingleLineToJobProfileImporting(csvreader.ReadLine()));
                }

                // Remove CSV Title row
                markedJobProfiles.Remove(markedJobProfiles.SingleOrDefault(a => a.UrlName.Contains("ItemDefaultUrl")));

                if (!string.IsNullOrEmpty(importData))
                {
                    var mappingDictionary = GetPropertyMappingDictionary(model.SourceToDestinationPropertyMapping);

                    // No point continuing if no mapping is defined
                    if (mappingDictionary.Any())
                    {
                        // Get JobProfiles from BAU WS Feed
                        var sourceJobProfiles = asyncHelper.Synchronise(() => GetAllJobProfilesBySourcePropertiesAsync(false));

                        // Select only required JobProfiles listed in csv from all BAU JP WS Feed
                        var selectedJobProfiles = manageBauJobProfilesService.SelectMarkedJobProfiles(sourceJobProfiles, markedJobProfiles);

                        bool errorOccurred = false;

                        // Process each selected JobProfile
                        foreach (var bauJobProfile in selectedJobProfiles)
                        {
                            try
                            {
                                string actionTaken = jobProfileRepository.AddOrUpdateJobProfileByProperties(bauJobProfile, mappingDictionary, model.ChangeComment, model.EnforcePublishing, model.DisableUpdate);
                                model.ResultText += bauJobProfile.Title + " ( " + bauJobProfile.UrlName + " ) - " + actionTaken + "<br />";
                            }
                            catch (Exception ex)
                            {
                                errorOccurred = true;
                                model.ResultText += bauJobProfile.Title + "<br />" + ex.Message + "<br />" + ex.InnerException + "<br />" + ex.StackTrace + "<br />";
                            }
                        }

                        if (!errorOccurred)
                        {
                            model.ResultText += "<br /><span style=\"font-weight:bold; \">Import was completed successfully</span>";
                        }
                        else
                        {
                            model.ResultText += "<br /><span style=\"font-weight:bold; color:red; \">There was a problem with the import</span>";
                        }
                    }
                    else
                    {
                        model.ResultText += "<br /><span style=\"font-weight:bold; color:red; \">Please, provide mapping for the import</span>";
                    }
                }

                if (!string.IsNullOrEmpty(importRelatedCareers))
                {
                    // Get JobProfiles from BAU WS Feed
                    var sourceJobProfiles = asyncHelper.Synchronise(() => GetAllJobProfilesBySourcePropertiesAsync(true));

                    // Select only required JobProfiles listed in csv from all BAU JP WS Feed
                    var selectedJobProfiles = manageBauJobProfilesService.SelectMarkedJobProfiles(sourceJobProfiles, markedJobProfiles);

                    bool errorOccurred = false;

                    // Process each selected JobProfile
                    foreach (var bauJobProfile in selectedJobProfiles)
                    {
                        try
                        {
                            string actionTaken = jobProfileRepository.UpdateRelatedCareers(bauJobProfile, model.ChangeComment, model.EnforcePublishing);
                            model.ResultText += bauJobProfile.Title + " ( " + bauJobProfile.UrlName + " ) - <br />" + actionTaken + "<br />";
                        }
                        catch (Exception ex)
                        {
                            errorOccurred = true;
                            model.ResultText += bauJobProfile.Title + "<br />" + ex.Message + "<br />" + ex.InnerException + "<br />" + ex.StackTrace + "<br />";
                        }
                    }

                    if (!errorOccurred)
                    {
                        model.ResultText += "<br /><span style=\"font-weight:bold; \">Update of RelatedCareers was completed successfully</span>";
                    }
                    else
                    {
                        model.ResultText += "<br /><span style=\"font-weight:bold; color:red; \">There was a problem with the Update of RelatedCareers</span>";
                    }
                }
            }
            else
            {
                model.ResultText = NotAllowedMessage;
            }

            return View(model);
        }

        #endregion Actions

        #region Non Action Methods

        private static Dictionary<string, string> GetPropertyMappingDictionary(string sourceToDestinationPropertyMapping)
        {
            var dictList = new Dictionary<string, string>();
            var mappings = sourceToDestinationPropertyMapping.Split(',');
            foreach (var mapping in mappings)
            {
                var keyValue = mapping.Split(':');

                if (keyValue.Length == 2)
                {
                    dictList.Add(keyValue[0].Trim(), keyValue[1].Trim());
                }
            }

            return dictList;
        }

        private static JobProfileImporting ConvertSingleLineToJobProfileImporting(string singleLine)
        {
            var jobProfileImporting = new JobProfileImporting();
            var singleLineParts = singleLine.Split(',');

            jobProfileImporting.UrlName = singleLineParts[0].Trim().TrimStart('/');
            jobProfileImporting.CourseKeywords = singleLineParts[1].Trim();

            return jobProfileImporting;
        }

        private async Task<IEnumerable<JobProfileImporting>> GetAllJobProfilesBySourcePropertiesAsync(bool includeRelatedCareers)
        {
            return await bauJobProfileRepository.GetAllJobProfilesBySourcePropertiesAsync(includeRelatedCareers);
        }

        #endregion Non Action Methods
    }
}