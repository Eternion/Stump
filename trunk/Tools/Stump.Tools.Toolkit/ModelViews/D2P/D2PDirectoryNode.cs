using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;
using Stump.DofusProtocol.D2oClasses.Tool.D2p;
using Stump.Tools.Toolkit.Helpers;

namespace Stump.Tools.Toolkit.ModelViews.D2P
{
    public class D2PDirectoryNode : D2PTreeViewNode
    {
        private ObservableCollection<D2PTreeViewNode> m_childrens;

        public D2PDirectoryNode(D2pDirectory directory)
            : this(directory, null)
        {
        }

        public D2PDirectoryNode(D2pDirectory directory, D2PDirectoryNode parent)
        {
            Directory = directory;
            m_childrens = new ObservableCollection<D2PTreeViewNode>(
                Directory.Directories.Select(entry => new D2PDirectoryNode(entry, this) as D2PTreeViewNode).
                Concat(Directory.Entries.Select(entry => new D2PFileNode(entry, this))));
            Parent = parent;
        }

        public D2pDirectory Directory
        {
            get;
            private set;
        }

        public ObservableCollection<D2PTreeViewNode> Childrens
        {
            get { return m_childrens; }
        }

        public override string Name
        {
            get
            {
                return Directory.Name;
            }
        }

        public override bool CanBeExpanded()
        {
            return true;
        }

        private static BitmapSource FolderIcon = ImageHelper.GetBitmapSource(IconHelper.GetFolderIcon().ToBitmap());
        public override BitmapSource Icon
        {
            get
            {
                return FolderIcon;
            }
        }
    }
}