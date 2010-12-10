// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System.Collections.Generic;

namespace Stump.Server.BaseServer.Database
{
    public enum DatabaseType
    {
        MySQL,
        MSSQL2005,
        MSSQL2008
    }

    public class DatabaseConfiguration
    {
        private readonly IDictionary<string, string> m_props;

        public DatabaseConfiguration(DatabaseType dbtype, string host, string dbName, string user, string password)
        {
            m_props = new Dictionary<string, string>();

            switch (dbtype)
            {
                case DatabaseType.MySQL:
                {
                    m_props.Add("connection.driver_class", "NHibernate.Driver.MySqlDataDriver");
                    m_props.Add("dialect", "NHibernate.Dialect.MySQLDialect");
                    m_props.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                    m_props.Add("connection.connection_string", "Database=" + dbName + ";Data Source=" + host +
                                                                ";User Id=" + user + ";Password=" + password);
                    m_props.Add("proxyfactory.factory_class",
                                "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                    break;
                }
                case DatabaseType.MSSQL2005:
                {
                    m_props.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                    m_props.Add("dialect", "NHibernate.Dialect.MsSql2005Dialect");
                    m_props.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                    m_props.Add("connection.connection_string",
                                "Data Source=" + host + ";Initial Catalog=" + dbName + ";User Id=" + user +
                                ";Password=" + password + ";");
                    m_props.Add("proxyfactory.factory_class",
                                "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                    break;
                }
                case DatabaseType.MSSQL2008:
                {
                    m_props.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                    m_props.Add("dialect", "NHibernate.Dialect.MsSql2008Dialect");
                    m_props.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                    m_props.Add("connection.connection_string",
                                "Data Source=" + host + ";Initial Catalog=" + dbName + ";User Id=" + user +
                                ";Password=" + password + ";");
                    m_props.Add("proxyfactory.factory_class",
                                "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                    break;
                }
            }
        }

        public IDictionary<string, string> GetProperties()
        {
            return m_props;
        }
    }
}