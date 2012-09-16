

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.SqlClient;
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

        public DbConnection BuildConnection()
        {
            string driver;

            switch (DatabaseType)
            {
                case DatabaseType.MySql:
                    driver = "MySql.Data.MySqlClient";
                    break;
                default:
                    throw new NotImplementedException();
            }

            var sqlBuilder =
                new SqlConnectionStringBuilder
                    {DataSource = Host, InitialCatalog = Name, Password = Password, UserID = User};

            var factory = DbProviderFactories.GetFactory(driver);
            var connection = factory.CreateConnection();

            if (connection == null)
                throw new Exception(string.Format("Connection not build correcty, is the driver {0} correctly installed ?", driver));

            connection.ConnectionString = sqlBuilder.ConnectionString;

            return connection;
        }
    }
}