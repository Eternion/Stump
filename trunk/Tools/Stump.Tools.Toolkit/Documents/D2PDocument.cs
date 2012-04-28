using System;
using Stump.DofusProtocol.D2oClasses.Tool.D2p;

namespace Stump.Tools.Toolkit.Documents
{
    public class D2PDocument : Document
    {
        public D2PDocument(IDocumentType documentType)
            : base(documentType)
        {
        }

        public D2PDocument(IDocumentType documentType, D2pFile file)
            : base(documentType)
        {
            File = file;
        }

        public D2pFile File
        {
            get;
            set;
        }

        public override string FileName
        {
            get
            {
                return File.FilePath;
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}