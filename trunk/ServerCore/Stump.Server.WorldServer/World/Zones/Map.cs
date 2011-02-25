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
using Stump.Server.DataProvider.Data.Map;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.World.Entities.Characters;
using Stump.Server.WorldServer.World.Entities.Merchants;
using Stump.Server.WorldServer.World.Entities.Monsters;
using Stump.Server.WorldServer.World.Entities.TaxCollectors;

namespace Stump.Server.WorldServer.World.Zones
{
    public class Map
    {

        public Map(MapTemplate template, SubArea subArea)
        {
            Template = template;
            Neighbours.Add(MapNeighbour.Top, template.TopNeighbourId);
            Neighbours.Add(MapNeighbour.Bottom, template.BottomNeighbourId);
            Neighbours.Add(MapNeighbour.Left, template.LeftNeighbourId);
            Neighbours.Add(MapNeighbour.Right, template.RightNeighbourId);
            SubArea = subArea;
        }

        public readonly MapTemplate Template;
        public readonly SubArea SubArea;

        public int Id
        {
            get { return Template.Id; }
        }

        public MapTypeEnum MapType
        {
            get { return (MapTypeEnum)Template.MapType; }
            set { Template.MapType = (byte)value; }
        }

        public Point Position
        {
            get { return Template.Position; }
            set { Template.Position = value; }
        }

        public MapCapabilities Capabilities
        {
            get { return Template.Capabilities; }
        }

        public  Dictionary<MapNeighbour, uint> Neighbours;



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
            m_characters.Remove(character);
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