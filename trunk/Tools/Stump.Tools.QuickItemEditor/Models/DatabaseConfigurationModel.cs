using System.ComponentModel;
using Castle.ActiveRecord.Framework.Config;
using Stump.ORM;
using Stump.Server.BaseServer.Database;

namespace Stump.Tools.QuickItemEditor.Models
{
    public class DatabaseConfigurationModel : INotifyPropertyChanged
    {
        private string m_host;
        private string m_name;
        private string m_password;

        private string m_updateFileDir;

        private string m_user;

        /// <summary>
        /// Database user
        /// </summary>
        public string User
        {
            get { return m_user; }
            set
            {
                m_user = value;
                OnPropertyChanged("User");
            }
        }

        /// <summary>
        /// Database password
        /// </summary>
        public string Password
        {
            get { return m_password; }
            set
            {
                m_password = value;
                OnPropertyChanged("Password");
            }
        }

        /// <summary>
        /// Database host
        /// </summary>
        public string Host
        {
            get { return m_host; }
            set
            {
                m_host = value;
                OnPropertyChanged("Host");
            }
        }

        /// <summary>
        /// Database name to connect to
        /// </summary>
        public string Name
        {
            get { return m_name; }
            set
            {
                m_name = value;
                OnPropertyChanged("Name");
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler evnt = PropertyChanged;
            if (evnt != null)
                evnt(this, new PropertyChangedEventArgs(propertyName));
        }


        public DatabaseConfiguration GetConfig()
        {
            return new DatabaseConfiguration()
            {
                ProviderName = "MySql.Data.MySqlClient",
                Host = Host,
                Password = Password,
                DbName = Name,
                User = User
            };
        }
    }
}