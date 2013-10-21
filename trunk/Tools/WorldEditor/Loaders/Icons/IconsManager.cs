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
using System.Collections.ObjectModel;
using System.Linq;
using DBSynchroniser.Records.Icons;
using Stump.Core.Reflection;
using Stump.DofusProtocol.D2oClasses.Tools.D2p;
using WorldEditor.Database;

namespace WorldEditor.Loaders.Icons
{
    public class IconsManager : Singleton<IconsManager>
    {
        private D2pFile m_d2PFile;

        private Dictionary<int, Icon> m_icons;

        public Icon ErrorIcon
        {
            get;
            private set;
        }

        public Icon EmptyIcon
        {
            get;
            private set;
        }

        public void Initialize()
        {
            m_icons = DatabaseManager.Instance.Database.Query<IconRecord>("SELECT * FROM icons").ToDictionary(x => x.Id, x => new Icon(x.Id, x.ImageBinary));
            ErrorIcon = m_icons[-1];
            EmptyIcon = m_icons[0];
        }

        public ReadOnlyDictionary<int, Icon> Icons
        {
            get
            {
                return new ReadOnlyDictionary<int, Icon>(m_icons);
            }
        }

        public Icon GetIcon(int id)
        {
            if (id == 0)
                return EmptyIcon;

            if (!m_icons.ContainsKey(id))
                return ErrorIcon;

            return m_icons[id];
        }
    }
}