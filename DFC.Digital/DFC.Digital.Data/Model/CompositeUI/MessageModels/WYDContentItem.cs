using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class WYDContentItem : RelatedContentItem
    {
        public string Description { get; set; }

        public bool IsNegative { get; set; }
    }
}
