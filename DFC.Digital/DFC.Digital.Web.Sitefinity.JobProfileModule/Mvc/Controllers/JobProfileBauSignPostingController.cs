using DFC.Digital.Data.Interfaces;
using DFC.Digital.Web.Sitefinity.Core.Interface;
using DFC.Digital.Web.Sitefinity.Core.Utility;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using System.ComponentModel;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "JobProfileBauSignPosting", Title = "Job Profile Bau JP SignPosting", SectionName = SitefinityConstants.CustomWidgetSection)]
    public class JobProfileBAUSignpostingController : BaseJobProfileWidgetController
    {
        #region Private Fields
        #endregion Private Fields

        #region Constructors

        public JobProfileBAUSignpostingController(IWebAppContext webAppContext, IJobProfileRepository jobProfileRepository, IApplicationLogger applicationLogger, ISitefinityPage sitefinityPage)
            : base(webAppContext, jobProfileRepository, applicationLogger, sitefinityPage)
        {
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Matching Job Profile exists in BAU text")]
        public string MatchingJobProfileInBAUText { get; set; } = "<a class='signpost signpost_jp' href =\"https://nationalcareersservice.direct.gov.uk/job-profiles/REPLACEWITHJPURL\"><p class='signpost_arrow'><span>Back to the National Careers Service</span> where you'll find all the job profiles</p></a>";

        [DisplayName("Matching Job Profile does not exists in BAU text")]
        public string NoMatchingJobProfileInBAUText { get; set; } = "<a class='signpost signpost_jp' href =\"https://nationalcareersservice.direct.gov.uk/job-profiles/home\"><p class='signpost_arrow'><span>Back to the National Careers Service</span> where you'll find all the job profiles</p></a>";

        #endregion Public Properties

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
            return View("Index", PopulateSignPosting());
        }

        protected override ActionResult GetEditorView()
        {
            return View("Index", PopulateSignPosting());
        }

        private BAUJobProfileSignpostViewModel PopulateSignPosting()
        {
            var jpexists = MatchingJobProfileInBAUText.Replace("REPLACEWITHJPURL", string.IsNullOrWhiteSpace(CurrentJobProfile.BAUSystemOverrideUrl) ? CurrentJobProfile.UrlName : CurrentJobProfile.BAUSystemOverrideUrl);
            return new BAUJobProfileSignpostViewModel
            {
                SignpostingHtml = CurrentJobProfile.DoesNotExistInBAU == true ? NoMatchingJobProfileInBAUText : jpexists
            };
        }

        #endregion Actions
    }
}