using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class RelatedSkillMapping
    {
        public string ONetOccupationalCode { get; set; }

        public string WhatItTakesSkillTitle { get; set; }

        public decimal OnetRank { get; set; }

        public decimal Rank { get; set; }
    }
}
