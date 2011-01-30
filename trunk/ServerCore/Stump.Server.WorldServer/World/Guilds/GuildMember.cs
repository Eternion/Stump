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
using System;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Entities
{
    public class GuildMember
    {

        public GuildMember(Guild guild, GuildMemberRecord record)
        {
            m_record = record;
            Guild = guild;
            Id = record.CharacterId;
            Experience = record.Character.Experience;
            Name = record.Character.Name;
            Breed = (BreedEnum)record.Character.Breed;
            Sex = record.Character.Sex == SexTypeEnum.SEX_MALE;
            Rank = record.Rank;
            GivenExperience = record.GivenExperience;
            GivenPercent = record.GivenPercent;
            Rights = record.Rights;
            AlignmentSide = record.Character.Alignment.AlignmentSide;
            LastConnection = record.Character.LastConnection;
        }

        private readonly GuildMemberRecord m_record;

        public Guild Guild
        {
            get;
            set;
        }

        public uint Id
        {
            get;
            set;
        }

        public long Experience
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public BreedEnum Breed
        {
            get;
            set;
        }

        public bool Sex
        {
            get;
            set;
        }

        public uint Rank
        {
            get;
            set;
        }

        public uint GivenExperience
        {
            get;
            set;
        }

        public byte GivenPercent
        {
            get;
            set;
        }

        public uint Rights
        {
            get;
            set;
        }

        public bool Connected
        { get { return World.World.Instance.IsConnected(Id); } }

        public int AlignmentSide
        {
            get;
            set;
        }

        public DateTime LastConnection
        {
            get;
            set;
        }


        public void Save()
        {
            m_record.GivenExperience = GivenExperience;
            m_record.GivenPercent = GivenPercent;
            m_record.Rank = Rank;
            m_record.Rights = Rights;
            m_record.SaveAndFlush();
        }

        public DofusProtocol.Classes.GuildMember ToGuildMember()
        {
            if (Connected)
            {
                var character = World.World.Instance.GetCharacter(Id);
                return new DofusProtocol.Classes.GuildMember(Id, Threshold.ThresholdManager.Thresholds["CharacterExp"].GetLevel(character.Experience), character.Name, (int)character.Breed, character.Sex == SexTypeEnum.SEX_MALE, Rank, GivenExperience, GivenPercent, Rights, 1, character.Alignment.AlignmentSide, 0, character.Mood);
            }
            else
                return new DofusProtocol.Classes.GuildMember(Id, Threshold.ThresholdManager.Thresholds["CharacterExp"].GetLevel(Experience), Name, (int)Breed, Sex, Rank, GivenExperience, GivenPercent, Rights, 0, AlignmentSide, (uint)DateTime.Now.Subtract(LastConnection).TotalMinutes, 0);
        }

    }
}