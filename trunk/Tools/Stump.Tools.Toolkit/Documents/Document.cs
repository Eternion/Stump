using System.ComponentModel;

namespace Stump.Tools.Toolkit.Documents
{
    public abstract class Document : INotifyPropertyChanged
    {
        private readonly DocumentType m_documentType;
        private string m_fileName;
        private bool m_modified;

        public Document(DocumentType documentType)
        {
            m_documentType = documentType;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public DocumentType DocumentType
        {
            get { return m_documentType; }
        }

        public string FileName
        {
            get { return m_fileName; }
            set { m_fileName = value; }
        }

        public bool Modified
        {
            get { return m_modified; }
            set { m_modified = value; }
        }
    }
}