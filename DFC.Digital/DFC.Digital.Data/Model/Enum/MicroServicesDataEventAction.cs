﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Model
{
    public enum MicroServicesDataEventAction
    {
        Ignored,
        PublishedOrUpdated,
        UnpublishedOrDeleted,
        Draft
    }
}
