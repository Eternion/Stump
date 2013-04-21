#region License GNU GPL
// DatabaseManager.cs
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
using System.Reflection;
using Stump.ORM;

namespace WorldEditor.Database
{
    public static class DatabaseManager
    {
        private static DatabaseAccessor m_dbAccessor = new DatabaseAccessor();


        public static DatabaseConfiguration Configuration
        {
            get { return m_dbAccessor.Configuration; }
            set { m_dbAccessor.Configuration = value; }
        }

        public static Stump.ORM.Database Database
        {
            get { return m_dbAccessor.Database; }
        }

        public static void Initialize(Assembly worldAssembly)
        {
            m_dbAccessor.RegisterMappingAssembly(worldAssembly);
        }

        public static void Connect()
        {
            m_dbAccessor.OpenConnection();
        }

        public static void Disconnect()
        {
            m_dbAccessor.CloseConnection();
        }

        public static bool TryConnection()
        {
            var db = new Stump.ORM.Database(Configuration.GetConnectionString(), Configuration.ProviderName)
            {
                KeepConnectionAlive = true,
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
    }
}