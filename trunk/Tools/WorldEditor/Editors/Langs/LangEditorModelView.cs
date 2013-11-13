#region License GNU GPL
// D2IEditorModelView.cs
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
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using DBSynchroniser.Records.Langs;
using Stump.Core.I18N;
using Stump.DofusProtocol.D2oClasses.Tools.D2i;
using Stump.ORM.SubSonic.Extensions;
using WorldEditor.Helpers;
using WorldEditor.Loaders.I18N;

namespace WorldEditor.Editors.Langs
{
    public class LangEditorModelView : INotifyPropertyChanged
    {
        private readonly LangEditor m_editor;
        private uint m_highestId;
        private Stack<LangGridRow> m_deletedRows = new Stack<LangGridRow>();


        private ReadOnlyObservableCollection<LangGridRow> m_readOnylRows;
        private ObservableCollection<LangGridRow> m_rows = new ObservableCollection<LangGridRow>();
        public LangEditorModelView(LangEditor editor)
        {
            m_editor = editor;
            m_readOnylRows = new ReadOnlyObservableCollection<LangGridRow>(m_rows);
            LastFoundIndex = -1;
            Open();
        }

        public ReadOnlyObservableCollection<LangGridRow> Rows
        {
            get { return m_readOnylRows; }
        }

        public string Title
        {
            get { return "Lang Editor"; }
        }

