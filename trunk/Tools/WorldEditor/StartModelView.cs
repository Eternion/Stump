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

using System.Diagnostics;
using System.Windows;
using DBSynchroniser;
using WorldEditor.Config;
using WorldEditor.Editors.Items;
using WorldEditor.Editors.Tables;
using WorldEditor.Helpers;
using WorldEditor.Search.Items;

namespace WorldEditor
{
    public class StartModelView
    {
        #region EditTableCommand

        private DelegateCommand m_editTableCommand;

        public DelegateCommand EditTableCommand
        {
            get { return m_editTableCommand ?? (m_editTableCommand = new DelegateCommand(OnEditTable, CanEditTable)); }
        }

        private static bool CanEditTable(object parameter)
        {
            return parameter is D2OTable;
        }

        private void OnEditTable(object parameter)
        {
            if (parameter == null || !CanEditTable(parameter))
                return;

            var editor = new TableEditor((D2OTable) parameter);
            editor.Show();
        }

        #endregion

        #region CreateItemCommand

        private DelegateCommand m_createItemCommand;

        public DelegateCommand CreateItemCommand
        {
            get { return m_createItemCommand ?? (m_createItemCommand = new DelegateCommand(OnCreateItem, CanCreateItem)); }
        }

        private static bool CanCreateItem(object parameter)
        {
            return true;
        }

        private static void OnCreateItem(object parameter)
        {
            var editor = new ItemEditor(new ItemWrapper());
            editor.Show();
        }

        #endregion


        #region CreateWeaponCommand

        private DelegateCommand m_createWeaponCommand;

        public DelegateCommand CreateWeaponCommand
        {
            get
            {
                return m_createWeaponCommand ?? (m_createWeaponCommand = new DelegateCommand(OnCreateWeapon, CanCreateWeapon));
            }
        }

        private bool CanCreateWeapon(object parameter)
        {
            return true;
        }

        private void OnCreateWeapon(object parameter)
        {
            var editor = new ItemEditor(new WeaponWrapper());
            editor.Show();
        }

        #endregion


        #region SearchItemCommand

        private DelegateCommand m_searchItemCommand;

        public DelegateCommand SearchItemCommand
        {
            get { return m_searchItemCommand ?? (m_searchItemCommand = new DelegateCommand(OnSearchItem, CanSearchItem)); }
        }

        private static bool CanSearchItem(object parameter)
        {
            return true;
        }

        private static void OnSearchItem(object parameter)
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

        private static void OnOpenConfig(object parameter)
        {
            var dialog = new ConfigDialog();
            if (dialog.ShowDialog() != true)
                return;

            if (!MessageService.ShowYesNoQuestion(null, "To take the config changes in account you have to restart the application. Do you want to restart ?"))
                return;

            Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }

        #endregion
    }
}