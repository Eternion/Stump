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
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.WorldServer.Database.Items.Templates;
using WorldEditor.Config;
using WorldEditor.Database;
using WorldEditor.Editors.Items;
using WorldEditor.Helpers;
using WorldEditor.Loaders.D2O;
using WorldEditor.Loaders.I18N;
using WorldEditor.Search.Items;

namespace WorldEditor
{
    public class StartModelView
    {


        #region CreateItemCommand

        private DelegateCommand m_createItemCommand;

        public DelegateCommand CreateItemCommand
        {
            get { return m_createItemCommand ?? (m_createItemCommand = new DelegateCommand(OnCreateItem, CanCreateItem)); }
        }

        private bool CanCreateItem(object parameter)
        {
            return true;
        }

        private void OnCreateItem(object parameter)
        {
            var editor = new ItemEditor(new ItemWrapper());
            editor.Show();
        }

        #endregion


        #region SearchItemCommand

        private DelegateCommand m_searchItemCommand;

        public DelegateCommand SearchItemCommand
        {
            get { return m_searchItemCommand ?? (m_searchItemCommand = new DelegateCommand(OnSearchItem, CanSearchItem)); }
        }

        private bool CanSearchItem(object parameter)
        {
            return true;
        }

        private void OnSearchItem(object parameter)
        {
            var window = new ItemSearchDialog();
            window.Show();
        }

        #endregion


        #region OpenConfigCommand

        private DelegateCommand m_openConfigCommand;

        public DelegateCommand OpenConfigCommand
        {
            get { return m_openConfigCommand ?? (m_openConfigCommand = new DelegateCommand(OnOpenConfig, CanOpenConfig)); }
        }

        private bool CanOpenConfig(object parameter)
        {
            return true;
        }

        private void OnOpenConfig(object parameter)
        {
            var dialog = new ConfigDialog();
            if (dialog.ShowDialog() == true)
            {
                if (MessageService.ShowYesNoQuestion(null,
                                                     "To take the config changes in account you have to restart the application. Do you want to restart ?"))
                {
                    Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                }
            }
        }

        #endregion
    }
}