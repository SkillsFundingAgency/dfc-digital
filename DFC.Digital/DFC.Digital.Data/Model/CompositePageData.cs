﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class CompositePageData
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string MetaDescription { get; set; }

        public string MetaKeyWords { get; set; }

        public IEnumerable<string> Content { get; set; } = Enumerable.Empty<string>();

        public bool IncludeInSitemap { get; set; }
    }
}