using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using GuildMemberNetwork = Stump.DofusProtocol.Types.GuildMember;

namespace Stump.Server.WorldServer.Game.Guilds
{
    public class Guild
    {
        [Variable(true)]
        public static int MaxMembersNumber = 50;

        private readonly List<GuildMember> m_members = new List<GuildMember>();
        private readonly WorldClientCollection m_clients = new WorldClientCollection();
        private bool m_isDirty;

        public Guild(int id, string name)
        {
            Record = new GuildRecord();

            Id = id;
            Name = name;
            Level = 1;
            ExperienceLevelFloor = 0;
            ExperienceNextLevelFloor = ExperienceManager.Instance.GetGuildNextLevelExperience(Level);
            Record.CreationDate = DateTime.Now;
            Record.IsNew = true;
            Emblem = new GuildEmblem(Record)
                {
                    BackgroundColor = Color.White,
                    BackgroundShape = 1,
                    SymbolColor = Color.Black,
                    SymbolShape = 1,
                };
            IsDirty = true;
        }

        public Guild(GuildRecord record, IEnumerable<GuildMember> members)
        {
            Record = record;
            m_members.AddRange(members);
            Level = ExperienceManager.Instance.GetGuildLevel(Experience);
            Emblem = new GuildEmblem(Record);

            foreach (var member in m_members)
            {
                BindMemberEvents(member);
                member.BindGuild(this);
            }
        }

        public ReadOnlyCollection<GuildMember> Members
        {
            get { return m_members.AsReadOnly(); }
        }

        public GuildRecord Record
        {
            get;
            set;
        }

        public int Id
        {
            get { return Record.Id; }
            private set { Record.Id = value; }
        }

        public long Experience
        {
            get { return Record.Experience; }
            protected set { Record.Experience = value;
                IsDirty = true;
            }
        }

        public long ExperienceLevelFloor
        {
            get;
            protected set;
        }

        public long ExperienceNextLevelFloor
        {
            get;
            protected set;
        }

        public DateTime CreationDate
        {
            get { return Record.CreationDate; }
        }

        public string Name
        {
            get { return Record.Name; }
            protected set
            {
                Record.Name = value;
                IsDirty = true;
            }
        }

        public GuildEmblem Emblem
        {
            get;
            protected set;
        }

        public byte Level
        {
            get;
            protected set;
        }

        public bool IsDirty
        {
            get { return m_isDirty || Emblem.IsDirty; }
            set { m_isDirty = value;

                if (!value)
                    Emblem.IsDirty = false;
            }
        }

        public void AddXP(long experience)
        {
            Experience += experience;

            var level = ExperienceManager.Instance.GetGuildLevel(Experience);

            if (level == Level) return;

            Level = level;
            OnLevelChanged();
        }

        public void SetXP(long experience)
        {
            Experience = experience;

            var level = ExperienceManager.Instance.GetGuildLevel(Experience);

            if (level == Level) return;

            Level = level;
            OnLevelChanged();
        }

        public bool KickMember(Character character, Character member)
        {
            if (character.Guild != member.Guild)
                return false;

            if (character != member && !character.GuildMember.HasRight(character, GuildRightsBitEnum.GUILD_RIGHT_BAN_MEMBERS))
                return false;

            if (!member.Guild.RemoveMember(member))
                return false;

            member.GuildMember = null;
            member.Map.Refresh(member);

            member.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 176);
            if (character != member)
                character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 177, member);

            return true;
        }

        public bool ChangeParameters(Character character, Character member, short rank, sbyte xpPercent, uint rights)
        {
            if (character.Guild != member.Guild)
                return false;

            if (character != member && character.GuildMember.RankId == 1 && rank == 1)
            {
                GuildManager.Instance.SetBoss(member.GuildMember);
                member.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 199, member, character);
            }
            else
            {
                if (character.GuildMember.HasRight(character, GuildRightsBitEnum.GUILD_RIGHT_MANAGE_RANKS))
                    member.GuildMember.RankId = rank;

                if (character.GuildMember.HasRight(character, GuildRightsBitEnum.GUILD_RIGHT_MANAGE_RIGHTS))
                    member.GuildMember.Rights = (GuildRightsBitEnum)rights;
            }

            if (character.GuildMember.HasRight(character, GuildRightsBitEnum.GUILD_RIGHT_MANAGE_XP_CONTRIBUTION) ||
                (character == member && character.GuildMember.HasRight(character, GuildRightsBitEnum.GUILD_RIGHT_MANAGE_MY_XP_CONTRIBUTION)))
                member.GuildMember.GivenPercent = (byte)xpPercent;

            return true;
        }

        public bool CanAddMember()
        {
            return m_members.Count < MaxMembersNumber;
        }

        public bool TryAddMember(Character character)
        {
            GuildMember dummy;
            return TryAddMember(character, out dummy);
        }

        public bool TryAddMember(Character character, out GuildMember member)
        {
            if (!CanAddMember())
            {
                member = null;
                return false;
            }

            member = new GuildMember(this, character);

            member.OnCharacterConnected(character);

            m_members.Add(member);

            OnMemberAdded(member);

            return true;
        }

        public bool RemoveMember(Character character)
        {
            if (character.GuildMember == null || !m_members.Contains(character.GuildMember))
                return false;

            OnMemberRemoved(character.GuildMember);
            return true;
        }

        protected virtual void OnMemberAdded(GuildMember member)
        {
            BindMemberEvents(member);
            // todo : sends the packet to all connected members
            GuildManager.Instance.RegisterGuildMember(member);
        }

        protected virtual void OnMemberRemoved(GuildMember member)
        {
            UnBindMemberEvents(member);
            // todo : sends the packet to all connected members
            GuildManager.Instance.DeleteGuildMember(member);
        }

        protected virtual void OnLevelChanged()
        {
            ExperienceLevelFloor = ExperienceManager.Instance.GetGuildLevelExperience(Level);
            ExperienceNextLevelFloor = ExperienceManager.Instance.GetGuildNextLevelExperience(Level);
        }

        private void OnMemberConnected(GuildMember member)
        {
            m_clients.Add(member.Character.Client);
            // todo : sends the packet to all connected members

        }

        private void OnMemberDisconnected(GuildMember member, Character character)
        {
            m_clients.Remove(character.Client);
            // todo : sends the packet to all connected members

        }

        private void BindMemberEvents(GuildMember member)
        {
            member.Connected += OnMemberConnected;
            member.Disconnected += OnMemberDisconnected;
        }


        private void UnBindMemberEvents(GuildMember member)
        {
            member.Connected -= OnMemberConnected;
            member.Disconnected -= OnMemberDisconnected;
        }

        public GuildInformations GetGuildInformations()
        {
            return new GuildInformations(Id, Name, Emblem.GetNetworkGuildEmblem());
        }
    }
}
