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
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Items;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.World.Actors.Character
{
    public partial class Character
    {

        public readonly WorldClient Client;

        public readonly CharacterRecord Record;

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

        public BreedEnum Breed;

        //public Guild Guild;

        //public List<Job> Jobs;

        //public List<Quests> Quests;

        //public List<Zaap> Zaaps;

        public int Mood
        {
            get;
            set;
        }

        public uint Energy;

        public uint EnergyMax;

        public long Experience;

        public uint Level
        {
            get { return Threshold.ThresholdManager.Thresholds["CharacterExp"].GetLevel(Experience); }
            set { Experience = Threshold.ThresholdManager.Thresholds["CharacterExp"].GetLowerBound(value); }
        }

        public SexTypeEnum Sex
        {
            get;
            set;
        }

        public GameContextEnum Context
        {
            get;
            set;
        }

        public List<EntityLook> FollowingCharactersLook
        {
            get;
            set;
        }

        public int EmoteId
        {
            get;
            set;
        }

        public uint EmoteEndTime
        {
            get;
            set;
        }

        public CharacterRestrictions Restrictions
        {
            get;
            set;
        }

        public uint TitleId
        {
            get;
            set;
        }

        public string TitleParam
        {
            get;
            set;
        }

        public ActorAlignment Alignment
        {
            get;
            set;
        }

    }
}