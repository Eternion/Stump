using System.ComponentModel;
using System.Waf.Applications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Stump.Tools.Toolkit.Documents
{
    public abstract class Document : DataModel, IDocument
    {
        private readonly IDocumentType documentType;


        protected Document(IDocumentType documentType)
        {
            Assert.IsNotNull(documentType);
            this.documentType = documentType;
        }


        public IDocumentType DocumentType { get { return documentType; } }

        public virtual string FileName
        {
            get;
            set;
        }

        public virtual bool Modified
        {
            get;
            set;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
