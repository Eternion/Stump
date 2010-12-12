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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using MySql.Data.MySqlClient;
using NLog;
using Stump.BaseCore.Framework.Attributes;
using Stump.BaseCore.Framework.Utils;
using Stump.Database;

namespace Stump.Server.BaseServer.Database
{
    public static class DatabaseAccessor
    {
        private const string UpdateFileDir = "./sql_update/";
        public static Logger logger = LogManager.GetCurrentClassLogger();

        private static uint m_databaseRevision;

        private static VersionRecord m_version;

        // example : 12_to_14.sql
        private static readonly Func<string, bool> SelectSqlUpdateFile = entry =>
        {
            if (m_version == null)
                return false;

            string fileName = Path.GetFileNameWithoutExtension(entry);

            Match match;
            if (!(match = Regex.Match(fileName, "([0-9]+)_to_([0-9]+)")).Success)
            {
                return false;
            }

            int forVersion = int.Parse(match.Groups[1].Value);
            int toVersion = int.Parse(match.Groups[2].Value);

            return forVersion == m_version.Revision &&
                   toVersion == m_databaseRevision &&
                   forVersion < toVersion;
        };

        private static readonly Func<string, uint, bool> SelectAllSqlUpdateFileOf = (file, revision) =>
        {
            if (m_version == null)
                return false;

            string fileName = Path.GetFileNameWithoutExtension(file);

            Match match;
            if (!(match = Regex.Match(fileName, "([0-9]+)_to_([0-9]+)")).Success)
            {
                return false;
            }

            int forVersion = int.Parse(match.Groups[1].Value);
            int toVersion = int.Parse(match.Groups[2].Value);

            return forVersion == m_version.Revision &&
                   forVersion < toVersion;
        };

        private static readonly Func<string, int> SortSqlUpdateFile = entry =>
        {
            if (m_version == null)
                return -1;

            string fileName = Path.GetFileNameWithoutExtension(entry);

            Match match;
            if (!(match = Regex.Match(fileName, "([0-9]+)_to_([0-9]+)")).Success)
            {
                return -1;
            }

            int toVersion = int.Parse(match.Groups[2].Value);

            return toVersion;
        };


        private static string m_loginUser;

        private static string m_loginPass;

        /// <remarks>
        /// </remarks>
        private static string m_loginHost;

        /// <summary>
        /// </summary>
        private static string m_databaseName;

        [Variable]
        public static string LoginUser
        {
            get { return m_loginUser; }
            set { m_loginUser = value; }
        }

        [Variable]
        public static string LoginPassword
        {
            get { return m_loginPass; }
            set { m_loginPass = value; }
        }

        [Variable]
        public static string LoginHost
        {
            get { return m_loginHost; }
            set { m_loginHost = value; }
        }

        [Variable]
        public static string DatabaseName
        {
            get { return m_databaseName; }
            set { m_databaseName = value; }
        }

        public static bool IsInitialized
        {
            get { return ActiveRecordStarter.IsInitialized; }
        }

        public static bool IsOpen
        {
            get;
            private set;
        }


        public static void Initialize(Assembly asm, uint databaseRevision, DatabaseType dbtype, DatabaseService service)
        {
            if (!IsInitialized)
            {
                var config = new DatabaseConfiguration(dbtype, m_loginHost, m_databaseName, m_loginUser, m_loginPass);
                var source = new InPlaceConfigurationSource();

                source.Add(typeof (ActiveRecordBase), config.GetProperties());

                ActiveRecordStarter.Initialize(source, ActiveRecordHelper.GetTables(service));

                m_databaseRevision = databaseRevision;
            }
        }

