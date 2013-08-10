#region License GNU GPL

// D2PFolderRow.cs
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

using Stump.DofusProtocol.D2oClasses.Tools.D2p;

namespace WorldEditor.Editors.Files.D2P
{
    public class D2PFolderRow : D2PGridRow
    {
        private readonly D2pDirectory m_folder;

        public D2PFolderRow(D2pDirectory folder)
        {
            m_folder = folder;
        }

        public override string Name
        {
            get { return Folder.Name; }
            set { }
        }

        public override string Type
        {
            get
            {
                return "Folder";
            }
        }

        public override bool HasSize
        {
            get
            {
                return false;
            }
        }

        public override int Size
        {
            get
            {
                return 0;
            }
        }

        public override string Container
        {
            get { return string.Empty; }
        }

        public D2pDirectory Folder
        {
            get { return m_folder; }
        }
    }
}