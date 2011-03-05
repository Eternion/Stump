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
using Stump.BaseCore.Framework.Utils;
using Stump.Database.WorldServer;
using Stump.DofusProtocol.Enums;
using Stump.Server.DataProvider.Data.Threshold;

namespace Stump.Server.WorldServer.World.Guilds
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

        public uint LastConnectionTimestamp
        {
            get { return (uint) DateTime.Now.Subtract(LastConnection).TotalMinutes; }
        }

        public bool IsConnected
        { get { Singleton<World>.Instance.IsConnected(Id); } }

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
            if (IsConnected)
            {
                var character = Singleton<World>.Instance.GetCharacter(Id);
                return new DofusProtocol.Classes.GuildMember(Id, ThresholdManager.Instance["CharacterExp"].GetLevel(character.Experience), character.Name, (int)character.Breed, character.Sex == SexTypeEnum.SEX_MALE, Rank, GivenExperience, GivenPercent, Rights, 1, character.Alignment.AlignmentSide, 0, character.Mood);
            }
                return new DofusProtocol.Classes.GuildMember(Id, ThresholdManager.Instance["CharacterExp"].GetLevel(Experience), Name, (int)Breed, Sex, Rank, GivenExperience, GivenPercent, Rights, 0, AlignmentSide, LastConnectionTimestamp, 0);
        }

    }
}