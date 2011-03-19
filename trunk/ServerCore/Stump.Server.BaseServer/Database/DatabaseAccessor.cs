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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using MySql.Data.MySqlClient;
using NLog;
using Stump.BaseCore.Framework.Utils;
using Stump.Database;

namespace Stump.Server.BaseServer.Database
{
    public class DatabaseAccessor
    {
        private static readonly InPlaceConfigurationSource m_globalConfig = new InPlaceConfigurationSource();
        private static readonly List<Type> m_globalTypes = new List<Type>();

        public static void StartEngine()
        {
            ActiveRecordStarter.Initialize(m_globalConfig, m_globalTypes.ToArray());
        }

        private readonly DatabaseConfiguration m_config;

        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        private readonly uint m_databaseRevision;
        private readonly Type m_recordBaseType;

        private Type m_versionType;
        private IVersionRecord m_version;
        private Func<IVersionRecord> m_lastVersionMethod;

        public bool IsInitialized
        {
            get { return ActiveRecordStarter.IsInitialized; }
        }

        public bool IsOpen
        {
            get;
            private set;
        }

        #region Update Methods

        // example : 12_to_14.sql
        private bool SelectSqlUpdateFile(string path)
        {
            if (m_version == null)
                return false;

            var fileName = Path.GetFileNameWithoutExtension(path);

            Match match;
            if (!(match = Regex.Match(fileName, "([0-9]+)_to_([0-9]+)")).Success)
            {
                return false;
            }

            var forVersion = int.Parse(match.Groups[1].Value);
            var toVersion = int.Parse(match.Groups[2].Value);

            return forVersion == m_version.Revision &&
                   toVersion == m_databaseRevision &&
                   forVersion < toVersion;
        }

        private bool SelectAllSqlUpdateFileOf(string path, uint revision)
        {
            if (m_version == null)
                return false;

            string fileName = Path.GetFileNameWithoutExtension(path);

            Match match;
            if (!(match = Regex.Match(fileName, "([0-9]+)_to_([0-9]+)")).Success)
            {
                return false;
            }

            int forVersion = int.Parse(match.Groups[1].Value);
            int toVersion = int.Parse(match.Groups[2].Value);

            return forVersion == m_version.Revision &&
                   forVersion < toVersion;
        }

        private int SortSqlUpdateFile(string path)
        {
            if (m_version == null)
                return -1;

            var fileName = Path.GetFileNameWithoutExtension(path);

            Match match;
            if (!(match = Regex.Match(fileName, "([0-9]+)_to_([0-9]+)")).Success)
            {
                return -1;
            }

            var toVersion = int.Parse(match.Groups[2].Value);

            return toVersion;
        }

        #endregion

        public DatabaseAccessor(DatabaseConfiguration config, uint databaseRevision, Type recordBaseType)
        {
            m_config = config;
            m_databaseRevision = databaseRevision;
            m_recordBaseType = recordBaseType;
            Initialize();
        }

        public void Initialize()
        {
            if (IsInitialized)
                return;

            if (string.IsNullOrEmpty(m_config.Name))
                throw new Exception("Cannot access to database. Database's name is not defined");

            var connectionInfos = m_config.GetConnectionInfo();

            m_globalConfig.Add(m_recordBaseType, connectionInfos);

            var recordsType = ActiveRecordHelper.GetTables(m_recordBaseType);

            m_globalTypes.AddRange(recordsType);
            m_globalTypes.Add(m_recordBaseType);

            m_versionType = ActiveRecordHelper.GetVersionType(recordsType);
            m_lastVersionMethod = ActiveRecordHelper.GetFindVersionMethod(m_versionType);

        }

        public void OpenDatabase()
        {
            if (!IsInitialized)
                throw new Exception("DatabaseAccessor is not initialized");

            if (IsOpen)
                throw new Exception("Database is already open");

            try
            {
                m_version = m_lastVersionMethod();
            }
            catch (ActiveRecordException ex)
            {
                var innerException = ex.InnerException;

                while (innerException != null)
                {
                    if (innerException is MySqlException)
                    {
                        m_logger.Warn("Table 'version' doesn't exists, creating a new Schema...");
                        CreateSchema();

                        m_version = m_lastVersionMethod();
                        break;
                    }

                    innerException = innerException.InnerException;
                }

                if (innerException == null)
                {
                    throw new Exception("Cannot access to databse. Be sure that the database names '" + m_config.Name +
                                        "' exists. Exception : " + ex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot access to database. Unknow reason. Exception : " + ex.Message);
            }

            if (m_version == null)
            {
                m_logger.Error(
                    "Table 'version' is empty, do you want to re-create the schema ? IT WILL ERASE YOUR DATABASE [EXIT IN 60 SECONDS] (y/n)");

                // Wait that user enter any characters
                if (ConditionWaiter.WaitFor(() => Console.KeyAvailable, 60 * 1000, 35))
                {
                    // wait 'enter'
                    var response = (char)Console.In.Peek();

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
                        m_logger.Error(
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
            IsOpen = true;
        }

        internal void CreateSchema()
        {
            try
            {
                ActiveRecordStarter.CreateSchema(m_recordBaseType);

                m_logger.Info("Schema has been created");

                ActiveRecordHelper.CreateVersionRecord(m_versionType, m_databaseRevision);
            }
            catch (Exception ex)
            {
                m_logger.Error("Cannot create schema : " + ex);
            }
        }

        private void ExecuteUpdateAndCreateSchema()
        {
            if (!Directory.Exists(m_config.UpdateFileDir))
                throw new Exception(string.Format("Directory {0} isn't found.", m_config.UpdateFileDir));

            var files = Directory.GetFiles(m_config.UpdateFileDir, "*", SearchOption.AllDirectories).Where(SelectSqlUpdateFile);

            if (files.Count() <= 0)
            {
                var currentVersion = m_version.Revision;
                var revisionfiles = Directory.GetFiles(m_config.UpdateFileDir, "*", SearchOption.AllDirectories).Where(
                        entry => SelectAllSqlUpdateFileOf(entry, currentVersion)).OrderByDescending(SortSqlUpdateFile);

                while (currentVersion != m_databaseRevision)
                {
                    if (revisionfiles.Count() <= 0)
                        throw new FileNotFoundException("The update file isn't found");

                    ActiveRecordStarter.CreateSchemaFromFile(revisionfiles.First());
                    currentVersion = uint.Parse(revisionfiles.First().Split('_').Last());

                    revisionfiles =
                        Directory.GetFiles(m_config.UpdateFileDir, "*", SearchOption.AllDirectories).Where(
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

            ActiveRecordHelper.DeleteVersionRecord(m_versionType);

            ActiveRecordHelper.CreateVersionRecord(m_versionType, m_databaseRevision);
        }

        internal void CreateBackup()
        {
            throw new NotImplementedException();
        }

        internal void EraseBackup()
        {
            throw new NotImplementedException();
        }
    }
}