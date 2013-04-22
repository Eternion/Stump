#region License GNU GPL
// StartModelView.cs
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
using WorldEditor.Database;
using WorldEditor.Helpers;

namespace WorldEditor
{
    public class StartModelView
    {

        #region ConnectDBCommand

        private DelegateCommand m_connectDBCommand;

        public DelegateCommand ConnectDBCommand
        {
            get { return m_connectDBCommand ?? (m_connectDBCommand = new DelegateCommand(OnConnectDB, CanConnectDB)); }
        }

        private bool CanConnectDB(object parameter)
        {
            return !DatabaseManager.Instance.Connected;
        }

        private void OnConnectDB(object parameter)
        {
            if (!CanConnectDB(parameter))
                return;

            try
            {
                DatabaseManager.Instance.Connect();

                DisconnectDBCommand.RaiseCanExecuteChanged();
            }
            catch (Exception ex)
            {
                MessageService.ShowError(null, string.Format("Cannot connect to the DB : {0}", ex.Message));
            }
        }

        #endregion


        #region TryConnectionDBCommand

        private DelegateCommand m_tryConnectionDBCommand;

        public DelegateCommand TryConnectionDBCommand
        {
            get { return m_tryConnectionDBCommand ?? (m_tryConnectionDBCommand = new DelegateCommand(OnTryConnectionDB, CanTryConnectionDB)); }
        }

        private bool CanTryConnectionDB(object parameter)
        {
            return true;
        }

        private void OnTryConnectionDB(object parameter)
        {
            if (!CanTryConnectionDB(parameter))
                return;

            MessageService.ShowMessage(null,
                                       DatabaseManager.Instance.TryConnection()
                                           ? "Connection works !"
                                           : "Cannot establish connection with the database :(");
        }

        #endregion


        #region DisconnectDBCommand

        private DelegateCommand m_disconnectDBCommand;

        public DelegateCommand DisconnectDBCommand
        {
            get { return m_disconnectDBCommand ?? (m_disconnectDBCommand = new DelegateCommand(OnDisconnectDB, CanDisconnectDB)); }
        }

        private bool CanDisconnectDB(object parameter)
        {
            return DatabaseManager.Instance.Connected;
        }

        private void OnDisconnectDB(object parameter)
        {
            if (!CanDisconnectDB(parameter))
                return;

            DatabaseManager.Instance.Disconnect();

            ConnectDBCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}