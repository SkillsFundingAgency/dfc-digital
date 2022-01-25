using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DFC.Digital.Web.Sitefinity.Core.OrchardCore
{
    public interface IIdGenerator
    {
        string GenerateUniqueId();
    }
}