using System;
using System.Reflection;
using Stump.Core.Extensions;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Initialization;

namespace Stump.Server.BaseServer.Database
{
    public abstract class DataManager
    {
        public static ORM.Database DefaultDatabase;

        private ORM.Database m_database;

        public ORM.Database Database
        {
            get
            {
                return m_database ?? DefaultDatabase;
            }
        }

        public void ChangeDataSource(ORM.Database datasource)
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

    public abstract class DataManager<T> : Singleton<T> where T : class
    {
        private ORM.Database m_database;

        public ORM.Database Database
        {
            get
            {
                return m_database ?? DataManager.DefaultDatabase;
            }
        }

        public void ChangeDataSource(ORM.Database datasource)
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

    public static class DataManagerAllocator
    {
        public static Assembly Assembly;

        [Initialization(InitializationPass.First, "Initialize DataManagers")]
        public static void Initialize()
        {
            foreach (var type in Assembly.GetTypes())
            {
                if (type.IsAbstract || !type.IsSubclassOfGeneric(typeof (DataManager<>)) ||
                    type == typeof (DataManager<>)) continue;

                var method = type.GetMethod("Initialize", BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance);

                // if the method is already managed we don't call it
                if (method.GetCustomAttribute<InitializationAttribute>(true) != null)
                    continue;

                object instance = type.GetProperty("Instance", BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static).
                    GetValue(null, new object[0]);
                method.Invoke(instance, new object[0]);
            }
        }
    }
}