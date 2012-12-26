using System;
using System.Data.Entity;
using System.Data.Objects;
using System.Reflection;
using Stump.Core.Reflection;

namespace Stump.Server.BaseServer.Database
{
    public abstract class DataManager<T> : Singleton<T> where T : class
    {
        public static ORM.Database DefaultDatabase;

        private ORM.Database m_database;

        public ORM.Database Database
        {
            get { return m_database ?? DefaultDatabase; }
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
}