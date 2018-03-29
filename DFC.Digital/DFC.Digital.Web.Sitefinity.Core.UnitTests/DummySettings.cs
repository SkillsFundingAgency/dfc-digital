using System.Collections.Generic;

namespace DFC.Digital.Web.Sitefinity.Core.UnitTests
{
    public class DummySettings
    {
        public DummySettings()
        {
        }

        public virtual IDictionary<string, object> Values { get; internal set; }
    }
}