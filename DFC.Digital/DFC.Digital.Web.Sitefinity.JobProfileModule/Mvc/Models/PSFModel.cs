using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class PsfModel
    {
        public ICollection<PsfSection> Sections { get; set; }

        public PsfSection Section { get; set; }

        public IEnumerable<IGrouping<string, PreSearchFilterOption>> GroupedOptions { get; set; }

        public string OptionsSelected { get; set; }

        public int NumberOfMatches { get; set; }

        public string NumberOfMatchesMessage { get; set; }

        public PsfBack Back { get; set; }
    }
}