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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Castle.ActiveRecord;
using Stump.BaseCore.Framework.Extensions;
using Stump.Database.AuthServer;

namespace Stump.Database
{
    public static class ActiveRecordHelper
    {
        public static Dictionary<string, string> GetConfiguration(DatabaseType dbtype, string host, string dbName, string user, string password)
        {
            var props = new Dictionary<string, string>(5);

            switch (dbtype)
            {
                case DatabaseType.MySQL:
                    {
                        props.Add("connection.driver_class", "NHibernate.Driver.MySqlDataDriver");
                        props.Add("dialect", "NHibernate.Dialect.MySQLDialect");
                        props.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                        props.Add("connection.connection_string", "Database=" + dbName + ";Data Source=" + host +
                                                                  ";User Id=" + user + ";Password=" + password);
                        props.Add("proxyfactory.factory_class",
                                  "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                        break;
                    }
                case DatabaseType.MSSQL2005:
                    {
                        props.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                        props.Add("dialect", "NHibernate.Dialect.MsSql2005Dialect");
                        props.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                        props.Add("connection.connection_string",
                                  "Data Source=" + host + ";Initial Catalog=" + dbName + ";User Id=" + user +
                                  ";Password=" + password + ";");
                        props.Add("proxyfactory.factory_class",
                                  "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                        break;
                    }
                case DatabaseType.MSSQL2008:
                    {
                        props.Add("connection.driver_class", "NHibernate.Driver.SqlClientDriver");
                        props.Add("dialect", "NHibernate.Dialect.MsSql2008Dialect");
                        props.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
                        props.Add("connection.connection_string",
                                  "Data Source=" + host + ";Initial Catalog=" + dbName + ";User Id=" + user +
                                  ";Password=" + password + ";");
                        props.Add("proxyfactory.factory_class",
                                  "NHibernate.ByteCode.Castle.ProxyFactoryFactory, NHibernate.ByteCode.Castle");
                        break;
                    }
            }
            return props;
        }

        public static IEnumerable<Type> GetTables(Type dbType)
        {
            var asm = Assembly.GetExecutingAssembly();
            var types = asm.GetTypes();
            
            return types.Where(t => t.IsSubclassOfGeneric(dbType)).ToArray();
        }

        public static Type GetVersionType(IEnumerable<Type> types)
        {
            return types.First(t => t.GetInterfaces().Contains(typeof(IVersionRecord)));
        }

        public static Func<IVersionRecord> GetLastestVersionMethod(Type versionType)
        {
            var method = versionType.BaseType.BaseType.GetMethod("FindAll",Type.EmptyTypes);

            var deleg = Delegate.CreateDelegate(typeof (Func<IEnumerable<IVersionRecord>>), method) as Func<IEnumerable<IVersionRecord>>;
            return () => deleg().FirstOrDefault();
        }

        public static void CreateVersionRecord(Type versionType, uint revision)
        {
            var instance = Activator.CreateInstance(versionType) as IVersionRecord;
            instance.Revision = revision;
            instance.UpdateDate = DateTime.Now;
            instance.CreateAndFlush();
        }

        public static void DeleteVersionRecord(Type versionType)
        {
            versionType.BaseType.BaseType.GetMethod("DeleteAll", Type.EmptyTypes).Invoke(null,null);
        }
    }
}