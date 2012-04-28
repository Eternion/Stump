using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using Stump.Tools.Toolkit.Helpers;

namespace Stump.Tools.Toolkit.ModelViews.D2P
{
    public abstract class D2PTreeViewNode : INotifyPropertyChanged
    {
        private static readonly Dictionary<string, BitmapSource> ExtensionsIcon = new Dictionary<string, BitmapSource>();

        public D2PDirectoryNode Parent
        {
            get;
            protected set;
        }

        public abstract string Name
        {
            get;
        }

        private bool m_isExpanded;

        public bool IsExpanded
        {
            get { return m_isExpanded; }
            set
            {
                m_isExpanded = value; 
                
                if (m_isExpanded && Parent != null)
                    Parent.IsExpanded = true;
            }
        }

        public bool IsSelected
        {
            get;
            set;
        }

        public abstract bool CanBeExpanded();

        public bool NameContains(string str)
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(str))
                return false;

            return Name.Contains(str);
        }

        public virtual BitmapSource Icon
        {
            get
            {
                if (ExtensionsIcon.ContainsKey(Path.GetExtension(Name)))
                    return ExtensionsIcon[Path.GetExtension(Name)];

                var icon = ImageHelper.GetBitmapSource(IconHelper.GetFileIcon(Name).ToBitmap());
                ExtensionsIcon.Add(Path.GetExtension(Name), icon);

                return icon;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}