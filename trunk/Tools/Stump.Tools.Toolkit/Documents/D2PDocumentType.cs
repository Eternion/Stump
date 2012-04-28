using Stump.DofusProtocol.D2oClasses.Tool.D2p;

namespace Stump.Tools.Toolkit.Documents
{
    public class D2PDocumentType : DocumentType
    {
        public D2PDocumentType()
            : base(".d2p", "Dofus Package File (*.d2p)")
        {
        }

        public override bool CanOpen()
        {
            return true;
        }

        public override bool CanSave(IDocument document)
        {
            return document is D2PDocument;
        }

        public override bool CanNew()
        {
            return true;
        }

        protected override IDocument NewCore()
        {
            return new D2PDocument(this, new D2pFile());
        }

        protected override IDocument OpenCore(string fileName)
        {
            return new D2PDocument(this, new D2pFile(fileName));
        }

        protected override void SaveCore(IDocument document, string fileName)
        {
            (document as D2PDocument).File.SaveAs(fileName);
        }
    }
}