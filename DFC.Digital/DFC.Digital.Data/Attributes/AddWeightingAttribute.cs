using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class AddWeightingAttribute : Attribute
    {
        public AddWeightingAttribute(double weighting)
        {
            this.Weighting = weighting;
        }

        public double Weighting { get; private set; }
    }
}
