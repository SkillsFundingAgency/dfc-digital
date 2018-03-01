﻿using DFC.Digital.Data.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DFC.Digital.Core
{
    public class AsyncHelper : IAsyncHelper
    {
        public void Synchronise(Func<Task> asyncFunction) => Task.Factory.StartNew(asyncFunction, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();

        public T Synchronise<T>(Func<Task<T>> asyncFunction) => Task.Factory.StartNew(asyncFunction, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
    }
}