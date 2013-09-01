#region License GNU GPL

// IconsManager.cs
// 
// Copyright (C) 2012 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

#endregion

using System.Collections.Generic;
using System.Linq;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses.Tools.D2p;

namespace WorldEditor.Loaders.Icons
{
    public class IconsManager : Singleton<IconsManager>
    {
        private D2pFile m_d2PFile;

        private Icon m_emptyIcon;
        private Icon m_errorIcon;
        private Dictionary<int, Icon> m_icons;

        public Icon ErrorIcon
        {
            get { return m_errorIcon; }
        }

        public Icon EmptyIcon
        {
            get { return m_emptyIcon; }
        }

        public void Initialize(string path)
        {
            m_d2PFile = new D2pFile(path);
            m_icons = EnumerateIcons().ToDictionary(x => x.Id);
            m_errorIcon = new Icon(-1, m_d2PFile.FileName, m_d2PFile.ReadFile("error.png"));
            m_emptyIcon = new Icon(0, m_d2PFile.FileName, m_d2PFile.ReadFile("empty.png"));
        }

        public IEnumerable<Icon> Icons
        {
            get { return m_icons.Values; }
        }

        public Icon GetIcon(int id)
        {
            if (id == 0)
                return m_emptyIcon;

            if (!m_icons.ContainsKey(id))
                return m_errorIcon;

            byte[] data = m_d2PFile.ReadFile(id + ".png");

            return new Icon(id, id + ".png", data);
        }

        private IEnumerable<Icon> EnumerateIcons()
        {
            foreach (D2pEntry entry in m_d2PFile.Entries)
            {
                if (!entry.FullFileName.EndsWith(".png"))
                    continue;

                byte[] data = m_d2PFile.ReadFile(entry);
                string name = entry.FileName.Replace(".png", "");

                int id;
                if (name == "empty")
                    id = 0;
                else if (name == "error")
                    id = -1;
                else
                    id = int.Parse(name);

                yield return new Icon(id, entry.FileName, data);
            }
        }
    }
}