        public static void OpenDatabase()
        {
            if (!IsInitialized)
            {
                throw new Exception("DatabaseAccessor is not initialized");
            }
            if (IsOpen)
            {
                throw new Exception("Database is already open");
            }

            try
            {
                m_version = VersionRecord.FindAll().FirstOrDefault();
            }
            catch (ActiveRecordException ex)
            {
                Exception innerException = ex.InnerException;

                while (innerException != null)
                {
                    if (innerException is MySqlException)
                    {
                        logger.Warn("Table 'version' doesn't exists, creating a new Schema...");
                        CreateSchema();

                        m_version = VersionRecord.FindAll().FirstOrDefault();
                        break;
                    }

                    innerException = innerException.InnerException;
                }

                if (innerException == null)
                {
                    throw new Exception("Cannot access to databse. Be sure that the database names '" + m_databaseName +
                                        "' exists. Exception : " + ex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot access to databse. Unknow reason. Exception : " + ex);
            }


            if (m_version == null)
            {
                logger.Error(
                    "Table 'version' is empty, do you want to re-create the schema ? IT WILL ERASE YOUR DATABASE [EXIT IN 60 SECONDS] (y/n)");

                // Wait that user enter any characters
                if (ConditionWaiter.WaitFor(() => Console.KeyAvailable, 60*1000, 35))
                {
                    // wait 'enter'
                    var response = (char) Console.In.Peek();

                    if (response == 'y')
                        CreateSchema();
                    else
                        throw new Exception("Table 'version' is empty");
                }
                else
                    throw new Exception("Table 'version' is empty");
            }
            else
            {
                if (m_version.Revision < m_databaseRevision)
                {
                    try
                    {
                        ExecuteUpdateAndCreateSchema();
                    }
                    catch (FileNotFoundException)
                    {
                        logger.Error(
                            "Update File doesn't exists, do you want to re-create the schema ? IT WILL ERASE YOUR DATABASE [EXIT IN 60 SECONDS] (y/n)");

                        // Wait that user enter any characters
                        if (ConditionWaiter.WaitFor(() => Console.KeyAvailable, 60 * 1000, 35))
                        {
                            // wait 'enter'
                            var response = (char)Console.In.Peek();

                            if (response == 'y')
                                CreateSchema();
                            else
                                throw;
                        }
                        else
                            throw;
                    }
                }
                else if (m_version.Revision > m_databaseRevision)
                {
                    throw new Exception("This version don't support this database version : " + m_version.Revision);
                }
            }

            IdGenerator.InitializeCreators();

            IsOpen = true;
        }

        internal static void CreateSchema()
        {
            try
            {
                ActiveRecordStarter.CreateSchema();

                logger.Info("Schema has been created");

                var record = new VersionRecord
                    {
                        Revision = m_databaseRevision,
                        UpdateDate = DateTime.Now
                    };

                record.CreateAndFlush();
            }
            catch (Exception ex)
            {
                logger.Error("Cannot create schema : " + ex);
            }
        }

        private static void ExecuteUpdateAndCreateSchema()
        {
            if (!Directory.Exists(UpdateFileDir))
                throw new Exception(string.Format("Directory {0} isn't found.", UpdateFileDir));

            IEnumerable<string> files =
                Directory.GetFiles(UpdateFileDir, "*", SearchOption.AllDirectories).Where(SelectSqlUpdateFile);

            if (files.Count() <= 0)
            {
                uint currentVersion = m_version.Revision;
                IOrderedEnumerable<string> revisionfiles =
                    Directory.GetFiles(UpdateFileDir, "*", SearchOption.AllDirectories).Where(
                        entry => SelectAllSqlUpdateFileOf(entry, currentVersion)).OrderByDescending(SortSqlUpdateFile);

                while (currentVersion != m_databaseRevision)
                {
                    if (revisionfiles.Count() <= 0)
                        throw new FileNotFoundException("The update file isn't found");

                    ActiveRecordStarter.CreateSchemaFromFile(revisionfiles.First());
                    currentVersion = uint.Parse(revisionfiles.First().Split('_').Last());

                    revisionfiles =
                        Directory.GetFiles(UpdateFileDir, "*", SearchOption.AllDirectories).Where(
                            entry => SelectAllSqlUpdateFileOf(entry, currentVersion)).OrderByDescending(
                                SortSqlUpdateFile);
                }
            }
            else
            {
                files = files.OrderByDescending(SortSqlUpdateFile);

                if (files.Count() == 0)
                    throw new FileNotFoundException("The update file isn't found");

                ActiveRecordStarter.CreateSchemaFromFile(files.First());
            }

            VersionRecord.DeleteAll();

            var record = new VersionRecord
                {
                    Revision = m_databaseRevision,
                    UpdateDate = DateTime.Now
                };

            record.CreateAndFlush();
        }

        internal static void CreateBackup()
        {
            throw new NotImplementedException();
        }

        internal static void EraseBackup()
        {
            throw new NotImplementedException();
        }
    }
}