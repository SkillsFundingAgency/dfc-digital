[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "DFC.Digital.Core.Logging", Justification = "It makes sense to provide a sub namespace for logging")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Scope = "namespace", Target = "DFC.Digital.Core.Interceptors", Justification = "Reviewed, and prefer to seperate the interceptors")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode", Scope = "member", Target = "DFC.Digital.Core.Interceptors.ExceptionInterceptor.#InterceptAsyncFunc`1(Castle.DynamicProxy.IInvocation)", Justification = "Work in progress")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Apim", Scope = "member", Target = "DFC.Digital.Core.Constants.#OcpApimSubscriptionKey", Justification = "Reviewed. Correct spelling.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Ocp", Scope = "member", Target = "DFC.Digital.Core.Constants.#OcpApimSubscriptionKey", Justification = "Reviewed. Correct spelling.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error", Scope = "member", Target = "DFC.Digital.Core.IApplicationLogger.#Error(System.String,System.Exception)", Justification = "It make sense to call the method that logs exceptions in the logger as Error")]

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Scope = "type", Target = "DFC.Digital.Core.Utilities.HttpClientService", Justification ="Reviewed, There are performance benefits in reusing the same httpClient. This will be disposed by autofac container at the end of registered lifetime.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "DFC.Digital.Core.DigitalModelExtensions.#ToJson`1(Polly.DelegateResult`1<!!0>)", Justification = "JsonConvert may throw diffrent exceptions, at this point we need to catch any exceptions, log them and continue.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "DFC.Digital.Core.InstrumentationExtensions.#ReturnValueString(Castle.DynamicProxy.IInvocation)", Justification = "This is part of exception handling and logging code, hence we need to capture all exceptions.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Scope = "member", Target = "DFC.Digital.Core.InstrumentationExtensions.#ReturnValueString(Castle.DynamicProxy.IInvocation)", Justification = "This is a utility for tracing, it is by design to catch all exceptions.")]

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.
//
// To add a suppression to this file, right-click the message in the
// Code Analysis results, point to "Suppress Message", and click
// "In Suppression File".
// You do not need to add suppressions to this file manually.