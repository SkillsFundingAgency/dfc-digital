using DFC.Digital.Data.Model;
using System.Collections.Generic;

namespace DFC.Digital.Data.Interfaces
{
    public interface IBuildSearchFilterService
    {
        string BuildPreSearchFilters(PreSearchFiltersResultsModel preSearchFilterModel, IDictionary<string, PreSearchFilterLogicalOperator> indexFields);
    }
}
