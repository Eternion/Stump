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
using System.Windows.Forms;
using Stump.DofusProtocol.D2oClasses.Tools.D2i;
using WorldEditor.D2P;
using WorldEditor.Helpers;

namespace WorldEditor.D2I
{
    public class D2IEditorModelView : INotifyPropertyChanged
    {
        private readonly D2IEditor m_editor;
        private readonly D2IFile m_file;
        private int m_highestId;
        private Stack<D2IGridRow> m_deletedRows = new Stack<D2IGridRow>();


        private ReadOnlyObservableCollection<D2IGridRow> m_readOnylRows;
        private ObservableCollection<D2IGridRow> m_rows = new ObservableCollection<D2IGridRow>();
        public D2IEditorModelView(D2IEditor editor, D2IFile file)
        {
            m_editor = editor;
            m_file = file;
            m_readOnylRows = new ReadOnlyObservableCollection<D2IGridRow>(m_rows);
            LastFoundIndex = -1;
            Open();
        }

        public ReadOnlyObservableCollection<D2IGridRow> Rows
        {
            get
            {
                return m_readOnylRows;
            }
        }

        public string Title
        {
            get { return "D2I Editor : " + Path.GetFileName(m_file.FilePath); }
        }

        private void Open()
        {
            var uiTexts = m_file.GetAllUiText();
            foreach (var uiText in uiTexts)
            {
                m_rows.Add(new D2ITextUiRow(uiText.Key, uiText.Value));
            }

            var texts = m_file.GetAllText();
            foreach (var text in texts)
            {
                m_rows.Add(new D2ITextRow(text.Key, text.Value));
            }
            m_highestId = texts.Keys.Max();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private D2IGridRow FindNext()
        {
            int startIndex = LastFoundIndex == -1 || LastFoundIndex + 1 >= Rows.Count ? 0 : LastFoundIndex + 1;

            D2IGridRow row = null;
            int index = -1;
            if (SearchType == "Key")
            {
                var isNumber = SearchText.All(char.IsDigit);
                if (isNumber)
                {
                    int id = int.Parse(SearchText);
                    for (int i = startIndex; i < Rows.Count; i++)
                    {
                        if (Rows[i] is D2ITextRow && ( Rows[i] as D2ITextRow ).Id == id)
                        {
                            row = Rows[i];
                            index = i;
                            break;
                        }
                    }
                }

                if (row == null)
                {
                    for (int i = startIndex; i < Rows.Count; i++)
                    {
                        if (Rows[i].GetKey().IndexOf(SearchText, StringComparison.InvariantCultureIgnoreCase) != -1)
                        {
                            row = Rows[i];
                            index = i;
                            break;
                        }
                    }
                }
            }
            else if (SearchType == "Text")
            {
                for (int i = startIndex; i < Rows.Count; i++)
                {
                    if (Rows[i].Text.IndexOf(SearchText, StringComparison.InvariantCultureIgnoreCase) != -1)
                    {
                        row = Rows[i];
                        index = i;
                        break;
                    }
                }
            }

            if (row == null)
            {
                LastFoundIndex = -1;
                return null;
            }
            else
            {
                LastFoundIndex = index;
                return row;
            }
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
            var row = new D2ITextRow(++m_highestId, string.Empty);
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
            var row = new D2ITextUiRow(string.Empty, string.Empty);
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

            var rows = (parameter as IList).OfType<D2IGridRow>().ToArray();
            foreach (var row in rows)
            {
                if (m_rows.Remove(row))
                    m_deletedRows.Push(row);

                row.State = RowState.Removed;
            }
        }

        #endregion



        #region ConvertToTxtCommand

        private DelegateCommand m_convertToTxtCommand;

        public DelegateCommand ConvertToTxtCommand
        {
            get { return m_convertToTxtCommand ?? (m_convertToTxtCommand = new DelegateCommand(OnConvertToTxt, CanConvertToTxt)); }
        }

        private bool CanConvertToTxt(object parameter)
        {
            return true;
        }

        private void OnConvertToTxt(object parameter)
        {
            var dialog = new SaveFileDialog
            {
                FileName = Path.GetFileNameWithoutExtension(m_file.FilePath) + ".txt",
                InitialDirectory = Path.GetDirectoryName(m_file.FilePath),
                Title = "Convert to ...",
                DefaultExt = ".txt",
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                OverwritePrompt = true,
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            string filePath = dialog.FileName;
            var uiPadding = Rows.OfType<D2ITextUiRow>().Max(x => x.GetKey().Length + 1);
            var textPadding = Rows.OfType<D2ITextRow>().Max(x => x.GetKey().Length + 1);

            using (var stream = File.Open(filePath, FileMode.Append, FileAccess.Write))
            {
                var writer = new StreamWriter(stream);
                foreach (var row in Rows)
                {
                    int padding = row is D2ITextRow ? textPadding : uiPadding;

                    writer.WriteLine("{0}{1}", row.GetKey().PadRight(padding), row.Text.Replace("\n", "\n" + new string(' ', padding)));
                }
            }

            MessageService.ShowMessage(m_editor, string.Format("File converted to {0}", Path.GetFileName(filePath)));
        }

        #endregion


        #region SaveCommand

        private DelegateCommand m_saveCommand;

        public DelegateCommand SaveCommand
        {
            get { return m_saveCommand ?? (m_saveCommand = new DelegateCommand(OnSave, CanSave)); }
        }

        private bool CanSave(object parameter)
        {
            return true;
        }

        private void OnSave(object parameter)
        {
            PerformSave(m_file.FilePath);
        }

        #endregion


        #region SaveAsCommand

        private DelegateCommand m_saveAsCommand;

        public DelegateCommand SaveAsCommand
        {
            get { return m_saveAsCommand ?? (m_saveAsCommand = new DelegateCommand(OnSaveAs, CanSaveAs)); }
        }

        private bool CanSaveAs(object parameter)
        {
            return true;
        }

        private void OnSaveAs(object parameter)
        {
            var dialog = new SaveFileDialog
            {
                FileName = m_file.FilePath,
                Title = "Save file as ...",
                DefaultExt = ".d2i",
                OverwritePrompt = true,
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            string filePath = dialog.FileName;

            PerformSave(filePath);
        }

        #endregion

        private void PerformSave(string filePath)
        {
            try
            {
                if (!UpdateFile())
                    return;

                m_file.Save(filePath);

                MessageService.ShowMessage(m_editor, "File saved successfully");

            }
            catch (IOException ex)
            {
                MessageService.ShowError(m_editor, "Cannot perform save : " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageService.ShowError(m_editor, "Cannot perform save : " + ex);
            }
        }

        private bool UpdateFile()
        {
            foreach (var row in Rows)
            {
                if (row.State == RowState.Dirty || row.State == RowState.Added)
                {
                    if (Rows.Count(x => x.GetKey() == row.GetKey()) > 1)
                    {
                        if (!MessageService.ShowYesNoQuestion(m_editor, string.Format("WARNING ! Found duplicated keys '{0}'. The file may save but a row will be deleted", row.GetKey()) +
                            "Continue saving anyway ?"))
                        {
                            SearchType = "Key";
                            SearchText = row.GetKey();
                            OnFindNext(null);
                            return false;
                        }
                    }

                    if (row is D2ITextRow)
                        m_file.SetText(( row as D2ITextRow ).Id, row.Text);
                    if (row is D2ITextUiRow)
                        m_file.SetText(( row as D2ITextUiRow ).Id, row.Text);

                    row.State = RowState.None;
                }
            }

            while (m_deletedRows.Count > 0)
            {
                var row = m_deletedRows.Pop();
                if (row is D2ITextRow)
                    m_file.DeleteText(( row as D2ITextRow ).Id);
                if (row is D2ITextUiRow)
                    m_file.DeleteText(( row as D2ITextUiRow ).Id);
            }

            return true;
        }
    }
}