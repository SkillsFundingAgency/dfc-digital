using DFC.Digital.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace DFC.Digital.Core.Utilities
{
    public class HttpSessionStorage<T> : ISessionStorage<T>
    {
        private string SessionKey => typeof(T).FullName;

        public T Get()
        {
            var data = HttpContext.Current.Session[SessionKey];
            return (T)Convert.ChangeType(data, typeof(T));
        }

        public void Save(T data)
        {
            HttpContext.Current.Session.Add(SessionKey, data);
        }
    }
}
