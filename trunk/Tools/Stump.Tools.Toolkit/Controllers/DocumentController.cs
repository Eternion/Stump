using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Waf.Applications;
using Stump.Tools.Toolkit.Documents;

namespace Stump.Tools.Toolkit.Controllers
{
    public abstract class DocumentController : Controller
    {
        private readonly FileService fileService;


        protected DocumentController(FileService fileService)
        {
            if (fileService == null) { throw new ArgumentNullException("fileService"); }
            
            this.fileService = fileService;
            AddWeakEventListener(fileService, FileServicePropertyChanged);
            AddWeakEventListener(fileService.Documents, DocumentsCollectionChanged);
        }


        protected abstract void OnDocumentAdded(IDocument document);

        protected abstract void OnDocumentRemoved(IDocument document);

        protected abstract void OnActiveDocumentChanged(IDocument activeDocument);

        private void FileServicePropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ActiveDocument") { OnActiveDocumentChanged(fileService.ActiveDocument); }
        }

        private void DocumentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    OnDocumentAdded(e.NewItems.Cast<Document>().Single());
                    break;
                case NotifyCollectionChangedAction.Remove:
                    OnDocumentRemoved(e.OldItems.Cast<Document>().Single());
                    break;
                default:
                    throw new NotSupportedException("This kind of documents collection change is not supported.");
            }
        }
    }
}
