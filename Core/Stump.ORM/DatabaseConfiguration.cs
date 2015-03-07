using System;

namespace Stump.ORM
{
    [Serializable]
    public class DatabaseConfiguration
    {
        public DatabaseConfiguration()
        {
            
        }

        public DatabaseConfiguration(string host, string port, string user, string password, string dbName, string providerName)
        {
            Host = host;
            Port = port;
            User = user;
            Password = password;
            DbName = dbName;
            ProviderName = providerName;
        }

        public string User
        {
            get;
            set;
        }

        public string Password
        {
            get;
            set;
        }

        public string DbName
        {
            get;
            set;
        }

        public string Host
        {
            get;
            set;
        }

        public string Port
        {
            get;
            set;
        }

        public string ProviderName
        {
            get;
            set;
        }

        public string GetConnectionString()
        {
            return string.Format("database={0};uid={1};password={2};server={3};port={4}", DbName, User, Password, Host, Port);
        }
    }
}