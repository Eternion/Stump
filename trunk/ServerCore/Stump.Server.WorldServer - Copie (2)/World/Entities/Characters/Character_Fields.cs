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
using System.Collections.Generic;
using Stump.Database.WorldServer;
using Stump.Database.WorldServer.Character;
using Stump.DofusProtocol.Enums;
using Stump.Server.DataProvider.Data.Threshold;
using Stump.Server.WorldServer.Spells;
using Stump.Server.WorldServer.World.Actors;
using Stump.Server.WorldServer.World.Actors.Character;
using Stump.Server.WorldServer.World.Entities.Actors;
using Stump.Server.WorldServer.World.Storages;

namespace Stump.Server.WorldServer.World.Entities.Characters
{
    public partial class Character
    {

        public readonly WorldClient Client;

        private readonly CharacterRecord m_record;

        public PlayableBreedEnum Breed { get; set; }

        public SexTypeEnum Sex { get; set; }

        public uint Energy { get; set; }

        public uint EnergyMax { get; set; }

        public long Experience { get; set; }

        public uint Level
        {
            get { return ThresholdManager.Instance["CharacterExp"].GetLevel(Experience); }
            set { Experience = ThresholdManager.Instance["CharacterExp"].GetLowerBound(value); }
        }

        public Inventory Inventory
        {
            get;
            set;
        }

        public SpellInventory SpellInventory
        {
            get;
            set;
        }



        //public Guild Guild;

        //public List<Job> Jobs;

        //public List<Quests> Quests;

        //public List<Zaap> Zaaps;

        public int Mood {  get; set; }

        public GameContextEnum Context { get; set; }

        public List<Actor> FollowingCharacters { get; set; }

        public int EmoteId { get; set; }

        public uint EmoteEndTime { get; set; }

        public CharacterRestrictions Restrictions { get; set; }

        public uint TitleId { get; set; }

        public string TitleParam { get; set; }

        public ActorAlignment Alignment { get; set; }

    }
}