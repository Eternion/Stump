﻿// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using BrendanGrant.Helpers.FileAssociation;
using Stump.Server.DataProvider.Data.MapTool;
using Stump.Tools.DataLoader.Properties;

namespace Stump.Tools.DataLoader
{
    public partial class FormD2P : Form, IFormAdapter
    {
        #region WIN 32 

        public const uint SHGFI_ICON = 0x100;
        public const uint SHGFI_LARGEICON = 0x0; // 'Large icon
        public const uint SHGFI_SMALLICON = 0x1; // 'Small icon

        [DllImport("shell32.dll")]
        private static extern IntPtr SHGetFileInfo(string pszPath,
                                                   uint dwFileAttributes,
                                                   ref SHFILEINFO psfi,
                                                   uint cbSizeFileInfo,
                                                   uint uFlags);

        #endregion

        private static readonly string[] Extensions = FileAssociationInfo.GetExtensions();
        private static readonly Dictionary<string, Icon> AssociatedIcon = new Dictionary<string, Icon>();

        private static Icon DirectoryIcon;

        public FormD2P(D2PAdapter adapter)
        {
            InitializeComponent();

            Adapter = adapter;
            CurrentPath = "./";
            m_buttonExtractAll.Enabled = true;
        }

        public D2PAdapter Adapter
        {
            get;
            set;
        }

        public PakedDirectoryContainer BaseDirectory
        {
            get;
            private set;
        }

        public PakedDirectoryContainer CurrentDirectory
        {
            get;
            private set;
        }

        public string CurrentPath
        {
            get;
            private set;
        }

        #region IFormAdapter Members

        IFileAdapter IFormAdapter.Adapter
        {
            get { return Adapter; }
        }

        #endregion

        public void OpenPakFile(IEnumerable<PakFile.PakedFileInfo> files)
        {
            BaseDirectory = new PakedDirectoryContainer
                {
                    Name = "",
                    Path = "./",
                    Directories = new SortedDictionary<string, PakedDirectoryContainer>(),
                    Files = new SortedList<string, PakFile.PakedFileInfo>()
                };

            var directories = new Dictionary<string, PakedDirectoryContainer>();

            foreach (PakFile.PakedFileInfo file in files)
            {
                if (file.Directories.Length > 0)
                {
                    PakedDirectoryContainer currentDir = BaseDirectory;
                    string currentPath = currentDir.Name;
                    for (int i = 0; i < file.Directories.Length; i++)
                    {
                        currentPath += file.Directories[i] + "/";

                        if (!directories.ContainsKey(currentPath))
                        {
                            directories.Add(currentPath, new PakedDirectoryContainer
                                {
                                    Name = Path.GetDirectoryName(currentPath),
                                    Path = currentPath,
                                    Directories = new SortedDictionary<string, PakedDirectoryContainer>(),
                                    Files = new SortedList<string, PakFile.PakedFileInfo>()
                                });

                            currentDir.Directories.Add(currentPath, directories[currentPath]);
                        }

                        currentDir = currentDir.Directories[currentPath];

                        if (i == file.Directories.Length - 1)
                            currentDir.Files.Add(Path.GetFileName(file.Name), file);
                    }
                }
                else
                    BaseDirectory.Files.Add(Path.GetFileName(file.Name), file);
            }

            ExploreDirectory(BaseDirectory);
        }

        private void ExploreDirectory(PakedDirectoryContainer directory)
        {
            if (DirectoryIcon == null)
                LoadDirectoriesIcon();

            m_iconFilesList.Images.Clear();
            m_filesView.Items.Clear();

            int index = 0;

            if (directory != BaseDirectory)
            {
                directory.Container = CurrentDirectory;

                m_iconFilesList.Images.Add(DirectoryIcon);

                m_filesView.Items.Add(new ListViewItem(new[] {".."}, index)
                    {
                        Tag = directory.Container
                    });

                index++;
            }

            CurrentDirectory = directory;

            foreach (var directoryFile in directory.Directories)
            {
                m_iconFilesList.Images.Add(DirectoryIcon);

                m_filesView.Items.Add(new ListViewItem(new[] {directoryFile.Value.Name}, index)
                    {
                        Tag = directoryFile.Value
                    });

                index++;
            }

            foreach (PakFile.PakedFileInfo file in CurrentDirectory.Files.Values)
            {
                string ext = Path.GetExtension(file.Name);

                if (!AssociatedIcon.ContainsKey(ext))
                {
                    if (Extensions.Count(entry => entry == ext) == 0)
                    {
                        AssociatedIcon.Add(ext, Icon.FromHandle(Resources.page_white.GetHicon()));
                    }
                    else
                    {
                        var fileInfo = new FileAssociationInfo(ext);
                        var programAssociationInfo = new ProgramAssociationInfo(fileInfo.ProgID);

                        AssociatedIcon.Add(ext, new Icon(programAssociationInfo.DefaultIcon.Path));
                    }
                }

                m_iconFilesList.Images.Add(AssociatedIcon[ext]);

                m_filesView.Items.Add(
                    new ListViewItem(new[] {file.Name, file.Size.ToString(), file.Index.ToString(), file.Container},
                                     index) {Tag = file});

                index++;
            }
        }

