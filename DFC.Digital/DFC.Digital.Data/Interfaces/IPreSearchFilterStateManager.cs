using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IPreSearchFilterStateManager
    {
        PreSearchFilterSection GetSavedSection(string sectionTitle, PreSearchFilterType filterType);

        string GetStateJson();

        PreSearchFilterSection RestoreOptions(PreSearchFilterSection savedSection, IEnumerable<PreSearchFilter> filters);

        void RestoreState(string previousState);

        void SaveState(PreSearchFilterSection previousSection);

        bool ShouldSaveState(int thisPageNumber, int previousPageNumber);

        void UpdateSectionState(PreSearchFilterSection section);

        PreSearchFilterState GetPreSearchFilterState();
    }
}