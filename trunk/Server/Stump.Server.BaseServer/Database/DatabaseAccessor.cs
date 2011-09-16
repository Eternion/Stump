
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
using Stump.Core.Threading;
using Stump.Server.BaseServer.Database.Interfaces;

namespace Stump.Server.BaseServer.Database
{
    public class DatabaseAccessor
    {
        private static readonly InPlaceConfigurationSource m_globalConfig = new InPlaceConfigurationSource();
        private static readonly List<Type> m_globalTypes = new List<Type>();

        private readonly DatabaseConfiguration m_config;

        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        private readonly uint m_databaseRevision;
        private readonly Type m_recordBaseType;
        private readonly Assembly m_assembly;

        private Type m_versionType;
        private IVersionRecord m_version;
        private Func<IVersionRecord> m_lastVersionMethod;

        public static bool IsInitialized
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

        public DatabaseAccessor(DatabaseConfiguration config, uint databaseRevision, Type recordBaseType, Assembly assembly)
        {
            m_config = config;
            m_databaseRevision = databaseRevision;
            m_recordBaseType = recordBaseType;
            m_assembly = assembly;
        }

        public void Initialize()
        {
            if (IsInitialized)
                return;

            if (string.IsNullOrEmpty(m_config.Name))
                throw new Exception("Cannot access to database. Database's name is not defined");

            var connectionInfos = m_config.GetConnectionInfo();

            m_globalConfig.Add(m_recordBaseType, connectionInfos);

            var recordsType = ActiveRecordHelper.GetTables(m_assembly, m_recordBaseType);

            m_globalTypes.AddRange(recordsType);
            m_globalTypes.Add(m_recordBaseType);

            m_versionType = ActiveRecordHelper.GetVersionType(recordsType);
            m_lastVersionMethod = ActiveRecordHelper.GetFindVersionMethod(m_versionType);

            ActiveRecordStarter.Initialize(m_globalConfig, m_globalTypes.ToArray());
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
                if (ServerBase.InstanceAsBase.ConsoleInterface.AskAndWait(
                    "Table 'version' is empty, do you want to re-create the schema ? IT WILL ERASE YOUR DATABASE",
                    60))
                {
                    CreateSchema();
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
                        if (ServerBase.InstanceAsBase.ConsoleInterface.AskAndWait(
                            "Update File doesn't exist, do you want to re-create the schema ? IT WILL ERASE YOUR DATABASE",
                            60))
                        {
                            CreateSchema();
                        }
                        else
                            throw;
                    }
                }
                else if (m_version.Revision > m_databaseRevision)
                {
                    throw new Exception("The actual version don't support this database revision : " + m_version.Revision);
                }
            }
            IsOpen = true;
        }

        public void CloseDatabase()
        {
            ActiveRecordStarter.ResetInitializationFlag();
        }

        internal void CreateSchema()
        {
            try
            {
                ActiveRecordStarter.CreateSchema(m_recordBaseType);

                m_logger.Info("Database schema rev. {0} has been created", m_databaseRevision);

                ActiveRecordHelper.CreateVersionRecord(m_versionType, m_databaseRevision);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot create schema : " + ex);
            }
        }

        private void ExecuteUpdateAndCreateSchema()
        {
            if (!Directory.Exists(m_config.UpdateFileDir))
                throw new FileNotFoundException(string.Format("Directory {0} isn't found.", m_config.UpdateFileDir));

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

                if (files.Count() == 0) // it's theoretically not possible
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