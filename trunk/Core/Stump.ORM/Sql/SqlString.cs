using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace Stump.ORM.Sql
{
    public class SqlString
    {
        private readonly List<MySqlParameter> m_parameters = new List<MySqlParameter>();
        private readonly StringBuilder m_stringBuilder;

        public SqlString()
        {
            m_stringBuilder = new StringBuilder();
        }

        public SqlString(string str)
        {
            m_stringBuilder = new StringBuilder(str);
        }

        public SqlString(string str, IEnumerable<MySqlParameter> parameters)
            : this (str)
        {
            m_parameters = new List<MySqlParameter>(parameters);
        }

        public SqlString(string str, params MySqlParameter[] parameters)
            : this (str)
        {
            m_parameters = new List<MySqlParameter>(parameters);
        }


        public string ActualString
        {
            get { return m_stringBuilder.ToString(); }
        }

        public List<MySqlParameter> Parameters
        {
            get { return m_parameters; }
        }

        public void AddParameter()
        {
            m_parameters.Add(new MySqlParameter());
        }

        public void AddParameter(MySqlParameter parameter)
        {
            m_parameters.Add(parameter);
        }

        public void AddParameter(string name, object value)
        {
            m_parameters.Add(new MySqlParameter(name, value));
        }

        public void Append(string str)
        {
            m_stringBuilder.Append(str);
        }

        public string GetSqlString()
        {
            return m_stringBuilder.ToString();
        }

        public MySqlCommand CreateCommand()
        {
            var cmd = new MySqlCommand(GetSqlString());

            foreach (var mySqlParameter in Parameters)
            {
                cmd.Parameters.Add(mySqlParameter);
            }

            return cmd;
        }

        public override string ToString()
        {
            return GetSqlString();
        }

        public SqlString Copy()
        {
            return new SqlString(GetSqlString(), Parameters.ToArray());
        }
    }
}