using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace DFC.Digital.Web.Sitefinity.BreadCrumbs
{

    public class StateEntry
    {
        public ActionExecutingContext Context { get; private set; }
        public string Label { get; set; }
        public int Key { get; set; }
        public string Url { get; set; }
        public int Level { get; set; }

        public StateEntry WithKey(int key)
        {
            Key = key;
            return this;
        }

        public StateEntry WithUrl(string url)
        {
            Url = url;
            return this;
        }

        public StateEntry WithLabel(string label)
        {
            Label = label ?? Label;
            return this;
        }

        public StateEntry WithLevel(int level)
        {
            Level = level;
            return this;
        }


        public StateEntry SetContext(ActionExecutingContext context)
        {
            if (context != null)
            {
                Context = context;
                var type = Context.Controller.GetType();
                var actionName = (string)Context.RouteData.Values["Action"];
                var labelQuery =
                    from m in type.FindMembers(MemberTypes.Method, BindingFlags.Public | BindingFlags.Instance, (memberInfo, _) => memberInfo.Name == actionName, null)
                    let atts = m.GetCustomAttributes(typeof(DisplayAttribute), false)
                    where atts.Length > 0
                    select ((DisplayAttribute)atts[0]).GetName();
                Label = labelQuery.FirstOrDefault() ?? (string)context.RouteData.Values["Action"];
            }

            return this;
        }

        public string Controller
        {
            get
            {
                if (Context == null)
                    return null;

                return (string)Context.RouteData.Values["controller"];
            }
        }

        public string Action
        {
            get
            {
                if (Context == null)
                    return null;

                return (string)Context.RouteData.Values["action"];
            }
        }
    }
}