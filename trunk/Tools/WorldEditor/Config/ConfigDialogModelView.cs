#region License GNU GPL
// ConfigDialogModelView.cs
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

using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Stump.ORM;
using WorldEditor.Annotations;
using WorldEditor.Database;
using WorldEditor.Helpers;

namespace WorldEditor.Config
{
    public class ConfigDialogModelView : INotifyPropertyChanged
    {
        public ConfigDialogModelView()
        {
            Factories = new List<DbFactoryInformation>();
            foreach (DataRow row in DbProviderFactories.GetFactoryClasses().Rows)
            {
                Factories.Add(new DbFactoryInformation()
                    {
                        Name = row["Name"].ToString(),
                        InvariantName = row["InvariantName"].ToString()
                    });
            }
        }

        public bool IsFirstLaunch
        {
            get { return Settings.IsFirstLaunch; }
            set { Settings.IsFirstLaunch = value; }
        }

        public List<DbFactoryInformation> Factories
        {
            get;
            set;
        }

        public DatabaseConfiguration DBConfig
        {
            get;
            set;
        }

        public LoaderSettings LoaderSettings
        {
            get;
            set;
        }


        #region TestDBCommand

        private DelegateCommand m_testDBCommand;

        public DelegateCommand TestDBCommand
        {
            get { return m_testDBCommand ?? (m_testDBCommand = new DelegateCommand(OnTestDB, CanTestDB)); }
        }

        private bool CanTestDB(object parameter)
        {
            return true;
        }

        private void OnTestDB(object parameter)
        {
            MessageService.ShowMessage(null,
                                       DatabaseManager.Instance.TryConnection(DBConfig)
                                           ? "Connection works"
                                           : "Connection doesn't work");
        }

        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}