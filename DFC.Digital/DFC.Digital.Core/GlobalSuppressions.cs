[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "DFC.Digital.Core.Interceptors", Justification = "Project in progress, check after 3 months")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "DFC.Digital.Core.Extensions", Justification = "Project in progress, check after 3 months")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "DFC.Digital.Core.Utilities", Justification = "Project in progress, check after 3 months")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "DFC.Digital.Core", Justification = "Project in progress, check after 3 months")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "DFC.Digital.Core.Logging", Justification = "It makes sense to provide a sub namespace for logging")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "DFC.Digital.Core.Extensions.InstrumentationExtensions.#ReturnValueString(Castle.DynamicProxy.IInvocation)", Justification ="This is a utility for tracing, it is by design to catch all exceptions.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "DFC.Digital.Core.Interceptors.ExceptionInterceptor.#InterceptAsyncFunc`1(Castle.DynamicProxy.IInvocation)", Justification = "Work in progress")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Apim", Scope = "member", Target = "DFC.Digital.Core.Utilities.Constants.#OcpApimSubscriptionKey", Justification ="Reviewed. Correct spelling.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ocp", Scope = "member", Target = "DFC.Digital.Core.Utilities.Constants.#OcpApimSubscriptionKey", Justification = "Reviewed. Correct spelling.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member", Target = "DFC.Digital.Core.Utilities.ServiceHelper.#Use`1(System.Action`1<!!0>,System.String)", Justification = "Reviewed. Not expecting other compilers to use.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member", Target = "DFC.Digital.Core.Utilities.ServiceHelper.#Use`2(System.Func`2<!!0,!!1>,System.String)", Justification = "Reviewed. Not expecting other compilers to use.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed", Scope = "member", Target = "DFC.Digital.Core.Utilities.ServiceHelper.#UseAsync`2(System.Func`2<!!0,System.Threading.Tasks.Task`1<!!1>>,System.String)", Justification = "Reviewed. Not expecting other compilers to use.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope = "type", Target = "DFC.Digital.Core.Utilities.HttpClientService", Justification ="Reviewed, There are performance benefits in reusing the same httpClient. This will be disposed by autofac container at the end of registered lifetime.")]

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the
// Code Analysis results, point to "Suppress Message", and click
// "In Suppression File".
// You do not need to add suppressions to this file manually.