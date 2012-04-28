using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stump.DofusProtocol.D2oClasses.Tool.D2p;

namespace Stump.Tools.Toolkit.ModelViews.D2P
{
    public class D2PFileNode : D2PTreeViewNode
    {
        public D2PFileNode(D2pEntry file)
        {
            Entry = file;
        }

        public D2PFileNode(D2pEntry file, D2PDirectoryNode parent)
        {
            Entry = file;
            Parent = parent;
        }

        public D2pEntry Entry
        {
            get;
            private set;
        }

        public override string Name
        {
            get { return Entry.FileName; }
        }

        public override bool CanBeExpanded()
        {
            return false;
        }
    }
}