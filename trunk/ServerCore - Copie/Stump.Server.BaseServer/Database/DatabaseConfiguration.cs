

using System.Collections.Generic;
using Castle.ActiveRecord.Framework.Config;

namespace Stump.Server.BaseServer.Database
{
    public class DatabaseConfiguration
    {
        public DatabaseType DatabaseType
        {
            get;
            set;
        }

        /// <summary>
        /// Folder which contains all of sql file to update db
        /// </summary>
        public string UpdateFileDir
        {
            get;
            set;
        }

        /// <summary>
        /// Database user
        /// </summary>
        public string User
        {
            get;
            set;
        }

        /// <summary>
        /// Database password
        /// </summary>
        public string Password
        {
            get;
            set;
        }

        /// <summary>
        /// Database host
        /// </summary>
        public string Host
        {
            get;
            set;
        }

        /// <summary>
        /// Database name to connect to
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        public Dictionary<string, string> GetConnectionInfo()
        {
            var props = new Dictionary<string, string>(5);

            switch (DatabaseType)
            {
                case DatabaseType.MySql:
                    {
                        props.Add("connection.driver_class", "NHibernate.Driver.MySqlDataDriver");
                        props.Add("dialect", "NHibernate.Dialect.MySQLDialect");
                        props.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                        props.Add("connection.connection_string", "Database=" + Name + ";Data Source=" + Host +
                                                                  ";User Id=" + User + ";Password=" + Password);
                        props.Add("proxyfactory.factory_class",
                                  "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                        break;
                    }
                case DatabaseType.MsSqlServer2005:
                    {
                        props.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                        props.Add("dialect", "NHibernate.Dialect.MsSql2005Dialect");
                        props.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                        props.Add("connection.connection_string",
                                  "Data Source=" + Host + ";Initial Catalog=" + Name + ";User Id=" + User +
                                  ";Password=" + Password + ";");
                        props.Add("proxyfactory.factory_class",
                                  "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                        break;
                    }
                case DatabaseType.MsSqlServer2008:
                    {
                        props.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                        props.Add("dialect", "NHibernate.Dialect.MsSql2008Dialect");
                        props.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                        props.Add("connection.connection_string",
                                  "Data Source=" + Host + ";Initial Catalog=" + Name + ";User Id=" + User +
                                  ";Password=" + Password + ";");
                        props.Add("proxyfactory.factory_class",
                                  "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                        break;
                    }
            }
            return props;
        }
    }
}