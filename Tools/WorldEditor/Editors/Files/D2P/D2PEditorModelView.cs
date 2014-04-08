#region License GNU GPL

// D2PEditorModelView.cs
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
using System.Windows.Forms;
using Stump.DofusProtocol.D2oClasses.Tools.D2p;
using WorldEditor.Helpers;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace WorldEditor.Editors.Files.D2P
{
    public class D2PEditorModelView : INotifyPropertyChanged
    {
        private readonly D2PEditor m_editor;
        private readonly D2pFile m_file;


        private ReadOnlyObservableCollection<D2PGridRow> m_readOnylRows;
        private ObservableCollection<D2PGridRow> m_rows = new ObservableCollection<D2PGridRow>();

        public D2PEditorModelView(D2PEditor editor, D2pFile file)
        {
            m_editor = editor;
            m_file = file;
            m_readOnylRows = new ReadOnlyObservableCollection<D2PGridRow>(m_rows);
            Open(null);
        }

        public string Title
        {
            get { return "D2P Editor : " + File.FileName + (CurrentFolder == null ? string.Empty : " - " + CurrentFolder.FullName); }
        }

        public D2pFile File
        {
            get { return m_file; }
        }

        public ReadOnlyObservableCollection<D2PGridRow> Rows
        {
            get { return m_readOnylRows; }
        }

        public D2pDirectory CurrentFolder
        {
            get;
            private set;
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void Open(D2pDirectory directory)
        {
            m_rows.Clear();
            var rows = new List<D2PGridRow>();
            if (directory == null)
            {
                rows.AddRange(m_file.RootDirectories.Select(rootDirectory => new D2PFolderRow(rootDirectory)));
                rows.AddRange(
                    m_file.Entries.Where(entry => entry.Directory == null).Select(entry => new D2PFileRow(entry)));
            }
            else
            {
                rows.Add(new D2PLastFolderRow(directory.Parent));
                rows.AddRange(directory.Directories.Select(subFolder => new D2PFolderRow(subFolder.Value)));
                rows.AddRange(directory.Entries.Select(entry => new D2PFileRow(entry)));
            }

            CurrentFolder = directory;
            m_readOnylRows =
                new ReadOnlyObservableCollection<D2PGridRow>(m_rows = new ObservableCollection<D2PGridRow>(rows));
            OnPropertyChanged("Rows");
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(name));
        }

        #region ExploreFolderCommand

        private DelegateCommand m_exploreFolderCommand;

        public DelegateCommand ExploreFolderCommand
        {
            get
            {
                return m_exploreFolderCommand ??
                       (m_exploreFolderCommand = new DelegateCommand(OnExploreFolder, CanExploreFolder));
            }
        }

        private bool CanExploreFolder(object parameter)
        {
            return parameter is D2PFolderRow;
        }

        private void OnExploreFolder(object parameter)
        {
            if (parameter == null || !CanExploreFolder(parameter))
                return;

            var folderRow = (D2PFolderRow) parameter;

            Open(folderRow.Folder);
        }

        #endregion

        #region AddFileCommand

        private DelegateCommand m_addFileCommand;

        public DelegateCommand AddFileCommand
        {
            get { return m_addFileCommand ?? (m_addFileCommand = new DelegateCommand(OnAddFile, CanAddFile)); }
        }

        private bool CanAddFile(object parameter)
        {
            return true;
        }

        private void OnAddFile(object parameter)
        {
            if (!CanAddFile(parameter))
                return;

            var dialog = new OpenFileDialog
                {
                    Filter = "*|*",
                    CheckPathExists = true,
                    Title = "Select the files to add",
                    Multiselect = true
                };

            if (dialog.ShowDialog() != true)
                return;

            foreach (var filePath in dialog.FileNames)
            {
                AddFile(filePath);
            }
        }

        public void AddFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                MessageService.ShowError(m_editor, string.Format("Cannot add file : File {0} not found", filePath));
                return;
            }

            string packedFileName = CurrentFolder != null
                                        ? Path.Combine(CurrentFolder.FullName, Path.GetFileName(filePath))
                                        : Path.GetFileName(filePath);

            D2pEntry entry = File.AddFile(packedFileName, System.IO.File.ReadAllBytes(filePath));

            var row = new D2PFileRow(entry);
            m_rows.Add(row);
            m_editor.FilesGrid.ScrollIntoView(row);
        }

        #endregion

        #region RemoveFileCommand

        private DelegateCommand m_removeFileCommand;

        public DelegateCommand RemoveFileCommand
        {
            get { return m_removeFileCommand ?? (m_removeFileCommand = new DelegateCommand(OnRemoveFile, CanRemoveFile)); }
        }

        private bool CanRemoveFile(object parameter)
        {
            return parameter != null && parameter is IList &&
                ( parameter as IList ).OfType<D2PGridRow>().Count(x => !( x is D2PLastFolderRow )) > 0;
        }

        private void OnRemoveFile(object parameter)
        {
            if (parameter == null || !CanRemoveFile(parameter))
                return;

            var rows = ( parameter as IList ).OfType<D2PGridRow>().ToArray();
            foreach (var row in rows)
            {
                if (row is D2PFileRow)
                {
                    var fileRow = row as D2PFileRow;
                    if (File.RemoveEntry(fileRow.Entry))
                    {
                        m_rows.Remove(fileRow);
                    }
                    else
                    {
                        MessageService.ShowError(m_editor, "Cannot remove this file. Unknown reason");
                        return;
                    }
                }
                else if (row is D2PFolderRow)
                {
                    var folderRow = row as D2PFolderRow;
                    if (folderRow.Folder.Entries.Count(entry => !File.RemoveEntry(entry)) > 0)
                    {
                        MessageService.ShowError(m_editor, "Some files cannot be removed. Unknow reason.");
                        return;
                    }
                    else
                    {
                        m_rows.Remove(folderRow);
                    }
                }
            }
         
        }

        #endregion


        #region ExtractCommand

        private DelegateCommand m_extractCommand;

        public DelegateCommand ExtractCommand
        {
            get { return m_extractCommand ?? (m_extractCommand = new DelegateCommand(OnExtract, CanExtract)); }
        }

        private bool CanExtract(object parameter)
        {
            return parameter != null && parameter is IList &&
                ( parameter as IList ).OfType<D2PGridRow>().Count(x => !( x is D2PLastFolderRow )) > 0;
        }

        private void OnExtract(object parameter)
        {
            if (parameter == null || !CanExtract(parameter))
                return;

            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                Description = "Select a folder where to extract these files ..."
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var rows = ( parameter as IList ).OfType<D2PGridRow>().ToArray();
                foreach (var row in rows)
                {
                    if (row is D2PFileRow)
                        File.ExtractFile(( row as D2PFileRow ).Entry.FullFileName, dialog.SelectedPath);
                    else if (row is D2PFolderRow)
                        File.ExtractDirectory(( row as D2PFolderRow ).Folder.FullName, dialog.SelectedPath);
                }

                MessageService.ShowMessage(m_editor, "Files extracted");
            }
        }

        #endregion


        #region ExtractAllCommand

        private DelegateCommand m_extractAllCommand;

        public DelegateCommand ExtractAllCommand
        {
            get { return m_extractAllCommand ?? (m_extractAllCommand = new DelegateCommand(OnExtractAll, CanExtractAll)); }
        }

        private bool CanExtractAll(object parameter)
        {
            return true;
        }

        private void OnExtractAll(object parameter)
        {
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                Description = "Select a folder where to extract these files ..."
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                File.ExtractAllFiles(dialog.SelectedPath);
            }


            MessageService.ShowMessage(m_editor, "All files extracted");
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
            try
            {
                File.Save();

                MessageService.ShowMessage(m_editor, string.Format("{0} saved !", File.FileName));
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
                FileName = File.FilePath,
                Title = "Save file as ...",
                DefaultExt = ".d2p",
                OverwritePrompt = true,
            };

            if (dialog.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                string filePath = dialog.FileName;

                File.SaveAs(filePath);
                OnPropertyChanged("Title");

                MessageService.ShowMessage(m_editor, string.Format("{0} saved !", File.FileName));
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

        #endregion

        public void Dispose()
        {
            File.Dispose();
        }
    }
}