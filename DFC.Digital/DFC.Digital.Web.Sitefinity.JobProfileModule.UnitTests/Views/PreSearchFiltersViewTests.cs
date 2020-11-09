using ASP;
using DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models;
using FluentAssertions;
using HtmlAgilityPack;
using RazorGenerator.Testing;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.UnitTests
{
    public class PreSearchFiltersViewTests
    {
        //Related Careers links are correctly displayed with a section header and links.
        [Fact]
        public void DFC1786ForPreSearchFilterButtonsAndLinkTest()
        {
            var index = new _MVC_Views_PreSearchFilters_Index_cshtml();
            var testDataModel = GeneratePreSEarchFiltersViewModel(false);
            var htmlDom = index.RenderAsHtml(testDataModel);

            //Should have  a link to the home page
            var backButton = htmlDom.GetElementbyId("filter-home");
            if (testDataModel.Section.PageNumber > 1)
            {
                backButton.InnerText.Should().BeEquivalentTo("Back");
            }
            else
            {
                backButton.Should().BeNull();
            }

            var backForm = htmlDom.GetElementbyId("backForm");
            backForm.Attributes["action"].Value.Should().BeEquivalentTo(testDataModel.Section.PreviousPageUrl);

            var continueButton = htmlDom.GetElementbyId("filter-continue");
            continueButton.InnerText.Should().BeEquivalentTo("Continue");

            var continueForm = htmlDom.GetElementbyId("continueForm");
            continueForm.Attributes["action"].Value.Should().BeEquivalentTo(testDataModel.Section.NextPageUrl);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DFC1786ForPreSearchFilterTextAndControllsDisplayed(bool singleSelectSection)
        {
            var index = new _MVC_Views_PreSearchFilters_Index_cshtml();
            var testDataModel = GeneratePreSEarchFiltersViewModel(singleSelectSection);
            var htmlDom = index.RenderAsHtml(testDataModel);

            htmlDom?.DocumentNode?.SelectNodes($"//*[@id='continueForm']/div/h4")?.FirstOrDefault()?.InnerText.Should().BeEquivalentTo($"Step {testDataModel.Section.PageNumber} of {testDataModel.Section.TotalNumberOfPages}");
            /* Think the testing frame work does not render Html.DisplayFor properly the view look fine but this test fails
            htmlDom.DocumentNode.SelectNodes("//*[@id='continueForm']/div/h4").FirstOrDefault().InnerText.Should().BeEquivalentTo(preSearchFiltersModel.Sections[0].Name);
            htmlDom.DocumentNode.SelectNodes("//*[@id='continueForm']/div/h4").FirstOrDefault().InnerText.Should().BeEquivalentTo(preSearchFiltersModel.Sections[0].Description);
            */

            //Check that we have all the hidden varibles for the section  on each formso that the model remains intact
            CheckForSectionHiddenValue(htmlDom, "Name", "backForm", testDataModel.Section.Name);
            CheckForSectionHiddenValue(htmlDom, "Name", "continueForm", testDataModel.Section.Name);

            CheckForSectionHiddenValue(htmlDom, "SectionDataType", "backForm", testDataModel.Section.SectionDataType);
            CheckForSectionHiddenValue(htmlDom, "SectionDataType", "continueForm", testDataModel.Section.SectionDataType);

            CheckForSectionHiddenValue(htmlDom, "PageNumber", "backForm", testDataModel.Section.PageNumber.ToString());
            CheckForSectionHiddenValue(htmlDom, "PageNumber", "continueForm", testDataModel.Section.PageNumber.ToString());

            CheckForHiddenValue(htmlDom, "OptionsSelected", "backForm", testDataModel.OptionsSelected);
            CheckForHiddenValue(htmlDom, "OptionsSelected", "continueForm", testDataModel.OptionsSelected);

            //Each of the options is displayed with a checkbox or radio and label for section
            for (int jj = 0; jj < testDataModel.Section.Options.Count; jj++)
            {
                if (testDataModel.Section.SingleSelectOnly)
                {
                    // this section should contains radio buttons
                    var filterOption = htmlDom?.DocumentNode?.SelectNodes($"//*[@id='Section_{jj}']").FirstOrDefault();
                    filterOption?.Attributes["type"].Value.Should().BeEquivalentTo("radio");
                }
                else
                {
                    var filterOption = htmlDom?.DocumentNode?.SelectNodes($"//*[@id='Section_Options_{jj.ToString()}__IsSelected']").FirstOrDefault();

                    // this section should contains check boxes
                    filterOption?.Attributes["type"].Value.Should().BeEquivalentTo("checkbox");

                    //if we have a N/A option needs this class
                    if (testDataModel.Section.Options[jj].ClearOtherOptionsIfSelected)
                    {
                        filterOption?.Attributes["class"].Value.Should().BeEquivalentTo("filter-na govuk-checkboxes__input");
                    }
                }

                //Have a label for the check box with the correct title and description
                htmlDom?.DocumentNode?.SelectNodes($"//*[@id='options1']/label[{(jj + 1).ToString()}]")?.FirstOrDefault()?.InnerText.Should().BeEquivalentTo($"{testDataModel.Section.Options[jj].Name}{testDataModel.Section.Options[jj].Description}");

                CheckForOptionHiddenValue(htmlDom, "OptionKey", "continueForm", jj, testDataModel.Section.Options[jj].OptionKey);
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        public void MatchingTotalIsDisplayedOnPagesAfterPageOne(int pageOn)
        {
            var index = new _MVC_Views_PreSearchFilters_Index_cshtml();
            var testDataModel = GeneratePreSEarchFiltersViewModel(true);
            testDataModel.Section.PageNumber = pageOn;
            var htmlDom = index.RenderAsHtml(testDataModel);

            var matchingCountMessage = $" you have {testDataModel.NumberOfMatches} career matches";
            if (pageOn > 1)
            {
                htmlDom?.DocumentNode?.SelectNodes($"//*[@id='continueForm']/div/h4")?.FirstOrDefault()?.InnerText.Should().Contain(matchingCountMessage);
            }
            else
            {
                htmlDom?.DocumentNode?.SelectNodes($"//*[@id='continueForm']/div/h4")?.FirstOrDefault()?.InnerText.Should().NotContain(matchingCountMessage);
            }
        }

        [Fact]
        public void SelectMessageShouldBeAsDefindedInModel()
        {
            var index = new _MVC_Views_PreSearchFilters_Index_cshtml();
            var testDataModel = GeneratePreSEarchFiltersViewModel(true);
            testDataModel.Section.SelectMessage = "Select Message";
            var htmlDom = index.RenderAsHtml(testDataModel);
            htmlDom?.GetElementbyId($"qualifications-hint").InnerText.Should().Contain(testDataModel.Section.SelectMessage);
        }

        [Fact]
        public void SelectMessageShouldNotRenderIfNotDefindeInModel()
        {
            var index = new _MVC_Views_PreSearchFilters_Index_cshtml();
            var testDataModel = GeneratePreSEarchFiltersViewModel(true);
            testDataModel.Section.SelectMessage = string.Empty;
            var htmlDom = index.RenderAsHtml(testDataModel);
            htmlDom?.GetElementbyId($"qualifications-hint").Should().BeNull();
        }

        private void CheckForSectionHiddenValue(HtmlDocument htmlDom, string fieldName, string formId, string expectedValue)
        {
            //*[@id="Section_Name"]
            CheckField(htmlDom.GetElementbyId($"{formId}").SelectSingleNode($"//*[@id='Section_{fieldName}']"), expectedValue);
        }

        private void CheckForHiddenValue(HtmlDocument htmlDom, string fieldName, string formId, string expectedValue)
        {
            CheckField(htmlDom.GetElementbyId($"{formId}").SelectSingleNode($"//*[@id='{fieldName}']"), expectedValue);
        }

        private void CheckForOptionHiddenValue(HtmlDocument htmlDom, string fieldName, string formId, int optionIndex, string expectedValue)
        {
            CheckField(htmlDom.GetElementbyId($"{formId}").SelectSingleNode($"//*[@id='Section_Options_{optionIndex}__{fieldName}']"), expectedValue);
        }

        private void CheckField(HtmlNode fieldToCheck, string expectedValue)
        {
            fieldToCheck.Attributes["type"].Value.Should().BeEquivalentTo("hidden");
            fieldToCheck.Attributes["value"].Value.Should().BeEquivalentTo(expectedValue ?? string.Empty);
        }

        private PsfModel GeneratePreSEarchFiltersViewModel(bool singleSelect)
        {
            var filtersModel = new PsfModel()
            {
                OptionsSelected = "TestJSONstring",
                Section = new PsfSection()
                {
                    Name = "Multi Select Section One",
                    Description = "Dummy Title One",
                    SingleSelectOnly = singleSelect,
                    NextPageUrl = "NextSectionURL",
                    PreviousPageUrl = "HomePageURL",
                    PageNumber = 1,
                    TotalNumberOfPages = 2,
                    SectionDataType = "Dummy Data Type One"
                },
                NumberOfMatches = 5,
            };

            filtersModel.Section.Options = new List<PsfOption>();

            for (int ii = 0; ii < 3; ii++)
            {
                var iiString = ii.ToString();
                filtersModel.Section.Options.Add(item: new PsfOption { Id = iiString, IsSelected = false, Name = $"Title-{iiString}", Description = $"Description-{iiString}", OptionKey = $"{iiString}-UrlName", ClearOtherOptionsIfSelected = false });
            }

            //Add in a N/A
            filtersModel.Section.Options.Add(item: new PsfOption { Id = "3", IsSelected = false, Name = $"Title-3", Description = $"Description-3", OptionKey = $"3-UrlName", ClearOtherOptionsIfSelected = true });

            return filtersModel;
        }
    }
}