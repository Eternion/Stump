using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Stump.Server.DataProvider.Data.MapTool;
using Stump.Tools.DataLoader.Properties;

namespace Stump.Tools.DataLoader
{
    public class D2PAdapter : IFileAdapter
    {
        public D2PAdapter()
        {
            m_form = new FormD2P(this);

            MenuItem = new ToolStripMenuItem("D2P");
            MenuItem.DropDownItems.Add("Extract All", Resources.arrow_out, ExtractAll);
        }

        private void ExtractAll(object sender, EventArgs e)
        {
            var dialog = new FolderBrowserDialog
            {
                Description = @"Select the directory where the files will be extracted...",
            };

            if (dialog.ShowDialog(m_form) == DialogResult.OK)
            {
                Pak.ExtractAll(dialog.SelectedPath);
            }
        }

        public D2PAdapter(string file)
            : this()
        {
            FileName = file;
        }

        public string FileName
        {
            get;
            private set;
        }

        public PakFile Pak
        {
            get;
            set;
        }

        public string ExtensionSupport
        {
            get { return "d2p"; }
        }

        public ToolStripMenuItem MenuItem
        {
            get;
            private set;
        }

        private FormD2P m_form;
        public Form Form
        {
            get
            {
                return m_form;
            }
        }

        public void Open()
        {
            m_form.Text = Path.GetFileName(FileName);

            Pak = new PakFile(FileName);

            m_form.OpenPakFile(Pak.GetFilesInfo());
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public void ExtractFile(string filePath, string destFile)
        {
            if (!Pak.ExistsFile(filePath))
                throw new FileNotFoundException(string.Format("{0} is not found", filePath));

            Pak.ExtractFile(filePath, destFile);
        }

        public void ExtractDirectory(string directoryName, string destDirectory)
        {
            if (m_form.CurrentDirectory.Path != directoryName && !m_form.CurrentDirectory.Directories.ContainsKey(directoryName))
                throw new FileNotFoundException(string.Format("{0} is not found", directoryName));

            destDirectory = Path.GetFullPath(destDirectory);

            var directory = m_form.CurrentDirectory.Path != directoryName ? m_form.CurrentDirectory.Directories[directoryName] : m_form.CurrentDirectory;

            foreach (var file in GetDirectoryFiles(directory))
            {
                Pak.ExtractFile(file.Name, Path.Combine(destDirectory, file.Name));
            }
        }

        private static IEnumerable<PakFile.PakedFileInfo> GetDirectoryFiles(FormD2P.PakedDirectoryContainer directory)
        {
            var files = directory.Files.Values.ToList();

            foreach (var subDirectory in directory.Directories.Values)
            {
                files.AddRange(GetDirectoryFiles(subDirectory));
            }

            return files;
        }
    }
}