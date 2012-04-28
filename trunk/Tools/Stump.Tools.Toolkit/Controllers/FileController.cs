using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Waf.Applications;
using Stump.Core.Reflection;
using Stump.Tools.Toolkit.Documents;

namespace Stump.Tools.Toolkit.Controllers
{
    [Export]
    public class FileController : Controller
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly List<DocumentType> m_documentTypes = new List<DocumentType>();
        private readonly ObservableCollection<IDocument> m_documents;

        public FileController()
        {
            
        }

        public void Initialize()
        {
            var documentTypes = ( from entry in Assembly.GetExecutingAssembly().GetTypes()
                                  where entry.IsSubclassOf(typeof(DocumentType)) && !entry.IsAbstract
                                  select entry );

            m_documentTypes.AddRange(documentTypes.Select(entry => Activator.CreateInstance(entry) as DocumentType));
        }

        private IDocument m_activeDocument;

        public IDocument ActiveDocument
        {
            get { return m_activeDocument; }
            set
            {
                if (value != m_activeDocument)
                {
                    if (value != null && !m_documents.Contains(value))
                    {
                        throw new ArgumentException("value is not an item of the Documents collection.");
                    }

                    m_activeDocument = value;
                }
            }
        }

        public IDocument Open(string file)
        {
            return m_documentTypes[0].Open(file);
        }

    }
}