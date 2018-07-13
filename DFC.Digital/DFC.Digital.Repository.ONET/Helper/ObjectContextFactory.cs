using System .Data .Entity .Core .Objects;

namespace DFC.Digital.Repository.ONET.Helper
{
    using System;
    using System.Data.Entity;
    using System.Reflection;

    public interface IObjectContextFactory<out T>
    {
        ConstructorInfo ClassConstructor { get;  }
        T GetContext();
        T GetContext(string connectionString);
    }
    public class ObjectContextFactory<T> :IObjectContextFactory<T> where T : DbContext, new()
    {

        public ConstructorInfo ClassConstructor => typeof(T).GetConstructor(new Type[] { typeof(string) });

        /// <summary>
        /// Gets the default ObjectContext for the project
        /// </summary>
        /// <returns>The default ObjectContext for the project</returns>
        public  T GetContext ( )
        {

            return new T ( ) as T;
        }

        /// <summary>
        /// Gets the  ObjectContext for the project
        /// </summary>
        /// <param name="connectionString">Connection string to use for database queries</param>
        /// <returns>The default ObjectContext for the project</returns>
        public  T GetContext ( string connectionString )
        {
            //Using Reflection here to dynamically create the ObjectContext
            if ( ClassConstructor == null )
            {
                return null;
            }
            else
            {
                T classInstance = ClassConstructor .Invoke ( new object [ ] { connectionString } ) as T;
                return classInstance;
            }
        }
    }
}
