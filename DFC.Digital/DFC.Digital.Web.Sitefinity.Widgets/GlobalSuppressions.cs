// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:Parameter must not span multiple lines", Justification = "This is okay to ignore as type initialisation.", Scope = "member", Target = "~M:DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers.VocSurveyController.ReturnSurveyViewModel~System.Web.Mvc.ActionResult")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "DFC.Digital.Web.Sitefinity.Widgets.Mvc.Controllers.AdminPanelController.#RestartSitefinity(System.String)", Justification = "We must catch all exceptions and in this instance we need to return the error message to the user(administrator)")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "DFC.Digital.Web.Sitefinity.Widgets", Justification = "We need to keep different types (e.g. Autofac config and MVC controllers) under different namespaces.")]