﻿using DFC.Digital.Core;
using DFC.Digital.Web.Core;
using DFC.Digital.Web.Sitefinity.Core;
using DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Models;
using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.CourseModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "CourseLanding", Title = "Courses Landing", SectionName = SitefinityConstants.CustomCoursesSection)]
    public class CourseLandingController : BaseDfcController
    {
        #region private Fields
        private readonly ICourseSearchConverter courseSearchConverter;
        #endregion

        #region Ctor
        public CourseLandingController(IApplicationLogger loggingService, ICourseSearchConverter courseSearchConverter) : base(loggingService)
        {
            this.courseSearchConverter = courseSearchConverter;
        }
        #endregion

        #region Public Properties
        [DisplayName("Course Name Hint Text")]
        public string CourseNameHintText { get; set; } = "For example, Maths or Sports Studies";

        [DisplayName("Course Name Label")]
        public string CourseNameLabel { get; set; } = "Course name";

        [DisplayName("Provider Label")]
        public string ProviderLabel { get; set; } = "Provider name (optional)";

        [DisplayName("Provider Hint Text")]
        public string ProviderHintText { get; set; } = "For example, Sheffield College.";

        [DisplayName("Location Label")]
        public string LocationLabel { get; set; } = "Location (optional)";

        [DisplayName("Location Hint Text")]
        public string LocationHintText { get; set; } = "Enter a full postcode. For example, S1 1WB";

        [DisplayName("Qualification Level Hint")]
        public string QualificationLevelHint { get; set; } = "What qualification levels mean";

        [DisplayName("Qualification Level Label")]
        public string QualificationLevelLabel { get; set; } = "Qualification level (optional)";

        [DisplayName("Training Courses Results Page")]
        public string CourseSearchResultsPage { get; set; } = "/course-directory/course-search-result";

        [DisplayName("Location Post Code Regex")]
        public string LocationRegex { get; set; } = @"([Gg][Ii][Rr] 0[Aa]{2})|((([A-Za-z][0-9]{1,2})|(([A-Za-z][A-Ha-hJ-Yj-y][0-9]{1,2})|(([A-Za-z][0-9][A-Za-z])|([A-Za-z][A-Ha-hJ-Yj-y][0-9][A-Za-z]?))))\s?[0-9][A-Za-z]{2})";

        [DisplayName("Dfe 1619 Funded Text")]
        public string Dfe1619FundedText { get; set; } = "Only show courses suitable for 16-19 year olds";
        #endregion

        #region Action
        [HttpGet]
        public ActionResult Index()
        {
            var model = new CourseLandingViewModel();
            AddWidgetProperties(model);
            return View("Index", model);
        }

        [HttpPost]
        public ActionResult Index(CourseLandingViewModel model)
        {
            model.StrippedSearchTerm = ReplaceSpecialCharactersExtension.ReplaceSpecialCharacter(model.SearchTerm);
            var redirectUrl = courseSearchConverter.BuildSearchRedirectPathAndQueryString(CourseSearchResultsPage, model, LocationRegex);
            return Redirect(redirectUrl);
        }
        #endregion

        private void AddWidgetProperties(CourseLandingViewModel model)
        {
            model.CourseNameHintText = CourseNameHintText;
            model.CourseNameLabel = CourseNameLabel;
            model.LocationLabel = LocationLabel;
            model.ProviderLabel = ProviderLabel;
            model.QualificationLevelHint = QualificationLevelHint;
            model.QualificationLevelLabel = QualificationLevelLabel;
            model.ProviderNameHintText = ProviderHintText;
            model.LocationHintText = LocationHintText;
            model.Dfe1619FundedText = Dfe1619FundedText;
            model.LocationRegex = LocationRegex;
        }
    }
}