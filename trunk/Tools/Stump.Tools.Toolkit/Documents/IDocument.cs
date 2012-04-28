using System.ComponentModel;

namespace Stump.Tools.Toolkit.Documents
{
    public interface IDocument : INotifyPropertyChanged
    {
        IDocumentType DocumentType { get; }

        string FileName { get; set; }

        bool Modified { get; set; }
    }
}
