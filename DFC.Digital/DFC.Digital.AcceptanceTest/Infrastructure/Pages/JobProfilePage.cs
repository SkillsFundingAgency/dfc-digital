using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using OpenQA.Selenium;
using System.Linq;
using TestStack.Seleno.PageObjects;

namespace DFC.Digital.AcceptanceTest.Infrastructure
{
    public class JobProfilePage : SitefinityPage<JobProfileDetailsViewModel>
    {
        public bool HasCurrentOpportunitiesSection => Find.OptionalElement(By.Id("current-opportunities")) != null;

        public bool HasRelatedCareersSection => Find.OptionalElement(By.ClassName("job-profile-related")) != null;

        public int NumberOfRelatedCareers => Find.Elements(By.CssSelector(".list-big li")).Count();

        public bool HasHowToBecomeSection => Find.OptionalElement(By.Id("HowToBecome")) != null;

        public bool HasSkillsSection => Find.OptionalElement(By.Id("Skills")) != null;

        public bool HasWhatYouWillDoSection => Find.OptionalElement(By.Id("WhatYouWillDo")) != null;

        public bool HasSalarySection => Find.OptionalElement(By.Id("Salary")) != null;

        public bool HasWorkingHoursSection => Find.OptionalElement(By.Id("WorkingHours")) != null;

        public bool HasWorkingHoursPatternsSection => Find.OptionalElement(By.Id("WorkingHoursPatterns")) != null;

        public bool HasCareerPathSection => Find.OptionalElement(By.Id("CareerPathAndProgression")) != null;

        public bool HasSignpostBanner => Find.OptionalElement(By.ClassName("signpost_jp")) != null;

        public bool HasNoVacancyText => Find.OptionalElement(By.ClassName("dfc-code-jp-novacancyText")) != null;

        public string NoVacancyText => Find.OptionalElement(By.ClassName("dfc-code-jp-novacancyText")).Text;

        public int VacancyCount => Find.Elements(By.CssSelector(".opportunity-item a")).Count();

        public bool AllVacanciesHaveHyperlinks => Find.Elements(By.CssSelector(".opportunity-item a")) != null && Find.Elements(By.CssSelector(".opportunity-item a")).Any() && !Find.Elements(By.CssSelector(".opportunity-item a")).Any(item => string.IsNullOrWhiteSpace(item.GetAttribute("href")));

        public bool HasJobProfileSearch => Find.OptionalElement(By.ClassName("job-profile-search")) != null;

        public bool HasValidFindApprenticeshipLink => Find.OptionalElement(By.ClassName("dfc-code-jp-findApprenticeship")) != null && !string.IsNullOrWhiteSpace(Find.OptionalElement(By.ClassName("dfc-code-jp-findApprenticeship")).GetAttribute("href"));

        public bool HasUsefulLinksSection => Find.OptionalElement(By.ClassName("govuk-related-items")) != null;

        public string ProfilePageHeading => Find.OptionalElement(By.ClassName("heading-xlarge")).Text;

        public string HomeCareersText => Find.Element(By.Id("proposition-name")).Text;

        public T ClickHomeCareersLink<T>()
            where T : UiComponent, new()
        {
            return Navigate.To<T>(By.Id("proposition-name"));
        }

        public string RelatedCareersTitle(int index)
        {
            var title = Find.Elements(By.CssSelector(".list-big li a")).ToList().ElementAt(index - 1).Text;
            return title;
        }

        public void ClickUsefulLink(string linkText)
        {
            var link = Find.OptionalElement(By.PartialLinkText(linkText));
            link.Click();
        }

        public T ClickBackToHomepageLink<T>()
            where T : UiComponent, new()
        {
            return Navigate.To<T>(By.PartialLinkText("Back to homepage"));
        }

        public T Search<T>(JobProfileDetailsViewModel model)
            where T : UiComponent, new()
        {
            Input.Model(model);
            return Navigate.To<T>(By.ClassName("submit"));
        }

        public T ClickRelatedCareer<T>(int index)
            where T : UiComponent, new()
        {
            var results = Find.Elements(By.CssSelector(".list-big li a")).ToList();
            return Navigate.To<T>(results[index - 1].GetAttribute("href"));
        }

        public T ClickProfilePageBanner<T>()
            where T : UiComponent, new()
        {
            return Navigate.To<T>(By.ClassName("signpost_jp"));
        }

        public string GetCourseTitle(int coursenumber)
        {
            return Find.Elements(By.CssSelector(".dfc-code-jp-trainingCourse .heading-small")).ElementAt(coursenumber - 1)
                .Text;
        }

        public T ClickCourseTitle<T>(int coursenumber)
            where T : UiComponent, new()
        {
            var result = Find.Elements(By.CssSelector(".dfc-code-jp-trainingCourse .heading-small a")).ToList();
            return Navigate.To<T>(result[coursenumber - 1].GetAttribute("href"));
        }

        public T ClickFindCourseLink<T>()
            where T : UiComponent, new()
        {
            var result = Find.Element(By.CssSelector(".dfc-code-jp-FindTrainingCoursesLink a"));
            return Navigate.To<T>(result.GetAttribute("href"));
        }

        internal bool ContainsUrlName(string urlname) => UrlContains(urlname);
    }
}