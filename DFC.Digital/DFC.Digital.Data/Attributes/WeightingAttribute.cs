using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class WeightingAttribute : Attribute
    {
        public WeightingAttribute(double weighting)
        {
            this.Weighting = weighting;
        }

        public double Weighting { get; private set; }
    }
}
