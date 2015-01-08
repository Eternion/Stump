#region License GNU GPL
// ZaapsAutoFinder.cs
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

using System.Linq;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Game;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Interactives;
using Stump.Server.WorldServer.Game.Maps;

namespace ArkalysPlugin
{
    public class ZaapsAutoFinder
    {
        private static Map[] m_zaaps;
        private const int ZAAP_TEMPLATE = 16;
        static ZaapsAutoFinder()
        {
            CharacterManager.Instance.CreatingCharacter += OnCreatingCharacter;
        }

        [Initialization(typeof(World))]
        public static void Initialize()
        {
            m_zaaps = InteractiveManager.Instance.GetInteractiveSpawns().Where(x => x.TemplateId == ZAAP_TEMPLATE).Select(x => World.Instance.GetMap(x.MapId)).ToArray();
        }

        private static void OnCreatingCharacter(CharacterRecord record)
        {
            record.KnownZaaps = m_zaaps.Where(x => x != null).ToList();
            WorldServer.Instance.IOTaskPool.AddMessage(() => CharacterManager.Instance.Database.Update(record));
        }
    }
}