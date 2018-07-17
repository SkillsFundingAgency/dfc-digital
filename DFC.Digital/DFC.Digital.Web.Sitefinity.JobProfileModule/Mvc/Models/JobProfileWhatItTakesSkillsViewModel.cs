using DFC.Digital.Data.Model;
using System.Collections.Generic;
using System.Linq;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class JobProfileWhatItTakesSkillsViewModel : JobProfileSectionViewModel
    {
        /// <summary>
        /// Gets or sets the section title.
        /// </summary>
        /// <value>
        /// The section title.
        /// </value>
        public string WhatItTakesSectionTitle { get; set; }

        public bool UseSkillsFramework { get; set; }

        public string SkillsSectionIntro { get; set; }

        public IEnumerable<WhatItTakesSkill> WhatItTakesSkills { get; set; } = Enumerable.Empty<WhatItTakesSkill>();

        public string DigitalSkillsLevel { get; set; }
    }
}