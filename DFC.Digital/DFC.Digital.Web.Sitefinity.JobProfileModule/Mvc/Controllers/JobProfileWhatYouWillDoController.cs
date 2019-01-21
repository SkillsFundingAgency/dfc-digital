using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    /// <summary>
    /// Job Profile Section Controller
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "JobProfileWhatYouWillDo", Title = "JobProfile What You Will Do", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileWhatYouWillDoController : BaseJobProfileWidgetController
    {
        private readonly IFormatContentService formatContentService;

        #region Ctor

        public JobProfileWhatYouWillDoController(IJobProfileRepository jobProfileRepository, IWebAppContext webAppContext, IApplicationLogger applicationLogger, ISitefinityPage sitefinityPage, IFormatContentService formatContentService)
             : base(webAppContext, jobProfileRepository, applicationLogger, sitefinityPage)
        {
            this.formatContentService = formatContentService;
        }

        #endregion Ctor

        #region public Properties

        /// <summary>
        /// Gets or sets the content of the top section.
        /// </summary>
        /// <value>
        /// The content of the top section.
        /// </value>
        public string TopSectionContent { get; set; }

        /// <summary>
        /// Gets or sets the content of the bottom section.
        /// </summary>
        /// <value>
        /// The content of the bottom section.
        /// </value>
        public string BottomSectionContent { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string MainSectionTitle { get; set; } = "What you'll do";

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string SectionId { get; set; } = "WhatYouWillDo";

        /// <summary>
        /// Gets or sets the what it takes section title.
        /// </summary>
        /// <value>
        /// The what it takes section title.
        /// </value>
        public string WhatYouWillDoSectionTitle { get; set; } = "Day-to-day tasks";

        /// <summary>
        /// Gets or sets the environment title.
        /// </summary>
        /// <value>
        /// The environment title.
        /// </value>
        public string EnvironmentTitle { get; set; } = "Working environment";

        /// <summary>
        /// Gets or sets the environments leading text.
        /// </summary>
        /// <value>
        /// The environments leading text.
        /// </value>
        public string EnvironmentsLeadingText { get; set; } = "Your working environment may be";

        /// <summary>
        /// Gets or sets the uniform leading text.
        /// </summary>
        /// <value>
        /// The uniform leading text.
        /// </value>
        public string UniformLeadingText { get; set; } = "You may need to wear";

        /// <summary>
        /// Gets or sets the location leading text.
        /// </summary>
        /// <value>
        /// The location leading text.
        /// </value>
        public string LocationLeadingText { get; set; } = "You could work";

        /// <summary>
        /// Gets or sets the environments conjunction.
        /// </summary>
        /// <value>
        /// The environments conjunction.
        /// </value>
        public string EnvironmentsConjunction { get; set; } = "and";

        /// <summary>
        /// Gets or sets the uniform conjunction.
        /// </summary>
        /// <value>
        /// The uniform conjunction.
        /// </value>
        public string UniformConjunction { get; set; } = "and";

        /// <summary>
        /// Gets or sets the location conjunction.
        /// </summary>
        /// <value>
        /// The location conjunction.
        /// </value>
        public string LocationConjunction { get; set; } = "or";

        /// <summary>
        /// Gets or sets a value indicating whether this instance is wyd intro active.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is wyd intro active; otherwise, <c>false</c>.
        /// </value>
        public bool IsWYDIntroActive { get; set; }

        #endregion private Properties

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
            return ReturnSectionView();
        }

        protected override ActionResult GetEditorView()
        {
            return ReturnSectionView();
        }

        /// <summary>
        /// Returns the section view.
        /// </summary>
        /// <returns>Action Result</returns>
        private ActionResult ReturnSectionView()
        {
            var model = new JobProfileWhatYouWillDoViewModel();
            if (CurrentJobProfile != null)
            {
                 model = new JobProfileWhatYouWillDoViewModel
                {
                    TopSectionContent = TopSectionContent,
                    BottomSectionContent = BottomSectionContent,
                    PropertyValue = CurrentJobProfile.WhatYouWillDo,
                    Title = MainSectionTitle,
                    SectionId = SectionId,
                    IsIntroActive = IsWYDIntroActive,
                    DailyTasksSectionTitle = WhatYouWillDoSectionTitle,
                    EnvironmentTitle = EnvironmentTitle,
                    IsWhatYouWillDoCadView = CurrentJobProfile.WhatYouWillDoData.IsCadReady
                };

                if (model.IsWhatYouWillDoCadView)
                {
                    model.Location = formatContentService.GetParagraphText(LocationLeadingText, CurrentJobProfile.WhatYouWillDoData.Locations, LocationConjunction);
                    model.Uniform = formatContentService.GetParagraphText(UniformLeadingText, CurrentJobProfile.WhatYouWillDoData.Uniforms, UniformConjunction);
                    model.Environment = formatContentService.GetParagraphText(EnvironmentsLeadingText, CurrentJobProfile.WhatYouWillDoData.Environments, EnvironmentsConjunction);
                    model.Introduction = CurrentJobProfile.WhatYouWillDoData.Introduction;
                    model.DailyTasks = CurrentJobProfile.WhatYouWillDoData.DailyTasks;
                }
            }

            return View(model);
        }

        #endregion Actions
    }
}