        private void Open()
        {
            var uiTexts = I18NDataManager.Instance.LangsUi;
            foreach (var uiText in uiTexts)
            {
                m_rows.Add(new LangTextUiRow(uiText.Value.Copy()));
            }

            var texts = I18NDataManager.Instance.Langs;
            foreach (var text in texts)
            {
                m_rows.Add(new LangTextRow(text.Value.Copy()));
            }

            m_highestId = texts.Keys.Max();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private LangGridRow FindNext()
        {
            var startIndex = LastFoundIndex == -1 || LastFoundIndex + 1 >= Rows.Count ? 0 : LastFoundIndex + 1;

            LangGridRow row = null;
            var index = -1;
            if (SearchType == "Key")
            {
                var isNumber = SearchText.All(char.IsDigit);
                if (isNumber)
                {
                    var id = int.Parse(SearchText);
                    for (var i = startIndex; i < Rows.Count; i++)
                    {
                        if (!(Rows[i] is LangTextRow) || (Rows[i] as LangTextRow).Id != id)
                            continue;

                        row = Rows[i];
                        index = i;
                        break;
                    }
                }

                if (row == null)
                    for (var i = startIndex; i < Rows.Count; i++)
                    {
                        if (Rows[i].GetKey().IndexOf(SearchText, StringComparison.InvariantCultureIgnoreCase) == -1)
                            continue;
                        row = Rows[i];
                        index = i;
                        break;
                    }
            }
            else
            {
                var lang = (Languages) Enum.Parse(typeof (Languages), SearchType);
                for (var i = startIndex; i < Rows.Count; i++)
                {
                    if (!Rows[i].DoesContainText(SearchText, lang))
                        continue;

                    row = Rows[i];
                    index = i;
                    break;
                }
            }

            if (row == null)
            {
                LastFoundIndex = -1;
                return null;
            }

            LastFoundIndex = index;
            return row;
        }

        #region FindCommand

        private DelegateCommand m_findCommand;

        public DelegateCommand FindCommand
        {
            get { return m_findCommand ?? (m_findCommand = new DelegateCommand(OnFind, CanFind)); }
        }

        private string m_searchText;

        public string SearchText
        {
            get { return m_searchText; }
            set { m_searchText = value; }
        }

        public string SearchType
        {
            get;
            set;
        }

        public int LastFoundIndex
        {
            get;
            set;
        }

        private bool CanFind(object parameter)
        {
            return !string.IsNullOrEmpty(m_searchText);
        }

        private void OnFind(object parameter)
        {
            if (!CanFind(parameter))
                return;

            LastFoundIndex = 0;
            var row = FindNext();

            FindNextCommand.RaiseCanExecuteChanged();

            if (row != null)
            {
                m_editor.TextsGrid.SelectedItem = row;
                m_editor.TextsGrid.ScrollIntoView(row);
                m_editor.TextsGrid.Focus();
            }
            else
            {
                MessageService.ShowMessage(m_editor, "Not found");
            }
        }

        #endregion


        #region FindNextCommand

        private DelegateCommand m_findNextCommand;

        public DelegateCommand FindNextCommand
        {
            get { return m_findNextCommand ?? (m_findNextCommand = new DelegateCommand(OnFindNext, CanFindNext)); }
        }

        private bool CanFindNext(object parameter)
        {
            return LastFoundIndex != -1;
        }

        private void OnFindNext(object parameter)
        {
            var row = FindNext();

            if (row == null)
                row = FindNext();

            if (row != null)
            {
                m_editor.TextsGrid.SelectedItem = row;
                m_editor.TextsGrid.ScrollIntoView(row);
                m_editor.TextsGrid.Focus();
            }
            else
            {
                MessageService.ShowMessage(m_editor, "Not found");
            }
        }

        #endregion

        #region AddRowCommand

        private DelegateCommand m_addRowCommand;

        public DelegateCommand AddRowCommand
        {
            get { return m_addRowCommand ?? (m_addRowCommand = new DelegateCommand(OnAddRow, CanAddRow)); }
        }

        private bool CanAddRow(object parameter)
        {
            return true;
        }

        private void OnAddRow(object parameter)
        {
            var row = new LangTextRow(new LangText {Id = ++m_highestId});
            row.State = RowState.Added;
            m_rows.Add(row);

            m_editor.TextsGrid.SelectedItem = row;
            m_editor.TextsGrid.ScrollIntoView(row);
            m_editor.TextsGrid.Focus();
        }

        #endregion



        #region AddUIRowCommand

        private DelegateCommand m_addUIRowCommand;

        public DelegateCommand AddUIRowCommand
        {
            get { return m_addUIRowCommand ?? (m_addUIRowCommand = new DelegateCommand(OnAddUIRow, CanAddUIRow)); }
        }

        private bool CanAddUIRow(object parameter)
        {
            return true;
        }

        private void OnAddUIRow(object parameter)
        {
            var row = new LangTextUiRow(new LangTextUi());
            row.State = RowState.Added;
            m_rows.Add(row);

            m_editor.TextsGrid.SelectedItem = row;
            m_editor.TextsGrid.ScrollIntoView(row);
            m_editor.TextsGrid.Focus();
        }

        #endregion


        #region RemoveRowCommand

        private DelegateCommand m_removeRowCommand;

        public DelegateCommand RemoveRowCommand
        {
            get { return m_removeRowCommand ?? (m_removeRowCommand = new DelegateCommand(OnRemoveRow, CanRemoveRow)); }
        }

        private bool CanRemoveRow(object parameter)
        {
            return parameter is IList && (parameter as IList).Count > 0;
        }

        private void OnRemoveRow(object parameter)
        {
            if (parameter == null || !CanRemoveRow(parameter))
                return;

            var rows = (parameter as IList).OfType<LangGridRow>().ToArray();
            foreach (var row in rows)
            {
                if (m_rows.Remove(row))
                    m_deletedRows.Push(row);

                row.State = RowState.Removed;
            }
        }

        #endregion


        #region SaveCommand

        private DelegateCommand m_saveCommand;

        public DelegateCommand SaveCommand
        {
            get { return m_saveCommand ?? (m_saveCommand = new DelegateCommand(OnSave, CanSave)); }
        }

        private static bool CanSave(object parameter)
        {
            return true;
        }

        private void OnSave(object parameter)
        {
            foreach (var row in m_rows)
            {
                row.Save();
            }

            foreach (var row in m_deletedRows)
            {
                row.Save();
            }

            MessageService.ShowMessage(m_editor, "Saved ! ");
        }

        #endregion

    }
}