using AutoMapper;
using DFC.Digital.Core;
using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    /// <summary>
    /// Custom Widget for Job Profile Anchor Links
    /// </summary>
    /// <seealso cref="DFC.Digital.Web.Core.BaseDfcController" />
    [ControllerToolboxItem(Name = "JobProfileAnchorLinks", Title = "JobProfile Anchor links", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileAnchorLinksController : BaseDfcController
    {
        /// <summary>
        /// The job profile section caption
        /// </summary>
        private const string Jobprofilesectioncaption = "Job Profile Section";

        /// <summary>
        /// The jobprofilehowto become ssection caption
        /// </summary>
        private const string JobprofilehowtoBecomeSectionCaption = "JobProfile HowToBecome";

        /// <summary>
        /// The jobprofilehowto become ssection caption
        /// </summary>
        private const string JobprofileWhatItTakesCaption = "Job Profile What It Takes";

        /// <summary>
        /// The jobprofile what you will do caption
        /// </summary>
        private const string JobprofileWhatYouWillDoCaption = "JobProfile What You Will Do";

        /// <summary>
        /// The job profile section title
        /// </summary>
        private const string Jobprofilesectiontitle = "Title";

        /// <summary>
        /// The job profile section property name
        /// </summary>
        private const string Jobprofilesectionpropertyname = "PropertyName";

        /// <summary>
        /// The job profile apprenticeship title
        /// </summary>
        private const string MainSectionTitle = "MainSectionTitle";

        /// <summary>
        /// The job profile apprenticeship section caption
        /// </summary>
        private const string Jobprofileopportunitiessectioncaption = "GDS JobProfile Opportunities section";

        /// <summary>
        /// The job profile apprenticeship anchor target
        /// </summary>
        private const string SectionId = "SectionId";

        private const string Jobprofileapprenticeships = "JobProfile Apprenticeships";

        private const string Jobprofilecourses = "JobProfile Course Opportunities";

        #region Private Fields

        /// <summary>
        /// The web application context
        /// </summary>
        private readonly IWebAppContext webAppContext;

        /// <summary>
        /// The page content service
        /// </summary>
        private readonly IJobProfilePage pageContentService;

        private readonly IMapper mapper;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JobProfileAnchorLinksController"/> class.
        /// </summary>
        /// <param name="webAppContext">The web application context.</param>
        /// <param name="applicationLogger">The application logger.</param>
        /// <param name="pageContentService">The page content service.</param>
        /// <param name="mapper">mapper</param>
        public JobProfileAnchorLinksController(IWebAppContext webAppContext, IApplicationLogger applicationLogger, IJobProfilePage pageContentService, IMapper mapper) : base(applicationLogger)
        {
            this.webAppContext = webAppContext;
            this.pageContentService = pageContentService;
            this.mapper = mapper;
        }

        #endregion Constructors

        #region Actions

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>Action Result</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            if (webAppContext.IsContentAuthoringSite)
            {
                return GetJobProfileAnchorLinks();
            }

            return Redirect("\\");
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
            var unused = urlName;
            return GetJobProfileAnchorLinks();
        }

        /// <summary>
        /// Gets the job profile anchor links.
        /// </summary>
        /// <returns>Action Result</returns>
        private ActionResult GetJobProfileAnchorLinks()
        {
            var itemList = pageContentService.GetJobProfileSections(
                new[]
                {
                    new JobProfileSectionFilter
                    {
                        SectionCaption = Jobprofilesectioncaption,
                        ContentFieldMember = Jobprofilesectionpropertyname,
                        TitleMember = Jobprofilesectiontitle
                    },
                    new JobProfileSectionFilter
                    {
                        SectionCaption = JobprofilehowtoBecomeSectionCaption,
                        ContentFieldMember = SectionId,
                        TitleMember = MainSectionTitle
                    },
                    new JobProfileSectionFilter
                    {
                        SectionCaption = JobprofileWhatItTakesCaption,
                        ContentFieldMember = SectionId,
                        TitleMember = MainSectionTitle
                    },
                    new JobProfileSectionFilter
                    {
                        SectionCaption = JobprofileWhatYouWillDoCaption,
                        ContentFieldMember = SectionId,
                        TitleMember = MainSectionTitle
                    },
                    new JobProfileSectionFilter
                    {
                        SectionCaption = Jobprofileopportunitiessectioncaption,
                        ContentFieldMember = SectionId,
                        TitleMember = MainSectionTitle,
                        SubFilters = new List<string> { Jobprofileapprenticeships, Jobprofilecourses }
                    },
                });

            return View("Index", new JobProfileAnchorLinksViewModel { AnchorLinks = mapper.Map<IEnumerable<AnchorLink>>(itemList) });
        }

        #endregion Actions
    }
}