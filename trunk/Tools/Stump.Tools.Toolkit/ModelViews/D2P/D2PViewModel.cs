using System;
using System.ComponentModel;
using System.IO;
using System.Waf.Applications;
using System.Windows.Forms;
using System.Windows.Input;
using Stump.Tools.Toolkit.Documents;
using Stump.Tools.Toolkit.Views;

namespace Stump.Tools.Toolkit.ModelViews.D2P
{
    public class D2PViewModel : StatusViewModel<D2PView>
    {
        public D2PViewModel(D2PView view, D2PDocument document) : base(view)
        {
            Document = document;
            TreeViewModel = new D2PTreeViewModel(Document.File);
            SearchCommand = new DelegateCommand(PerformSearch);
            ExtractCommand = new DelegateCommand(ExtractSelection, () => TreeViewModel.SelectedItem != null);
            ExtractAllCommand = new DelegateCommand(ExtractAll);
            AddFileCommand = new DelegateCommand(AddFile);
            RemoveFileCommand = new DelegateCommand(RemoveFile, () => TreeViewModel.SelectedItem != null);
            SaveCommand = new DelegateCommand(Save);
            SaveAsCommand = new DelegateCommand(SaveAs);

            AddWeakEventListener(TreeViewModel, TreeViewModelPropertyChanged);
        }

        private void TreeViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "SelectedItem")
            {
                ExtractCommand.RaiseCanExecuteChanged();
                RemoveFileCommand.RaiseCanExecuteChanged();
            }
        }

        public D2PDocument Document
        {
            get;
            private set;
        }

        public string SearchText
        {
            get;
            set;
        }

        public D2PTreeViewModel TreeViewModel
        {
            get;
            private set;
        }

        public DelegateCommand SearchCommand
        {
            get;
            private set;
        }

        public DelegateCommand ExtractCommand
        {
            get;
            private set;
        }

        public DelegateCommand ExtractAllCommand
        {
            get;
            private set;
        }

        public DelegateCommand AddFileCommand
        {
            get;
            private set;
        }

        public DelegateCommand RemoveFileCommand
        {
            get;
            private set;
        }

        public DelegateCommand SaveCommand
        {
            get;
            private set;
        }
        public DelegateCommand SaveAsCommand
        {
            get;
            private set;
        }

        public void PerformSearch()
        {
            SetStatus("Perform Search");
            D2PTreeViewNode node = TreeViewModel.PerformSearch(SearchText);

            if (node != null)
            {
                ViewCore.SelectNode(node);
                SetStatus(string.Format("Found {0}", node.Name));
            }
            else
            {
                SetStatus(string.Format("{0} not found", SearchText));
            }
        }

        public void ExtractSelection()
        {
            SetStatus("Extracing file ...");
            var dialog = new FolderBrowserDialog
                             {
                                 ShowNewFolderButton = true,
                                 Description = "Select a folder where to extract these files ..."
                             };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                if (TreeViewModel.SelectedItem is D2PFileNode)
                    Document.File.ExtractFile((TreeViewModel.SelectedItem as D2PFileNode).Entry.FullFileName, dialog.SelectedPath);
                else if (TreeViewModel.SelectedItem is D2PDirectoryNode)
                    Document.File.ExtractDirectory(( TreeViewModel.SelectedItem as D2PDirectoryNode ).Directory.FullName, dialog.SelectedPath);

            }

            ResetStatus();
        }

        public void ExtractAll()
        {
            SetStatus("Extracing all files ...");
            var dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = true,
                Description = "Select a folder where to extract these files ..."
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                Document.File.ExtractAllFiles(dialog.SelectedPath);
            }

            ResetStatus();
        }

        public void AddFile()
        {
            SetStatus("Adding files ...");
            var dialog = new OpenFileDialog
            {
                Filter = "*|*",
                CheckPathExists = true,
                Title = "Select the files to add",
                Multiselect = true
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string fileName in dialog.SafeFileNames)
                {
                    D2PDirectoryNode directory = null;
                    string path = string.Empty;
                    if (TreeViewModel.SelectedItem != null)
                    {
                        directory = ( TreeViewModel.SelectedItem is D2PDirectoryNode ) ?
                            ( TreeViewModel.SelectedItem as D2PDirectoryNode ) : ( TreeViewModel.SelectedItem as D2PFileNode ).Parent;

                        path = directory.Directory.FullName;
                    }

                    bool add = true;
                    if (Document.File.Exists(Path.Combine(path, Path.GetFileName(fileName))))
                        add = MessageBox.Show("Overwrite existing file ?", "Overwrite", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

                    if (add)
                    {
                        var entry = Document.File.AddFile(Path.Combine(path, Path.GetFileName(fileName)),
                                                          File.ReadAllBytes(fileName));
                        if (directory != null)
                            directory.Childrens.Add(new D2PFileNode(entry, directory));
                        else
                            TreeViewModel.RootNodes.Add(new D2PFileNode(entry));
                    }
                }
            }

            ResetStatus();
        }

        public void RemoveFile()
        {
            var item = TreeViewModel.SelectedItem;

            if (item != null)
            {
                if (item is D2PDirectoryNode)
                {
                    foreach (var entry in (item as D2PDirectoryNode).Directory.Entries.ToArray())
                    {
                        Document.File.RemoveEntry(entry);
                    }
                }
                else if (item is D2PFileNode)
                {
                    Document.File.RemoveEntry(( item as D2PFileNode ).Entry);
                }

                if (item.Parent != null)
                    item.Parent.Childrens.Remove(item);
                else
                    TreeViewModel.RootNodes.Remove(item);
            }
        }

        public void Save()
        {
            
        }

        public void SaveAs()
        {
          
        }
    }
}