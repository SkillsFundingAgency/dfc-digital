using DFC.Digital.Core;
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
    public class JobProfileBauSignPostingController : BaseJobProfileWidgetController
    {
        #region Constructors

        public JobProfileBauSignPostingController(IWebAppContext webAppContext, IJobProfileRepository jobProfileRepository, IApplicationLogger applicationLogger, ISitefinityPage sitefinityPage)
            : base(webAppContext, jobProfileRepository, applicationLogger, sitefinityPage)
        {
        }

        #endregion Constructors

        #region Public Properties

        [DisplayName("Matching Job Profile exists in BAU text")]
        public string MatchingJpinBauText { get; set; } = "<a class='signpost signpost_jp' href =\"https://nationalcareersservice.direct.gov.uk/job-profiles/REPLACEWITHJPURL\"><p class='signpost_arrow'><span>Back to the National Careers Service</span> where you'll find all the job profiles</p></a>";

        [DisplayName("Matching Job Profile does not exists in BAU text")]
        public string NoMatchingJpinBauText { get; set; } = "<a class='signpost signpost_jp' href =\"https://nationalcareersservice.direct.gov.uk/job-profiles/home\"><p class='signpost_arrow'><span>Back to the National Careers Service</span> where you'll find all the job profiles</p></a>";

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
        /// <param name="urlname">The urlname.</param>
        /// <returns>Action Result</returns>
        [HttpGet]
        [RelativeRoute("{urlname}")]
        public ActionResult Index(string urlname)
        {
            return BaseIndex(urlname);
        }

        protected override ActionResult GetDefaultView()
        {
            return View("Index", PopulateSignPosting());
        }

        protected override ActionResult GetEditorView()
        {
            return View("Index", PopulateSignPosting());
        }

        private BauJpSignPostViewModel PopulateSignPosting()
        {
            var jpexists = MatchingJpinBauText.Replace("REPLACEWITHJPURL", string.IsNullOrWhiteSpace(CurrentJobProfile.BAUSystemOverrideUrl) ? CurrentJobProfile.UrlName : CurrentJobProfile.BAUSystemOverrideUrl);
            return new BauJpSignPostViewModel
            {
                SignPostingHtml = CurrentJobProfile.DoesNotExistInBAU == true ? NoMatchingJpinBauText : jpexists
            };
        }

        #endregion Actions
    }
}