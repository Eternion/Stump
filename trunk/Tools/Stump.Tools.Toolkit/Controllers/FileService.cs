using System;
using System.Waf.Applications;
using System.ComponentModel.Composition;
using Stump.Tools.Toolkit.Documents;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Stump.Tools.Toolkit.Controllers
{
    [Export(typeof(FileService)), Export]
    public class FileService : DataModel
    {
        private readonly ObservableCollection<IDocument> documents;
        private readonly ReadOnlyObservableCollection<IDocument> readOnlyDocuments;
        private IDocument activeDocument;
        private RecentFileList recentFileList;
        private ICommand newCommand;
        private ICommand openCommand;
        private ICommand closeCommand;
        private ICommand saveCommand;
        private ICommand saveAsCommand;


        [ImportingConstructor]
        public FileService()
        {
            this.documents = new ObservableCollection<IDocument>();
            this.readOnlyDocuments = new ReadOnlyObservableCollection<IDocument>(documents);
        }


        public ReadOnlyObservableCollection<IDocument> Documents { get { return readOnlyDocuments; } }

        public IDocument ActiveDocument
        {
            get { return activeDocument; }
            set
            {
                if (activeDocument != value)
                {
                    if (value != null && !documents.Contains(value))
                    {
                        throw new ArgumentException("value is not an item of the Documents collection.");
                    }
                    activeDocument = value;
                }
            }
        }

        public RecentFileList RecentFileList
        {
            get { return recentFileList; }
            set
            {
                if (recentFileList != value)
                {
                    recentFileList = value;
                }
            }
        }

        public ICommand NewCommand
        {
            get { return newCommand; }
            set
            {
                if (newCommand != value)
                {
                    newCommand = value;
                }
            }
        }

        public ICommand OpenCommand
        {
            get { return openCommand; }
            set
            {
                if (openCommand != value)
                {
                    openCommand = value;
                }
            }
        }

        public ICommand CloseCommand
        {
            get { return closeCommand; }
            set
            {
                if (closeCommand != value)
                {
                    closeCommand = value;
                }
            }
        }

        public ICommand SaveCommand
        {
            get { return saveCommand; }
            set
            {
                if (saveCommand != value)
                {
                    saveCommand = value;
                }
            }
        }

        public ICommand SaveAsCommand
        {
            get { return saveAsCommand; }
            set
            {
                if (saveAsCommand != value)
                {
                    saveAsCommand = value;
                }
            }
        }


        public void AddDocument(IDocument document)
        {
            documents.Add(document);
        }

        public void RemoveDocument(IDocument document)
        {
            documents.Remove(document);
        }
    }
}
