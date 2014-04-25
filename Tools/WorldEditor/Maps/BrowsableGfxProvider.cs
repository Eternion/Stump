#region License GNU GPL
// BrowsableGfxProvider.cs
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

using System.Collections.Generic;
using Stump.DofusProtocol.D2oClasses.Tools.Ele.Datas;
using WorldEditor.Helpers.Collections;

namespace WorldEditor.Maps
{
    public class BrowsableGfxProvider : IItemsProvider<BrowsableGfx>
    {
        private readonly NormalGraphicalElementData[] m_elements;

        public BrowsableGfxProvider(NormalGraphicalElementData[] elements)
        {
            m_elements = elements;
        }

        public int FetchCount()
        {
            return m_elements.Length;
        }

        public IList<BrowsableGfx> FetchRange(int startIndex, int count)
        {
            var page = new List<BrowsableGfx>();
            for (int i = 0; i < count && i+startIndex < m_elements.Length; i++)
            {
                var element = m_elements[i + startIndex];
                page.Add(new BrowsableGfx(element.Gfx, WorldGFXManager.GetGfx(element.Gfx)));
            }
            return page;
        }
    }
}