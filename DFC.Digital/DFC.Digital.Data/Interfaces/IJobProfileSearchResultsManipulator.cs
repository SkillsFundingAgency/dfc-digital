using DFC.Digital.Data.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Interfaces
{
    public interface IJobProfileSearchResultsManipulator
    {
        SearchResult<JobProfileIndex> ReorderForAlterantiveTitle(SearchResult<JobProfileIndex> searchResult, string searchTerm);
    }
}
