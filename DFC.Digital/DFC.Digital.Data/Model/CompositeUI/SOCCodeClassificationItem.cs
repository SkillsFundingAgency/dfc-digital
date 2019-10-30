using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class SOCCodeClassificationItem : ClassificationItem
    {
        public Guid SOCCodeClassificationId { get; set; }

        public string SOCCode { get; set; }
    }
}
