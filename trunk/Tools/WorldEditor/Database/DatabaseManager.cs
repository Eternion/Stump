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
using System.ComponentModel;
using System.Reflection;
using Stump.Core.Reflection;
using Stump.ORM;
using WorldEditor.Config;

namespace WorldEditor.Database
{
    public class DatabaseManager : Singleton<DatabaseManager>, INotifyPropertyChanged
    {
        private DatabaseAccessor m_dbAccessor = new DatabaseAccessor();

        public DatabaseManager()
        {
        }

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