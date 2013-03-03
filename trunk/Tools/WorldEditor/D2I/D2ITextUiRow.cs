#region License GNU GPL
// D2ITextUiRow.cs
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

using System.ComponentModel;

namespace WorldEditor.D2I
{
    public sealed class D2ITextUiRow : D2IGridRow
    {
        public D2ITextUiRow()
        {
            
        }

        public D2ITextUiRow(string id, string text)
        {
            m_id = id;
            m_text = text;
        }

        private string m_id;

        public string Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private string m_text;

        public override string Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        public override string GetKey()
        {
            return Id;
        }
    }
}