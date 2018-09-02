using System;

namespace DFC.Digital.Core.Interceptors
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public sealed class IgnoreOutputInInterceptionAttribute : Attribute
    {
    }
}