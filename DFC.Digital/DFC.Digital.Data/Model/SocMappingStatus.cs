using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public class SocMappingStatus
    {
        public int AwaitingUpdate { get; set; }

        public int UpdateCompleted { get; set; }

        public int SelectedForUpdate { get; set; }
    }
}
