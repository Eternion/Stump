using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using Castle.ActiveRecord.Framework.Config;
using Stump.Core.Attributes;
using Stump.Core.Xml.Config;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database;
using Stump.Tools.QuickItemEditor.Models;

namespace Stump.Tools.QuickItemEditor
{
    /// <summary>
    /// Interaction logic for DBConnector.xaml
    /// </summary>
    public partial class DBConnector : UserControl
    {
        public event Action<DBConnector, DatabaseAccessor> Connected;

        private void OnConnected(DatabaseAccessor connection)
        {
            Action<DBConnector, DatabaseAccessor> handler = Connected;
            if (handler != null) handler(this, connection);
        }

        public const string ConfigPath = "dbconfig.xml";
        private XmlConfig m_xmlConfig;

        public DBConnector()
        {
            DataContext = this;
            Configuration = new DatabaseConfigurationModel();
            Configuration.DatabaseType = DatabaseType.MySql;

            m_xmlConfig = new XmlConfig(ConfigPath);
            m_xmlConfig.AddAssembly(Assembly.GetExecutingAssembly());
            m_xmlConfig.AddInstance(typeof(DBConnector), this);

            LoadConfigFile();

            InitializeComponent();
        }

        public void LoadConfigFile()
        {
            if (File.Exists(ConfigPath))
            {
                if (m_xmlConfig.Loaded)
                    m_xmlConfig.Reload();
                else
                    m_xmlConfig.Load();
            }
            else
                m_xmlConfig.Create();
        }

        [Variable(true)]
        public DatabaseConfigurationModel Configuration
        {
            get;
            set;
        }

        public DatabaseAccessor DatabaseAccessor
        {
            get;
            private set;
        }

        public bool EtablishConnection()
        {
            var dbAccessor = new DatabaseAccessor(Configuration.GetConfig(), Definitions.DatabaseRevision, typeof(WorldBaseRecord<>), typeof(WorldBaseRecord<>).Assembly, false);

            try
            {
                dbAccessor.Initialize();
                dbAccessor.OpenDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot etablish connection with database : \r\n\r\n" + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                dbAccessor.Reset();
                return false;
            }

            DatabaseAccessor = dbAccessor;
            return true;
        }

        public void SaveConfigFile()
        {
            m_xmlConfig.Save();
        }

        private void OnButtonClicked(object sender, RoutedEventArgs e)
        {
            Dispatcher.BeginInvoke((Action) (() => IsEnabled = false));

            if (EtablishConnection())
            {
                OnConnected(DatabaseAccessor);
            }
            else
            {
                IsEnabled = true;
            }
        }
    }
}