        private void ExtractItems(ListView.SelectedListViewItemCollection items)
        {
            if (items.Count == 1)
                ExtractSingleItem(items[0]);
            else
            {
                var dialog = new FolderBrowserDialog
                    {
                        Description = @"Select the directory where the files will be extracted...",
                    };

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    foreach (ListViewItem item in items)
                    {
                        if (item.Tag is PakedDirectoryContainer)
                        {
                            Adapter.ExtractDirectory((item.Tag as PakedDirectoryContainer).Path, dialog.SelectedPath);
                        }
                        else if (item.Tag is PakFile.PakedFileInfo)
                        {
                            Adapter.ExtractFile((item.Tag as PakFile.PakedFileInfo).Name,
                                                Path.Combine(dialog.SelectedPath,
                                                             (item.Tag as PakFile.PakedFileInfo).Name));
                        }
                    }
                }
            }
        }

        private void ExtractSingleItem(ListViewItem item)
        {
            if (item.Tag is PakedDirectoryContainer)
            {
                var dialog = new FolderBrowserDialog
                    {
                        Description = @"Select the directory where the files will be extracted...",
                    };

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    Adapter.ExtractDirectory((item.Tag as PakedDirectoryContainer).Path, dialog.SelectedPath);
                }
            }
            else if (item.Tag is PakFile.PakedFileInfo)
            {
                var dialog = new SaveFileDialog
                    {
                        Title = @"Create the new extracted file",
                        FileName = Path.GetFileName((item.Tag as PakFile.PakedFileInfo).Name),
                    };

                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    Adapter.ExtractFile((item.Tag as PakFile.PakedFileInfo).Name, dialog.FileName);
                }
            }
        }

        private void ContextMenuExtractClick(object sender, EventArgs e)
        {
            ExtractItems(m_filesView.SelectedItems);
        }

        private static void LoadDirectoriesIcon()
        {
            var shinfo = new SHFILEINFO();

            IntPtr img = SHGetFileInfo(Environment.CurrentDirectory, 0, ref shinfo,
                                       (uint) Marshal.SizeOf(shinfo), SHGFI_ICON | SHGFI_SMALLICON);

            DirectoryIcon = Icon.FromHandle(shinfo.hIcon);
        }

        private void FilesViewMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (m_filesView.SelectedItems.Count == 0)
                return;

            ListViewItem item = m_filesView.SelectedItems[0];

            if (item.Tag is PakedDirectoryContainer)
            {
                ExploreDirectory(item.Tag as PakedDirectoryContainer);
            }
        }

        private void FilesViewItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            m_buttonExtract.Enabled = m_filesView.SelectedItems.Count > 0;
            m_contextMenuExtract.Enabled = m_filesView.SelectedItems.Count > 0;
        }

        private void ButtonExtractClick(object sender, EventArgs e)
        {
            ExtractItems(m_filesView.SelectedItems);
        }

        private void ButtonExtractAllClick(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog
                {
                    Description = @"Select the directory where the files will be extracted...",
                };

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                Adapter.Pak.ExtractAll(dialog.SelectedPath);
            }
        }

        #region Nested type: PakedDirectoryContainer

        public class PakedDirectoryContainer
        {
            public string Name
            {
                get;
                set;
            }

            public string Path
            {
                get;
                set;
            }

            public SortedDictionary<string, PakedDirectoryContainer> Directories
            {
                get;
                set;
            }

            public SortedList<string, PakFile.PakedFileInfo> Files
            {
                get;
                set;
            }

            public PakedDirectoryContainer Container
            {
                get;
                set;
            }
        }

        #endregion

        #region Nested type: SHFILEINFO

        [StructLayout(LayoutKind.Sequential)]
        public struct SHFILEINFO
        {
            public IntPtr hIcon;
            public IntPtr iIcon;
            public uint dwAttributes;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
            public string szDisplayName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
            public string szTypeName;
        } ;

        #endregion
    }
}