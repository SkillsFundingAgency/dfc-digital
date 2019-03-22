using System;
using System.Web;

namespace DFC.Digital.Core
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
