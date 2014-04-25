#region License GNU GPL
// DatabaseLogged.cs
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
using NLog;

namespace Stump.Server.BaseServer.Database
{
    public class DatabaseLogged : ORM.Database
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public DatabaseLogged(IDbConnection connection) : base(connection)
        {
        }

        public DatabaseLogged(string connectionString, string providerName) : base(connectionString, providerName)
        {
        }

        public DatabaseLogged(string connectionString, DbProviderFactory provider) : base(connectionString, provider)
        {
        }

        public DatabaseLogged(string connectionStringName) : base(connectionStringName)
        {
        }

        public override IDbConnection OnConnectionOpened(IDbConnection conn)
        {
            return base.OnConnectionOpened(conn);
        }

        public override void OnConnectionClosing(IDbConnection conn)
        {
            logger.Warn("Database connection closed !");

            base.OnConnectionClosing(conn);
        }

        public override void OnExecutingCommand(IDbCommand cmd)
        {
            base.OnExecutingCommand(cmd);
        }

        public override void OnExecutedCommand(IDbCommand cmd)
        {
            base.OnExecutedCommand(cmd);
        }

        public override void OnException(Exception x)
        {
            logger.Error("DB error : {0}", x);
            base.OnException(x);
        }

    }
}