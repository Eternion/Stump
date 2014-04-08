#region License GNU GPL
// MetaEditorModelView.cs
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

using System.IO;
using System.Linq;
using Microsoft.Win32;
using Stump.Core.Cryptography;
using Stump.Core.Extensions;
using WorldEditor.Helpers;

namespace WorldEditor.Meta
{
    public class MetaEditorModelView
    {
        private readonly MetaEditor m_editor;
        private readonly MetaFile m_file;

        public MetaEditorModelView(MetaEditor editor, MetaFile file)
        {
            m_editor = editor;
            m_file = file;
        }

        public MetaFile File
        {
            get { return m_file; }
        }


        #region UpdateCommand

        private DelegateCommand m_updateCommand;

        public DelegateCommand UpdateCommand
        {
            get { return m_updateCommand ?? (m_updateCommand = new DelegateCommand(OnUpdate, CanUpdate)); }
        }

        private bool CanUpdate(object parameter)
        {
            return true;
        }

        private void OnUpdate(object parameter)
        {
            int count = 0;
            // use copy to remove entries
            foreach (var entry in File.Entries.ToArray())
            {
                var dir = Path.GetDirectoryName(File.FilePath);
                var filePath = Path.Combine(dir, entry.FileName);

                if (!System.IO.File.Exists(filePath))
                {
                    if (MessageService.ShowYesNoQuestion(m_editor, "WARNING !! File {0} not found, remove it from the meta file ?"))
                        File.Entries.Remove(entry);
                }
                else
                {
                    var md5 = Cryptography.GetFileMD5Hash(filePath);
                    if (md5 != entry.MD5)
                        count++;

                    entry.MD5 = md5;
                }
            }

            MessageService.ShowMessage(m_editor,
                                       count > 0 ? string.Format("{0} files modified !", count) : "No file modified");
        }

        #endregion


        #region AddCommand

        private DelegateCommand m_addCommand;

        public DelegateCommand AddCommand
        {
            get { return m_addCommand ?? (m_addCommand = new DelegateCommand(OnAdd, CanAdd)); }
        }

        private bool CanAdd(object parameter)
        {
            return true;
        }

        private void OnAdd(object parameter)
        {
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

        public void AddFile(string file)
        {
            if (!System.IO.File.Exists(file))
                throw new FileNotFoundException(file);

            var md5 = System.IO.File.ReadAllText(file).GetMD5();
            File.Entries.Add(new MetaFileEntry(Path.GetFileName(file), md5));
        }

        #endregion


        #region RemoveCommand

        private DelegateCommand m_removeCommand;

        public DelegateCommand RemoveCommand
        {
            get { return m_removeCommand ?? (m_removeCommand = new DelegateCommand(OnRemove, CanRemove)); }
        }

        private bool CanRemove(object parameter)
        {
            return parameter is MetaFileEntry;
        }

        private void OnRemove(object parameter)
        {
            if (parameter == null || !CanRemove(parameter))
                return;

            File.Entries.Remove(parameter as MetaFileEntry);
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
            File.Save();

            MessageService.ShowMessage(m_editor, "File saved successfully !");
        }

        #endregion
    }
}