using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Repository.SitefinityCMS.OrchardCore
{
    public interface IIdGenerator
    {
        string GenerateUniqueId();
    }
}