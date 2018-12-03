using Autofac;
using Autofac.Core;
using System;

namespace DFC.Digital.NonUIAcceptanceTest
{
    public class AutofacBaseTest<TModule> : IDisposable
        where TModule : IModule, new()
    {
        private IContainer container;

        public AutofacBaseTest()
        {
            var builder = new ContainerBuilder();

            builder.RegisterModule(new TModule());
            builder.RegisterModule<AutofacAcceptanceTestModule>();

            //builder.RegisterModule<NLogModule>();
            //builder.RegisterModule<CoreAutofacModule>();
            container = builder.Build();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (container != null)
                {
                    container.Dispose();
                }
            }
        }

        protected TEntity Resolve<TEntity>()
        {
            return container.Resolve<TEntity>();
        }
    }
}