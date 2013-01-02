using System;
using System.Data.Entity;
using System.Data.Objects;
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

    internal static class DataManagerAllocator
    {
        [Initialization(InitializationPass.First, "Initialize DataManagers")]
        public static void Initialize()
        {
            foreach (Type type in Assembly.GetExecutingAssembly().GetTypes())
            {
                if (!type.IsAbstract && type.IsSubclassOfGeneric(typeof(DataManager<>)) &&
                    type != typeof(DataManager<>))
                {
                    Type baseType = type.BaseType;

                    while (baseType != null && baseType.GetGenericTypeDefinition() != typeof(DataManager<>))
                    {
                        baseType = baseType.BaseType;
                    }

                    if (baseType == null)
                        continue;

                    var method = baseType.GetMethod("Initialize", BindingFlags.Default | BindingFlags.FlattenHierarchy);

                    // if the method is already managed we don't call it
                    if (method.GetCustomAttribute<InitializationAttribute>() != null)
                        continue;

                    object instance = baseType.GetProperty("Instance", BindingFlags.Default | BindingFlags.FlattenHierarchy).
                        GetValue(null, new object[0]);
                    method.Invoke(instance, new object[0]);
                }
            }
        }
    }
}