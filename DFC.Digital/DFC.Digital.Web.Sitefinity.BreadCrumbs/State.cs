using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace DFC.Digital.Web.Sitefinity.BreadCrumbs
{
    public class State
	{
		public string SessionCookie { get; set; }
		public SortedSet<StateEntry> Crumbs { get; private set; }
		public StateEntry Current { get; private set; }

		public State(string cookie)
		{
			SessionCookie = cookie;
			Crumbs = new SortedSet<StateEntry>(new StateEntryComparer());
		}

		public void Push(string url, string label)
		{
			Add(url, label);
		}

		public void Push(ActionExecutingContext context, string label, Type resourceType)
		{
			Add(context.HttpContext.Request.Url.LocalPath.ToString(), label, resourceType, context);
		}

		public void SetCurrentLabel(string label)
		{
			Current.Label = label;
		}

		private void Add(string url, string label, Type resourceType = null, ActionExecutingContext actionContext = null)
		{
			var key = url.ToLowerInvariant().GetHashCode();

			int levels = BreadCrumb.HierarchyProvider.GetLevel(url);

			if (Crumbs.Any(x => x.Key == key))
			{
				var newCrumbs = new SortedSet<StateEntry>(new StateEntryComparer());
				var remove = false;
			
				foreach (var crumb in Crumbs)
				{
					if (crumb.Key == key)
					{
						remove = true;
					}
					if (!remove)
					{
						newCrumbs.Add(crumb);
					}
				}
				Crumbs = newCrumbs;
			}

			Current = new StateEntry()
				.WithKey(key)
				.SetContext(actionContext)
				.WithUrl(url)
				.WithLevel(levels)
				.WithLabel(ResourceHelper.GetResourceLookup(resourceType, label));

			Crumbs.Add(Current);
		}
	}

    public class StateEntryComparer : IComparer<StateEntry>
	{
		public int Compare(StateEntry x, StateEntry y)
		{
			return x.Level.CompareTo(y.Level);
		}
	}
}