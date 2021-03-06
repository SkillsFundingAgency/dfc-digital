[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member", Target = "DFC.Digital.Service.AzureSearch.SearchExtensions.#ToSearchResultItems`1(Microsoft.Azure.Search.Models.DocumentSearchResult`1<!!0>,DFC.Digital.Data.Model.SearchProperties)", Justification = "CLR type used by the search results")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Scope = "member", Target = "DFC.Digital.Service.AzureSearch.SearchExtensions.#ToSuggestResultItems`1(Microsoft.Azure.Search.Models.DocumentSuggestResult`1<!!0>)", Justification = "Considered and ruled out as this result type makes sense than the resquest and response types")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "DFC.Digital.Service.AzureSearch.ISuggesterBuilder.#BuildForType`1()", Justification = "Reviewed and this is apropriate for the scenario it is used")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Scope = "member", Target = "DFC.Digital.Service.AzureSearch.IWeightingBuilder.#BuildForType`1()", Justification = "Reviewed and this is apropriate for the scenario it is used")]

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the
// Code Analysis results, point to "Suppress Message", and click
// "In Suppression File".
// You do not need to add suppressions to this file manually.