
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Castle.ActiveRecord;
using Castle.ActiveRecord.Framework;
using Castle.ActiveRecord.Framework.Config;
using Castle.ActiveRecord.Framework.Internal;
using MySql.Data.MySqlClient;
using NHibernate.Engine;
using NLog;
using Stump.Core.Threading;
using Stump.Server.BaseServer.Database.Interfaces;
using Stump.Server.BaseServer.Database.Patchs;

namespace Stump.Server.BaseServer.Database
{
    public class DatabaseAccessor
    {
        public bool SupportAsks
        {
            get;
            set;
        }

        private readonly InPlaceConfigurationSource m_globalConfig = new InPlaceConfigurationSource();
        private readonly List<Type> m_globalTypes = new List<Type>();

        private readonly DatabaseConfiguration m_config;

        private readonly Logger m_logger = LogManager.GetCurrentClassLogger();

        private readonly uint m_databaseRevision;
        private readonly Type m_recordBaseType;
        private readonly Assembly m_assembly;

        private Type m_versionType;
        private IVersionRecord m_version;
        private Func<IVersionRecord> m_lastVersionMethod;

        private PatchFile[] m_patchs;

        public static bool IsInitialized
        {
            get { return ActiveRecordStarter.IsInitialized; }
        }

        public bool IsOpen
        {
            get;
            private set;
        }

        public DatabaseAccessor(DatabaseConfiguration config, uint databaseRevision, Type recordBaseType, Assembly assembly, bool supportAsks)
        {
            SupportAsks = supportAsks;
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
#if DEBUG
            if (File.Exists("nh_debug.xml"))
            {
                m_globalConfig.SetDebugFlag(true);
                log4net.Config.XmlConfigurator.Configure(new FileInfo("nh_debug.xml"));
            }
#endif
            m_globalConfig.Add(m_recordBaseType, connectionInfos);
            NHibernate.Cfg.Environment.UseReflectionOptimizer = true;

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
                if (SupportAsks && ServerBase.InstanceAsBase.ConsoleInterface.AskAndWait(
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
                        if (SupportAsks && ServerBase.InstanceAsBase.ConsoleInterface.AskAndWait(
                            "Update File doesn't exist, do you want to re-create the schema ? IT WILL ERASE YOUR DATABASE",
                            60))
                        {
                            CreateSchema();

                            m_logger.Info("Database schema update to rev. {0}", m_databaseRevision);

                            m_version = m_lastVersionMethod();
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

        public void Reset()
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
                throw new FileNotFoundException(string.Format("Cannot update schema, update directory {0} isn't found.", m_config.UpdateFileDir));

            m_patchs = Directory.GetFiles(m_config.UpdateFileDir, "*", SearchOption.AllDirectories).
                Select(entry => new PatchFile(entry)).ToArray();

            var sequence = PatchFile.GeneratePatchSequenceExecution(m_patchs, m_version.Revision, m_databaseRevision).ToArray();

            if (!sequence.Any())
            {
                throw new FileNotFoundException(string.Format("Cannot found the patchs to process update {0} to {1}.", m_version.Revision, m_databaseRevision));
            }

            var holder = ActiveRecordMediator.GetSessionFactoryHolder();
            var session = (ISessionImplementor)holder.CreateSession(m_recordBaseType);

            var currentRev = m_version.Revision;
            foreach (var patchFile in sequence)
            {
                m_logger.Info("Executing patch {0}.sql ...", patchFile.FileName);

                try
                {
                    ExecuteSqlFile(patchFile.Path, session.Connection);

                    currentRev = patchFile.ToRevision;
                }
                catch (Exception ex)
                {
                    if (currentRev != m_version.Revision)
                    {
                        ActiveRecordHelper.DeleteVersionRecord(m_versionType);
                        ActiveRecordHelper.CreateVersionRecord(m_versionType, currentRev);
                    }

                    throw new Exception(string.Format("Could not execute patch '{0} properly : {1}", patchFile.Path, ex));
                }
            }

            ActiveRecordHelper.DeleteVersionRecord(m_versionType);
            ActiveRecordHelper.CreateVersionRecord(m_versionType, m_databaseRevision);

            m_logger.Info("Database schema update to rev. {0}", m_databaseRevision);
        }

        public void ExecuteSqlFile(string file, IDbConnection connection)
        {
            var queries = File.ReadAllText(file).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            
            using (IDbCommand command = connection.CreateCommand())
            {
                foreach (string str in queries)
                {
                    var query = str.Trim();

                    if (string.IsNullOrEmpty(query) || string.IsNullOrWhiteSpace(query) || query == "\r\n")
                        continue;

                    command.CommandText = str;
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }
            }
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