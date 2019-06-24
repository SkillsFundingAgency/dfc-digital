using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DFC.Digital.Service.CompositeUI
{
    public interface ICompositeClientProxy
    {
         Task<HttpResponseMessage> PostDataAsync(string postEndPoint, string pageDataJSon);
    }
}
