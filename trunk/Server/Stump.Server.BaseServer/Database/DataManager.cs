using System;
using System.Data.Entity;
using System.Data.Objects;
using System.Reflection;

namespace Stump.Server.BaseServer.Database
{
    public abstract class DataManager<T>
        where T : DbContext
    {
        public static T DefaultDatabase;

        private T m_database;

        public T Database
        {
            get { return m_database ?? DefaultDatabase; }
        }

        public void ChangeDataSource(T datasource)
        {
            if (m_database == null)
                m_database = datasource;
            else
            {
                m_database = datasource;

                TearDown();
                Initialize();
            }
        }

        public virtual void Initialize()
        {
        }

        public virtual void TearDown()
        {
        }
    }

    public abstract class DataManager<T, SINGLETON> : DataManager<T>
        where T : DbContext
        where SINGLETON : class
    {
        #region Singleton

        public static SINGLETON Instance
        {
            get { return SingletonAllocator.instance; }
            protected set { SingletonAllocator.instance = value; }
        }

        internal static class SingletonAllocator
        {
            internal static SINGLETON instance;

            static SingletonAllocator()
            {
                CreateInstance(typeof (SINGLETON));
            }

            public static SINGLETON CreateInstance(Type type)
            {
                ConstructorInfo[] ctorsPublic = type.GetConstructors(
                    BindingFlags.Instance | BindingFlags.Public);

                if (ctorsPublic.Length > 0)
                    return instance = (SINGLETON) Activator.CreateInstance(type);

                ConstructorInfo ctorNonPublic = type.GetConstructor(
                    BindingFlags.Instance | BindingFlags.NonPublic, null, Type.EmptyTypes, new ParameterModifier[0]);

                if (ctorNonPublic == null)
                {
                    throw new Exception(
                        type.FullName +
                        " doesn't have a private/protected constructor so the property cannot be enforced.");
                }

                try
                {
                    return instance = (SINGLETON) ctorNonPublic.Invoke(new object[0]);
                }
                catch (Exception e)
                {
                    throw new Exception(
                        "The Singleton couldnt be constructed, check if " + type.FullName + " has a default constructor",
                        e);
                }
            }
        }

        #endregion
    }
}