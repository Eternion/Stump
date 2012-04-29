using System;
using System.Data;
using System.IO;
using Castle.ActiveRecord.Framework.Config;
using MySql.Data.MySqlClient;
using Stump.Server.BaseServer.Database;

namespace Stump.Tools.CacheManager.SQL
{
    public class MySqlAccessor
    {
        private readonly MySqlConnection m_connection;

        public MySqlAccessor(DatabaseConfiguration configuration)
        {
            if (configuration.DatabaseType != DatabaseType.MySql &&
                configuration.DatabaseType != DatabaseType.MySql5)
                throw new Exception("Unsupported database : choose a mysql database");

            m_connection = new MySqlConnection(string.Format("Database={0};Data Source={1};User Id={2};Password={3}",
                configuration.Name, configuration.Host, configuration.User, configuration.Password));
        }

        public MySqlConnection Connection
        {
            get { return m_connection; }
        }

        public void Open()
        {
            m_connection.Open();
        }

        public void Close()
        {
            m_connection.Close();
        }

        public int ExecuteNonQuery(string sqlCommand)
        {
            var cmd = new MySqlCommand(sqlCommand, m_connection);
            return cmd.ExecuteNonQuery();
        }

        public DataSet RequestDataSet(string sqlCommand)
        {
            var adapater = new MySqlDataAdapter(sqlCommand, m_connection);
            var dataset = new DataSet();

            adapater.Fill(dataset);
            return dataset;
        }
    }
}