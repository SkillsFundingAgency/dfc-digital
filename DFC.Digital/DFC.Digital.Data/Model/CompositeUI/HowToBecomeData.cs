﻿using System.Collections.Generic;

namespace DFC.Digital.Data.Model
{
    public class HowToBecomeData
    {
        public IEnumerable<RouteEntryItem> RouteEntries { get; set; }

        public MoreInformation FurtherInformation { get; set; }

        public FurtherRoutes FurtherRoutes { get; set; }

        public string IntroText { get; set; }

        public IEnumerable<RegistrationItem> Registrations { get; set; }

        public bool IsHTBCaDReady { get; set; }
    }
}
