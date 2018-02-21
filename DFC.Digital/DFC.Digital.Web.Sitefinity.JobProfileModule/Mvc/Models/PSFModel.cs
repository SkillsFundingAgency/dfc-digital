﻿using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.JobProfileModule.Mvc.Models
{
    public class PsfModel
    {
        public List<PsfSection> Sections { get; set; }

        public PsfSection Section { get; set; }

        public string OptionsSelected { get; set; }
    }
}