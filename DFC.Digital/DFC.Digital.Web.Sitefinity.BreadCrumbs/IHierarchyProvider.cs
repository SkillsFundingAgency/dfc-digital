namespace DFC.Digital.Web.Sitefinity.BreadCrumbs
{
	internal interface IHierarchyProvider
	{
		int GetLevel();
		int GetLevel(string url);
	}
}