using System;

namespace DFC.Digital.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class IsSuggestibleAttribute : Attribute
    {
    }
}