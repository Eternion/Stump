using System;
using System.ComponentModel;

namespace Stump.Tools.Toolkit.Documents
{
    public class DocumentType : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public DocumentType(string fileExt, string description)
        {
            if (fileExt[0] != '.')
            {
                throw new ArgumentException("The argument fileExtension must start with the '.' character.");
            }

            FileExt = fileExt;
            Description = description;
        }

        public string FileExt
        {
            get;
            private set;
        }

        public string Description
        {
            get;
            private set;
        }

        public virtual bool CanNew()
        {
            return false;
        }

        public Document New()
        {
            if (!CanNew())
            {
                throw new NotSupportedException("The New operation is not supported. CanNew returned false.");
            }

            return NewCore();
        }

        public virtual bool CanOpen()
        {
            return false;
        }

        public Document Open(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("fileName must not be null or empty.");
            }
            if (!CanOpen())
            {
                throw new NotSupportedException("The Open operation is not supported. CanOpen returned false.");
            }

            Document document = OpenCore(fileName);
            if (document != null)
            {
                document.FileName = fileName;
            }
            return document;
        }

        public virtual bool CanSave(Document document)
        {
            return false;
        }

        public void Save(Document document, string fileName)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }
            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("fileName must not be null or empty.");
            }
            if (!CanSave(document))
            {
                throw new NotSupportedException("The Save operation is not supported. CanSave returned false.");
            }

            SaveCore(document, fileName);

            if (CanOpen())
            {
                document.FileName = fileName;
                document.Modified = false;
            }
        }

        protected virtual Document NewCore()
        {
            throw new NotSupportedException();
        }

        protected virtual Document OpenCore(string fileName)
        {
            throw new NotSupportedException();
        }

        protected virtual void SaveCore(Document document, string fileName)
        {
            throw new NotSupportedException();
        }
    }
}