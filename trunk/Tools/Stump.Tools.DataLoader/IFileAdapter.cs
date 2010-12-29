using System.Windows.Forms;

namespace Stump.Tools.DataLoader
{
    public interface IFileAdapter
    {
        string FileName
        {
            get;
        }

        string ExtensionSupport
        {
            get;
        }

        ToolStripMenuItem MenuItem
        {
            get;
        }

        Form Form
        {
            get;
        }

        void Open();
        void Save();
    }
}