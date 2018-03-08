using System;
using System.Collections.Generic;

namespace DFC.Digital.Service.AzureSearch
{
    public interface ISuggesterBuilder
    {
        IList<string> BuildForType(Type typeofT);
    }
}