using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using Stump.Tools.Toolkit.Documents;

namespace Stump.Tools.Toolkit.Controllers.Documents
{
    [Export]
    public class D2PDocumentController : DocumentController
    {
        private readonly CompositionContainer m_container;
        private readonly FileService m_fileService;

        [ImportingConstructor]
        public D2PDocumentController(CompositionContainer container, FileService fileService)
            : base(fileService)
        {
            m_container = container;
            m_fileService = fileService;
        }

        public D2PDocumentController(FileService fileService) : base(fileService)
        {
        }

        protected override void OnDocumentAdded(IDocument document)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnDocumentRemoved(IDocument document)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnActiveDocumentChanged(IDocument activeDocument)
        {
            throw new System.NotImplementedException();
        }
    }
}