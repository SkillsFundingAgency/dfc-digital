using DFC.Digital.AcceptanceTest.Infrastructure;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace DFC.Digital.AcceptanceTest.AcceptanceCriteria.Steps
{
    [Binding]
    public class JobProfileSteps : BaseStep
    {
        #region Fields

        //private JobProfilePage jobProfilePage;
        //private HomePage homePage;
        #endregion Fields

        #region Ctor

        public JobProfileSteps(BrowserStackSelenoHost browserStackSelenoHost, ScenarioContext scenarioContext) : base(browserStackSelenoHost, scenarioContext)
        {
        }

        #endregion Ctor

        #region Givens

        [Given(@"that I am viewing the '(.*)' job profile page")]
        public void GivenThatIAmViewingTheJobProfilePage(string jobProfileUrl)
        {
            var visitedPage = NavigateToJobProfilePage<JobProfilePage, JobProfileDetailsViewModel>(jobProfileUrl);

            ScenarioContext.Set(visitedPage.ProfilePageHeading, "visitedPageHeading");
            ScenarioContext.Set(jobProfileUrl, "profileUrl");
        }

        #endregion Givens

        #region Whens

        [When(@"I click the '(.*)' link")]
        public void WhenIClickTheLink(string link)
        {
            GetNavigatedPage<JobProfilePage>().ClickUsefulLink(link);
        }

        [When(@"I click on the Back To Homepage link")]
        public void WhenIClickOnTheBackToHomepageLink()
        {
            GetNavigatedPage<JobProfilePage>().ClickBackToHomepageLink<Homepage>().SaveTo(ScenarioContext);
        }

        [When(@"I click the Explore careers link")]
        public void WhenIClickTheExploreCareersLink()
        {
            GetNavigatedPage<JobProfilePage>().ClickExploreCareersLink<Homepage>().SaveTo(ScenarioContext);
        }

        [When(@"I search using '(.*)' on the profile page")]
        public void WhenISearchUsingOnTheProfilePage(string searchTerm)
        {
            GetNavigatedPage<JobProfilePage>()
                .Search<SearchPage>(new JobProfileDetailsViewModel
                {
                    SearchTerm = searchTerm
                })
                .SaveTo(ScenarioContext);
        }

        [When(@"I click on career title '(.*)'")]
        public void WhenIClickOnCareerTitle(int titleNo)
        {
            var jobProfile = GetNavigatedPage<JobProfilePage>();
            ScenarioContext.Set(jobProfile.RelatedCareersTitle(titleNo), "RelatedCareerTitle");
            jobProfile.ClickRelatedCareer<JobProfilePage>(titleNo)
                .SaveTo(ScenarioContext);
        }

        [When(@"I click Find courses near you")]
        public void WhenIClickFindcoursesnearyou()
        {
            var profilePage = GetNavigatedPage<JobProfilePage>();
            profilePage.ClickFindCourseLink<CourseDirectoryPage>().SaveTo(ScenarioContext);
        }

        [When(@"I click on the training course title no '(.*)'")]
        public void WhenIClickOnTheTrainingCourseTitleNo(int courseNumber)
        {
            var profilePage = GetNavigatedPage<JobProfilePage>();
            ScenarioContext.Set(profilePage.GetCourseTitle(courseNumber), "courseTitle");
            profilePage.ClickCourseTitle<CourseDirectoryPage>(courseNumber).SaveTo(ScenarioContext);
        }

        [When(@"I click to go back")]
        public void WhenIClickToGoBack()
        {
            PressBack();
        }

        [When(@"I click on the '(.*)' page banner link")]
        public void WhenIClickOnThePageBannerLink(string page)
        {
            switch (page?.ToLower())
            {
                case "profile":
                    var profilePage = GetNavigatedPage<JobProfilePage>();
                    profilePage.ClickProfilePageBanner<BauProfilePage>()
                        .SaveTo(ScenarioContext);
                    break;
                case "category":
                    var categoryPage = GetNavigatedPage<JobProfileCategoryPage>();
                    categoryPage.ClickCategorySignpostBanner<BauJpLandingPage>()
                        .SaveTo(ScenarioContext);
                    break;
                case "search":
                    var searchPage = GetNavigatedPage<SearchPage>();
                    searchPage.ClickSearchSignpostBanner<BauSearchPage>()
                        .SaveTo(ScenarioContext);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Thens

        [Then(@"the current opportunites section is shown on the page")]
        public void ThenTheCurrentOpportunitesSectionIsShownOnThePage()
        {
            var jobProfilePage = GetNavigatedPage<JobProfilePage>();
            jobProfilePage.HasCurrentOpportunitiesSection.Should().BeTrue();
        }

        [Then(@"the count of vacancies is '(.*)'")]
        public void ThenTheCountOfVacanciesIs(int countOfVacancies)
        {
            var jobProfilePage = GetNavigatedPage<JobProfilePage>();
            jobProfilePage.VacancyCount.Should().Be(countOfVacancies);
        }

        [Then(@"the No Vacancies text is '(.*)'")]
        public void ThenTheNoVacanciesTextIs(bool noVacanciesTextShown)
        {
            var jobProfilePage = GetNavigatedPage<JobProfilePage>();
            jobProfilePage.HasNoVacancyText.Should().Be(noVacanciesTextShown);
        }

        [Then(@"all apprenticeship vacancies have a hyperlink is '(.*)'")]
        public void ThenAllApprenticeshipVacanciesHaveAHyperlinkIs(bool allVacanciesHaveHyperlink)
        {
            var jobProfilePage = GetNavigatedPage<JobProfilePage>();
            jobProfilePage.AllVacanciesHaveHyperlinks.Should().Be(allVacanciesHaveHyperlink);
        }

        [Then(@"the find apprenticeship near you has a '(.*)' displayed")]
        public void ThenTheFindApprenticeshipNearYouHasADisplayed(bool findApprenticeLinkShown)
        {
            var jobProfilePage = GetNavigatedPage<JobProfilePage>();
            jobProfilePage.HasValidFindApprenticeshipLink.Should().Be(findApprenticeLinkShown);
        }

        [Then(@"the Useful Links section is displayed on the page")]
        public void ThenTheUsefulLinksSectionIsDisplayedOnThePage()
        {
            var jobProfilePage = GetNavigatedPage<JobProfilePage>();
            jobProfilePage.HasUsefulLinksSection.Should().BeTrue();
        }

        [Then(@"I am redirected to the correct '(.*)' url")]
        public void ThenIAmRedirectedToTheCorrectPage(string url)
        {
            var browserUrl = CurrentBrowserUrl;
            browserUrl.OriginalString.Should().Contain(url);
            PressBack();
        }

        [Then(@"'(\d+)' apprenticeships should be displayed with valid data")]
        public void ThenApprenticeshipsShouldBeDisplayed(int noOfApprenticeships)
        {
            var jobProfilePage = GetNavigatedPage<JobProfilePage>();
            jobProfilePage.VacancyCount.Should().Be(noOfApprenticeships);
        }

        [Then(@"the correct sections should be displayed")]
        public void ThenTheCorrectSectionsShouldBeDisplayed()
        {
            var jobProfilePage = GetNavigatedPage<JobProfilePage>();

            jobProfilePage.HasHowToBecomeSection.Should().BeTrue();
            jobProfilePage.HasSkillsSection.Should().BeTrue();
            jobProfilePage.HasWhatYouWillDoSection.Should().BeTrue();
            jobProfilePage.HasCareerPathSection.Should().BeTrue();
            jobProfilePage.HasCurrentOpportunitiesSection.Should().BeTrue();
        }

        [Then(@"the no apprenticeships message should be shown")]
        public void ThenTheNoApprenticeshipsMessageShouldBeShown()
        {
            string noVacancyText = "We can't find any apprenticeship " +
                "vacancies in England for";

            var jobProfilePage = GetNavigatedPage<JobProfilePage>();
            jobProfilePage.HasNoVacancyText.Should().BeTrue();
            jobProfilePage.NoVacancyText.Should().Contain(noVacancyText);
        }

        [Then(@"I am redirected to the 404 page")]
        public void ThenIAmRedirectedToThePage()
        {
            var jobProfilePage = GetNavigatedPage<JobProfilePage>();
            jobProfilePage.ProfilePageHeading.Should().Contain("This page is not available");
        }

        [Then(@"the search section should be displayed")]
        public void ThenTheSearchSectionShouldBeDisplayed()
        {
            var jobProfilePage = GetNavigatedPage<JobProfilePage>();
            jobProfilePage.HasJobProfileSearch.Should().BeTrue();
        }

        [Then(@"the Related Careers section is displayed on the page")]
        public void ThenTheRelatedCareersSectionIsDisplayedOnThePage()
        {
            var jobProfile = GetNavigatedPage<JobProfilePage>();
            jobProfile.HasRelatedCareersSection.Should().BeTrue();
        }

        [Then(@"there should be no more than (.*) careers")]
        public void ThenThereShouldBeNoMoreThanCareers(int number)
        {
            var jobProfile = GetNavigatedPage<JobProfilePage>();
            jobProfile.NumberOfRelatedCareers.Should().BeLessOrEqualTo(number);
        }

        [Then(@"the Related Careers section is not displayed on the page")]
        public void ThenTheRelatedCareersSectionIsNotDisplayedOnThePage()
        {
            var jobProfile = GetNavigatedPage<JobProfilePage>();
            jobProfile.HasRelatedCareersSection.Should().BeFalse();
        }

        [Then(@"I am redirected to the correct job profile page")]
        public void ThenIAmRedirectedToTheCorrectJobProfilePage()
        {
            string titleText;
            ScenarioContext.TryGetValue("RelatedCareerTitle", out titleText);

            if (titleText == null)
            {
                ScenarioContext.TryGetValue("profileSelected", out titleText);
            }

            var jobProfile = GetNavigatedPage<JobProfilePage>();
            jobProfile.ProfilePageHeading.Should().Contain(titleText);
        }

        [Then(@"take me to the training course page on Course Directory")]
        public void ThenTakeMeToTheTrainingCoursePageOnCourseDirectory()
        {
            var courseDirectoryPage = GetNavigatedPage<CourseDirectoryPage>();
            var coursetitle = string.Empty;

            ScenarioContext.TryGetValue("courseTitle", out coursetitle);

            courseDirectoryPage.Heading.Should().Contain(coursetitle);
        }

        [Then(@"take me back to the job profile I had been viewing")]
        public void ThenTakeMeBackToTheJobProfileIHadBeenViewing()
        {
            var profilePage = GetNavigatedPage<JobProfilePage>();
            var coursetitle = string.Empty;

            var visitedPageHeading = string.Empty;
            ScenarioContext.TryGetValue("visitedPageHeading", out visitedPageHeading);
            ScenarioContext.TryGetValue("courseTitle", out coursetitle);
            profilePage.ProfilePageHeading.Should().Contain(visitedPageHeading);
        }

        [Then(@"take me to the search page on Course Directory")]
        public void ThenTakeMeToTheSearchPageOnCourseDirectory()
        {
            var courseDirectoryPage = GetNavigatedPage<CourseDirectoryPage>();
            courseDirectoryPage.Heading.Should().Contain("Find a course");
        }

        [Then(@"the '(.*)' page signpost banner is displayed")]
        public void ThenTheSignpostBannerIsDisplayed(string page)
        {
            switch (page?.ToLower())
            {
                case "profile":
                    var profilePage = GetNavigatedPage<JobProfilePage>();
                    profilePage.HasSignpostBanner.Should().BeTrue();
                    break;
                case "category":
                    var categoryPage = GetNavigatedPage<JobProfileCategoryPage>();
                    categoryPage.HasSignpostBanner.Should().BeTrue();
                    break;
                case "search":
                    var searchPage = GetNavigatedPage<SearchPage>();
                    searchPage.HasSignPostBanner.Should().BeTrue();
                    break;
                default:
                    break;
            }
        }

        [Then(@"I am redirected to corresponding '(.*)' profile page")]
        public void ThenIAmRedirectedToCorrespondingBauProfilePage(string env)
        {
            string visitedUrl;
            ScenarioContext.TryGetValue("profileUrl", out visitedUrl);
            switch (env?.ToLower())
            {
                case "bau":
                    var bauProfilePage = GetNavigatedPage<BauProfilePage>();
                    bauProfilePage.UrlContains(visitedUrl);
                    break;
                case "beta":
                    var betaProfilePage = GetNavigatedPage<JobProfilePage>();
                    betaProfilePage.UrlContains(visitedUrl).Should().BeTrue();
                    break;
            }
        }
        #endregion
    }
}