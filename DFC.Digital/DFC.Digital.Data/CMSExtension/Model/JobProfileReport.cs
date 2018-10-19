using DFC.Digital.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class JobProfileReport : IDigitalDataModel
    {
        public string Title { get; set; }

        public string Name { get; set; }

        public Guid SocCodeId { get; set; }
    }
}
