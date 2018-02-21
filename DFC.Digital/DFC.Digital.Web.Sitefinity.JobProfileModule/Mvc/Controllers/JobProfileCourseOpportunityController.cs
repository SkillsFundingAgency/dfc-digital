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
    [ControllerToolboxItem(Name = "JobProfileCourseOpportunities", Title = "JobProfile Course Opportunities", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileCourseOpportunityController : BaseJobProfileWidgetController
    {
        #region Fields

        /// <summary>
        /// The course search service
        /// </summary>
        private readonly ICourseSearchService courseSearchService;
        private readonly IAsyncHelper asyncHelper;

        #endregion Fields

        public JobProfileCourseOpportunityController(
            ICourseSearchService courseSearchService,
            IAsyncHelper asyncHelper,
            IWebAppContext webAppContext,
            IJobProfileRepository jobProfileRepository,
            IApplicationLogger loggingService,
            ISitefinityPage sitefinityPage)
            : base(webAppContext, jobProfileRepository, loggingService, sitefinityPage)
        {
            this.courseSearchService = courseSearchService;
            this.asyncHelper = asyncHelper;
        }

        #region Web Properties

        /// <summary>
        /// Gets or sets the courses section title.
        /// </summary>
        /// <value>
        /// The courses section title.
        /// </value>
        public string CoursesSectionTitle { get; set; } = "Training courses";

        /// <summary>
        /// Gets or sets the training courses location details.
        /// </summary>
        /// <value>
        /// The training courses location details.
        /// </value>
        public string TrainingCoursesLocationDetails { get; set; } = "In England";

        /// <summary>
        /// Gets or sets the find training courses text.
        /// </summary>
        /// <value>
        /// The find training courses text.
        /// </value>
        public string FindTrainingCoursesText { get; set; } = "Find courses near you";

        /// <summary>
        /// Gets or sets the no training courses text.
        /// </summary>
        /// <value>
        /// The no training courses text.
        /// </value>
        public string NoTrainingCoursesText { get; set; } = "There are no courses for {jobtitle} available at the moment";

        /// <summary>
        /// Gets or sets the find training courses link.
        /// </summary>
        /// <value>
        /// The find training courses link.
        /// </value>
        public string FindTrainingCoursesLink { get; set; } = "https://nationalcareersservice.direct.gov.uk/course-directory/home";

        /// <summary>
        /// Gets or sets the maximum training courses maximum count.
        /// </summary>
        /// <value>
        /// The maximum training courses maximum count.
        /// </value>
        public int MaxTrainingCoursesMaxCount { get; set; } = 2;

        /// <summary>
        /// Gets or sets the course link.
        /// </summary>
        /// <value>
        /// The course link.
        /// </value>
        public string CourseLink { get; set; } = "https://sit.nationalcareersservice.org.uk/course-directory/course-details?courseid=";

        /// <summary>
        /// Gets or sets the main section title.
        /// </summary>
        /// <value>
        /// The main section title.
        /// </value>
        public string MainSectionTitle { get; set; }

        /// <summary>
        /// Gets or sets the section identifier.
        /// </summary>
        /// <value>
        /// The section identifier.
        /// </value>
        public string SectionId { get; set; } = "current-opportunities";

        #endregion Web Properties

        #region Actions

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [RelativeRoute("")]
        public ActionResult Index()
        {
            return BaseIndex();
        }

        /// <summary>
        /// Indexes the specified urlname.
        /// </summary>
        /// <param name="urlname">The urlname.</param>
        /// <returns>ActionResult</returns>
        [HttpGet]
        [RelativeRoute("{urlname}")]
        public ActionResult Index(string urlname)
        {
            return BaseIndex(urlname);
        }

        #endregion Actions

        protected override ActionResult GetDefaultView()
        {
            IEnumerable<Course> trainingCourses = new List<Course>();
            if (!string.IsNullOrEmpty(CurrentJobProfile.CourseKeywords))
            {
                trainingCourses = asyncHelper.Synchronise(() => courseSearchService.GetCoursesAsync(CurrentJobProfile.CourseKeywords))?.Take(MaxTrainingCoursesMaxCount);
            }

            var model = new JobProfileCourseSearchViewModel
            {
                CoursesSectionTitle = CoursesSectionTitle,
                NoTrainingCoursesText = NoTrainingCoursesText.Replace("{jobtitle}", CurrentJobProfile.Title),
                FindTrainingCoursesLink = FindTrainingCoursesLink,
                FindTrainingCoursesText = FindTrainingCoursesText,
                CoursesLocationDetails = TrainingCoursesLocationDetails,
                CourseLink = CourseLink,
                Courses = trainingCourses,
                MainSectionTitle = MainSectionTitle
            };

            return View("Index", model);
        }

        protected override ActionResult GetEditorView()
        {
            if (CurrentJobProfile == null)
            {
                var model = new JobProfileCourseSearchViewModel
                {
                    CoursesSectionTitle = CoursesSectionTitle,
                    NoTrainingCoursesText = NoTrainingCoursesText,
                    FindTrainingCoursesLink = FindTrainingCoursesLink,
                    FindTrainingCoursesText = FindTrainingCoursesText,
                    CoursesLocationDetails = TrainingCoursesLocationDetails,
                    CourseLink = CourseLink,
                    MainSectionTitle = MainSectionTitle
                };

                return View("Index", model);
            }

            return GetDefaultView();
        }
    }
}