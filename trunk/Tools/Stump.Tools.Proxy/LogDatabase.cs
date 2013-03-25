#region License GNU GPL
// LogDatabase.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA
#endregion

using System;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using Stump.Core.Sql;
using Stump.ORM;

namespace Stump.Tools.Proxy
{
    public class LogDatabase : Database
    {
        public LogDatabase(IDbConnection connection) : base(connection)
        {
        }

        public LogDatabase(string connectionString, string providerName) : base(connectionString, providerName)
        {
        }

        public LogDatabase(string connectionString, DbProviderFactory provider) : base(connectionString, provider)
        {
        }

        public LogDatabase(string connectionStringName) : base(connectionStringName)
        {
        }

        public override void OnExecutedCommand(IDbCommand cmd)
        {
            base.OnExecutedCommand(cmd);

            if (cmd.CommandText.StartsWith("INSERT INTO") || cmd.CommandText.StartsWith("UPDATE"))
            {
                var query = EscapeParameters(cmd.CommandText.Replace(";\nSELECT @@IDENTITY AS NewID;", ""), cmd);

                if (!Directory.Exists("./sql_logs/"))
                    Directory.CreateDirectory("./sql_logs/");

                File.AppendAllText("./sql_logs/logs.sql", query + "\r\n");
                Console.WriteLine(query);
            }
        }

        private string EscapeParameters(string query, IDbCommand cmd)
        {
            var builder = new StringBuilder(query);

            foreach (DbParameter parameter in cmd.Parameters)
            {
                var value = "\"" + SqlBuilder.EscapeField(parameter.Value.ToString()) + "\"";
                builder.Replace(parameter.ParameterName, value);
            }

            return query;
        }
    }
}