using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class TimedLinks
    {
        public IEnumerable<MoreInformationLink> MoreInformationLinks { get; set; }

        public string TimeToExecute { get; set; }
    }
}
