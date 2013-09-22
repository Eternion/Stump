#region License GNU GPL
// TinselManager.cs
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
using System.Collections.ObjectModel;
using System.Linq;
using Stump.Core.Reflection;
using Stump.Server.BaseServer.Database;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Database.Effects;

namespace Stump.Server.WorldServer.Database.Tinsel
{
    public class TinselManager : DataManager<TinselManager>
    {
        private Dictionary<short, TitleRecord> m_titles;
        private Dictionary<short, OrnamentRecord> m_ornaments;

        [Initialization(InitializationPass.Fourth)]
        public override void Initialize()
        {
            m_titles = Database.Fetch<TitleRecord>(TitleRelator.FetchQuery).ToDictionary(entry => (short)entry.Id);
            m_ornaments = Database.Fetch<OrnamentRecord>(OrnamentRelator.FetchQuery).ToDictionary(entry => (short)entry.Id);

        }

        public IReadOnlyDictionary<short, TitleRecord> Titles
        {
            get { return new ReadOnlyDictionary<short, TitleRecord>(m_titles); }
        }

        public IReadOnlyDictionary<short, OrnamentRecord> Ornaments
        {
            get
            {
                return new ReadOnlyDictionary<short, OrnamentRecord>(m_ornaments);
            }
        }
    }
}