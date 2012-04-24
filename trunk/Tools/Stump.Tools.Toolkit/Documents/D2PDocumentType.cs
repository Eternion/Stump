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

        public override bool CanSave(Document document)
        {
            return document is D2PDocument;
        }

    }
}