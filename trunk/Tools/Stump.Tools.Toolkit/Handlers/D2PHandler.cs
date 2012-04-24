using System.IO;
using System.Windows;

namespace Stump.Tools.Toolkit.Handlers
{
    [FileHandler("d2p")]
    public class D2PHandler : IFileHandler
    {
        public bool Process(Stream file)
        {
            MessageBox.Show("Process a file");
            return true;
        }
    }
}