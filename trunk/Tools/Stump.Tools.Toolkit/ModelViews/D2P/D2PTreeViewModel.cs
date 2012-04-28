using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Waf.Applications;
using Stump.DofusProtocol.D2oClasses.Tool.D2p;

namespace Stump.Tools.Toolkit.ModelViews.D2P
{
    public class D2PTreeViewModel : DataModel
    {
        private readonly ObservableCollection<D2PTreeViewNode> m_rootNodes = new ObservableCollection<D2PTreeViewNode>();
        private IEnumerator<D2PTreeViewNode> m_searchEnumerator;

        public D2PTreeViewModel(D2pFile file)
        {
            foreach (var rootDirectory in file.RootDirectories)
            {
                RootNodes.Add(new D2PDirectoryNode(rootDirectory));
            }

            foreach (var entry in file.Entries.Where(entry => entry.Directory == null))
            {
                RootNodes.Add(new D2PFileNode(entry));
            }
        }

        public ObservableCollection<D2PTreeViewNode> RootNodes
        {
            get { return m_rootNodes; }
        }

        private D2PTreeViewNode m_selectedItem;

        public D2PTreeViewNode SelectedItem
        {
            get { return m_selectedItem; }
            set { m_selectedItem = value; }
        }

        private string m_currentSearchText;
        public D2PTreeViewNode PerformSearch(string text)
        {
            if (string.IsNullOrEmpty(m_currentSearchText) || m_currentSearchText != text ||
                m_searchEnumerator == null || !m_searchEnumerator.MoveNext())
            {
                var matches = FindMatches(text, RootNodes);
                m_searchEnumerator = matches.GetEnumerator();
                m_currentSearchText = text;

                if (!m_searchEnumerator.MoveNext())
                    return null;
            }

            var node = m_searchEnumerator.Current;

            if (node == null)
                return null;

            return node;
        }

        private IEnumerable<D2PTreeViewNode> FindMatches(string search, ICollection<D2PTreeViewNode> fromNodes)
        {
            foreach (var node in fromNodes)
            {
                if (node.NameContains(search))
                    yield return node;

                var directory = ( node as D2PDirectoryNode );

                if (directory != null)
                    foreach (var child in FindMatches(search, directory.Childrens))
                    {
                        yield return child;
                    }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}