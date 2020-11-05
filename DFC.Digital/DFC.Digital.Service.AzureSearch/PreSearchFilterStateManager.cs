using DFC.Digital.Data.Interfaces;
using DFC.Digital.Data.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Service.AzureSearch
{
    public class PreSearchFilterStateManager : IPreSearchFilterStateManager
    {
        private PreSearchFilterState StateModel { get; set; }

        private string StateJson { get; set; }

        public void RestoreState(string previousState)
        {
            //StateJson = HttpUtility.UrlDecode(stateJson);
            StateJson = previousState;

            if (!string.IsNullOrEmpty(StateJson))
            {
                StateModel = JsonConvert.DeserializeObject<PreSearchFilterState>(StateJson);
            }
            else
            {
                StateModel = new PreSearchFilterState { Sections = new List<PreSearchFilterSection>() };
            }
        }

        public void UpdateSectionState(PreSearchFilterSection section)
        {
            var savedSectionIndex = StateModel?.Sections?.ToList().FindIndex(s => s.Name.Equals(section.Name, StringComparison.InvariantCultureIgnoreCase) && s.SectionDataType == section.SectionDataType);

            //just keep the selected options
            section.Options = section.Options?.Where(o => o.IsSelected == true).ToList();

            if (savedSectionIndex > -1)
            {
                StateModel.Sections.ToList()[savedSectionIndex.Value].Options = section.Options;
            }
            else
            {
                StateModel.Sections.Add(section);
            }

            StateJson = JsonConvert.SerializeObject(StateModel);
        }

        public PreSearchFilterSection GetSavedSection(string sectionTitle, PreSearchFilterType filterType)
        {
            return StateModel?.Sections?
                .FirstOrDefault(s => s.Name.Equals(sectionTitle, StringComparison.InvariantCultureIgnoreCase) && s.SectionDataType == filterType);
        }

        public PreSearchFilterSection RestoreOptions(PreSearchFilterSection savedSection, IEnumerable<PreSearchFilter> filters)
        {
            // If the user choose to go back, then there should be a saved version of the page.
            var filterSection = new PreSearchFilterSection
            {
                SingleSelectedValue = savedSection?.SingleSelectedValue,
                Options = new List<PreSearchFilterOption>()
            };

            var idCount = 0;
            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    var psfOptionKey = filter.UrlName;
                    var savedStated = savedSection?.Options?.FirstOrDefault(o => o.OptionKey == psfOptionKey);

                    filterSection.Options.Add(new PreSearchFilterOption
                    {
                        Id = (idCount++).ToString(),
                        IsSelected = savedStated?.IsSelected ?? false,
                        Name = filter.Title,
                        Description = filter.Description,
                        OptionKey = psfOptionKey,
                        ClearOtherOptionsIfSelected = filter.NotApplicable ?? false,
                        PSFCategory = filter.PSFCategory
                    });
                }
            }

            return filterSection;
        }

        public string GetStateJson()
        {
            //return HttpUtility.UrlEncode(StateJson);
            return StateJson;
        }

        public void SaveState(PreSearchFilterSection previousSection)
        {
            if (previousSection != null)
            {
                if (previousSection.SingleSelectOnly)
                {
                    //if we have a single select option just keep that
                    if (previousSection.SingleSelectedValue != null)
                    {
                        previousSection.Options.First(o => o.OptionKey == previousSection.SingleSelectedValue).IsSelected = true;
                    }
                }
                else
                {
                    //If there is no JS client side we may have other options selected as well, clear out other options if NON applicable is selected
                    if (previousSection.Options.Any(o => o.IsSelected && o.ClearOtherOptionsIfSelected))
                    {
                        previousSection.Options = previousSection.Options.Where(o => o.IsSelected && o.ClearOtherOptionsIfSelected).Take(1).ToList();
                    }
                }

                UpdateSectionState(previousSection);
            }
        }

        public bool ShouldSaveState(int thisPageNumber, int previousPageNumber)
        {
            // Not if the user wanted to go back
            // if we have come Forwards and not on the first page, then continue was pressed save state
            return thisPageNumber > 1 && previousPageNumber < thisPageNumber;
        }

        public PreSearchFilterState GetPreSearchFilterState()
        {
            return StateModel;
        }
    }
}