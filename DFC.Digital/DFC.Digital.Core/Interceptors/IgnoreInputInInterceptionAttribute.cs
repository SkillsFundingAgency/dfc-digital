﻿using System;

namespace DFC.Digital.Core.Interceptors
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class IgnoreInputInInterceptionAttribute : Attribute
    {
    }
}