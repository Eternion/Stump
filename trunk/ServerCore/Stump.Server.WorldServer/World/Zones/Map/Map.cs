// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System.Drawing;
using System.Linq;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Collections;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.World.Actors.Character;

namespace Stump.Server.WorldServer.World.Zones.Map
{
    public class Map
    {

        public Map(uint id, MapTypeEnum mapType, uint capabilities, uint topNeighbour, uint bottomNeighbour, uint leftNeighbour, uint rightNeighbour, Point position, SubArea subArea)
        {
            Id = id;
            MapType = mapType;
            Capabilities = capabilities;
            Neighbours.Add(MapNeighbour.Top, topNeighbour);
            Neighbours.Add(MapNeighbour.Bottom, bottomNeighbour);
            Neighbours.Add(MapNeighbour.Left, leftNeighbour);
            Neighbours.Add(MapNeighbour.Right, rightNeighbour);
            Position = position;
            SubArea = subArea;
        }

        public readonly uint Id;
        public readonly MapTypeEnum MapType;
        public readonly Dictionary<MapNeighbour, uint> Neighbours;
        public readonly Point Position;
        public readonly SubArea SubArea;
        public readonly uint Capabilities;

        private readonly ConcurrentList<Character> m_characters = new ConcurrentList<Character>();
        public IEnumerable<Character> Characters
        {
            get { return m_characters; }
        }

        public List<NPC> Npcs = new List<NPC>();
    
        public List<Merchant> Merchants = new List<Merchant>();

        public List<MonsterGroup> MonsterGroups = new List<MonsterGroup>();

        public TaxCollector TaxCollector;

        public Prism Prism;


        public void Spawn()
        {
            
        }


        public void Enter(Character character)
        {
            var lastMap = character.Map;

            m_characters.Add(character);

            if (lastMap.SubArea != this.SubArea)
            {
                lastMap.SubArea.Characters.Remove(character);
                SubArea.Characters.Add(character);

                if (lastMap.SubArea.Area != this.SubArea.Area)
                {
                    lastMap.SubArea.Area.Characters.Remove(character);
                    SubArea.Area.Characters.Add(character);
                }
            }
        }

        public void Leave(Character character)
        {
            Characters.Remove(character);
        }

        public List<GameRolePlayActorInformations> GetActors()
        {
            var actors = new List<GameRolePlayActorInformations>();
            
            actors.AddRange(m_characters.Select(c => c.ToGameRolePlayActor()));

            actors.AddRange(Npcs.Select(m => m.ToGameRolePlayActor()));

            actors.AddRange(Merchants.Select(m => m.ToGameRolePlayActor()));

            actors.AddRange(MonsterGroups.Select(m => m.ToGameRolePlayActor()));

            if (TaxCollector != null)
                actors.Add(TaxCollector.ToGameRolePlayActor());
            
            if (Prism != null)
                actors.Add(Prism.ToGameRolePlayActor());

            return actors;
        }

    }
}