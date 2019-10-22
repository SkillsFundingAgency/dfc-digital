using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class InfoDataItem
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Info { get; set; }

        public string CType { get; set; }
    }
}
