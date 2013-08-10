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

using System;
using System.Collections.Generic;
using System.Linq;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses.Tools.D2p;

namespace WorldEditor.Loaders.Icons
{
    public class IconsManager : Singleton<IconsManager>
    {
        private D2pFile m_d2PFile;
        private List<Icon> m_icons;

        public void Initialize(string path)
        {
            m_d2PFile = new D2pFile(path);
            m_icons = EnumerateIcons().ToList();
        }

        public List<Icon> Icons
        {
            get { return m_icons; }
            set { m_icons = value; }
        }

        public Icon GetIcon(int id)
        {
            if (!m_d2PFile.Exists(id + ".png"))
                throw new ArgumentException(string.Format("Item icon {0} not found", id));

            var data = m_d2PFile.ReadFile(id + ".png");

            return new Icon(id, id + ".png", data);
        }

        private IEnumerable<Icon> EnumerateIcons()
        {
            foreach (var entry in m_d2PFile.Entries)
            {
                if (!entry.FullFileName.EndsWith(".png"))
                    continue;

                var data = m_d2PFile.ReadFile(entry);
                var name = entry.FileName.Replace(".png", "");
                var id = name == "empty" || name == "error" ? -1 : int.Parse(name);

                yield return new Icon(id, entry.FileName, data);
            }
        }
    }
}