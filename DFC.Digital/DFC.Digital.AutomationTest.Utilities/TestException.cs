using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.AutomationTest.Utilities
{
    public class TestException : Exception
    {
        public TestException(string message) : base(message)
        {
        }
    }
}
