#region License GNU GPL

// D2PFileRow.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
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

using System.IO;
using Stump.DofusProtocol.D2oClasses.Tools.D2p;

namespace WorldEditor.Editors.Files.D2P
{
    public class D2PFileRow : D2PGridRow
    {
        private readonly D2pEntry m_entry;
        private string m_type;

        public D2PFileRow(D2pEntry entry)
        {
            m_entry = entry;
        }

        public D2pEntry Entry
        {
            get { return m_entry; }
        }

        public override string Name
        {
            get { return m_entry.FileName; }
            set {  }
        }

        public override string Type
        {
            get { return m_type ?? (m_type = Path.GetExtension(m_entry.FileName).Remove(0, 1).ToUpper() + " File"); }
        }

        public override bool HasSize
        {
            get { return true; }
        }

        public override int Size
        {
            get { return m_entry.Size; }
        }

        public override string Container
        {
            get { return m_entry.Container.FileName; }
        }
    }
}