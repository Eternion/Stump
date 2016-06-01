using System;
using System.ComponentModel;
using System.Reflection;
using Stump.Core.Reflection;
using Stump.ORM;
using WorldEditor.Config;

namespace WorldEditor.Database
{
    public class DatabaseManager : Singleton<DatabaseManager>, INotifyPropertyChanged
    {
        private readonly DatabaseAccessor m_dbAccessor = new DatabaseAccessor();

        public Stump.ORM.Database Database
        {
            get { return m_dbAccessor.Database; }
        }

        public bool Connected
        {
            get;
            private set;
        }

        public void Initialize(Assembly worldAssembly)
        {
            m_dbAccessor.RegisterMappingAssembly(worldAssembly);
        }

        public void Connect()
        {
            m_dbAccessor.Configuration = Settings.DatabaseConfiguration;
            m_dbAccessor.OpenConnection();

            Connected = true;
        }

        public  void Disconnect()
        {
            m_dbAccessor.CloseConnection();

            Connected = false;
        }

        public bool TryConnection(DatabaseConfiguration config)
        {
            var db = new Stump.ORM.Database(config.GetConnectionString(), config.ProviderName)
            {
                KeepConnectionAlive = true,
                CommandTimeout = (24 * 60 * 60)
            };

            try
            {
                db.OpenSharedConnection();
            }
            catch (Exception)
            {
                return false;
            }

            db.CloseSharedConnection();
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}