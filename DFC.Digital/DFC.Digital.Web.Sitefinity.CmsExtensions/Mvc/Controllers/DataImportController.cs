using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.CmsExtensions.Mvc.Models;
using DFC.Digital.Web.Sitefinity.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Web;
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
        private readonly IContentImportService<JobProfile> contentImport;
        private readonly IWebAppContext webAppContext;

        #endregion Private members

        #region Constructors

        //public DataImportController(IWebAppContext webAppContext, IApplicationLogger applicationLogger, IBauJobProfileOdataRepository bauJobProfileRepository, IManageBauJobProfilesService manageBauJobProfilesService, IJobProfileRepository jobProfileRepository, IAsyncHelper asyncHelper)
        public DataImportController(IApplicationLogger applicationLogger, IWebAppContext webAppContext, IAsyncHelper asyncHelper, IContentImportService<JobProfile> contentImport)
            : base(applicationLogger)
        {
            this.asyncHelper = asyncHelper;
            this.contentImport = contentImport;
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
        public ActionResult Index(DataImportViewModel model)
        {
            if (webAppContext.IsUserAdministrator)
            {
                var importConfig = new ImportConfiguration
                {
                    Content = ReadLines(model.JobProfileImportDataFile),
                    Mappings = ParseMapping(model.SourceToDestinationPropertyMapping),
                    CanUpdate = !model.DisableUpdate,
                    Comment = model.ChangeComment,
                    ShouldBePublished = model.EnforcePublishing
                };

                asyncHelper.Synchronise(() => contentImport.ImportAsync(importConfig));
            }
            else
            {
                model.ResultText = NotAllowedMessage;
            }

            return View(model);
        }

        #endregion Actions

        #region Non Action Methods

        private IEnumerable<string> ReadLines(HttpPostedFileBase input)
        {
            using (var csvreader = new StreamReader(input.InputStream))
            {
                while (!csvreader.EndOfStream)
                {
                    yield return csvreader.ReadLine();
                }
            }
        }

        private Dictionary<string, string> ParseMapping(string sourceToDestinationPropertyMapping)
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

        #endregion Non Action Methods
    }
}