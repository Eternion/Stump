using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Basic;
using GuildMemberNetwork = Stump.DofusProtocol.Types.GuildMember;
using Stump.Server.WorldServer.Handlers.Guilds;

namespace Stump.Server.WorldServer.Game.Guilds
{
    public class Guild
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private static readonly double[][] XP_PER_GAP =
        {
            new double[] {0, 10}, 
            new double[] {10, 8}, 
            new double[] {20, 6}, 
            new double[] {30, 4},
            new double[] {40, 3},
            new double[] {50, 2}, 
            new double[] {60, 1.5}, 
            new double[] {70, 1}
        };

        [Variable(true)]
        public static int MaxMembersNumber = 50;

        private readonly List<GuildMember> m_members = new List<GuildMember>();
        private readonly WorldClientCollection m_clients = new WorldClientCollection();
        private bool m_isDirty;
        private readonly object m_xpLock = new object();

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
            ExperienceLevelFloor = ExperienceManager.Instance.GetGuildLevelExperience(Level);
            ExperienceNextLevelFloor = ExperienceManager.Instance.GetGuildNextLevelExperience(Level);
            Emblem = new GuildEmblem(Record);

            foreach (var member in m_members)
            {
                if (member.IsBoss)
                {
                    if (Boss != null)
                        logger.Error("There is at least two boss in guild {0} ({1})", Id, Name);

                    Boss = member;
                }

                if (member.CharacterRecord == null)
                {
                    logger.Error("GuildMember {0} is not linked to a Character -> Delete", member.Id);

                    World.Instance.Database.Delete(member.Record);
                }

                BindMemberEvents(member);
                member.BindGuild(this);
            }

            if (m_members.Count == 0)
            {
                logger.Error("Guild {0} ({1}) is empty", Id, Name);
                World.Instance.Database.Delete(Record);
            }
            else if (Boss == null)
            {
                var member = m_members.First();
                SetBoss(member);
                logger.Error("There is at no boss in guild {0} ({1}) -> Promote new Boss", Id, Name);
            }
        }

        public ReadOnlyCollection<GuildMember> Members
        {
            get { return m_members.AsReadOnly(); }
        }

        public WorldClientCollection Clients
        {
            get { return m_clients; }
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

        public GuildMember Boss
        {
            get;
            private set;
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

        public long AdjustGivenExperience(Character giver, long amount)
        {
            var gap = giver.Level - Level;

            for (var i = XP_PER_GAP.Length - 1; i >= 0; i--)
            {
                if (gap > XP_PER_GAP[i][0])
                    return (long) (amount*XP_PER_GAP[i][1]*0.01);
            }

            return (long)( amount * XP_PER_GAP[0][1] * 0.01 );
        }

        public void AddXP(long experience)
        {
            lock (m_xpLock)
            {
                Experience += experience;

                var level = ExperienceManager.Instance.GetGuildLevel(Experience);

                if (level == Level) return;

                Level = level;
                OnLevelChanged();
            }
        }

        public void SetXP(long experience)
        {
            lock (m_xpLock)
            {
                Experience = experience;

                var level = ExperienceManager.Instance.GetGuildLevel(Experience);

                if (level == Level) return;

                Level = level;
                OnLevelChanged();
            }
        }

        public void SetBoss(GuildMember guildMember)
        {
            if (guildMember.Guild != this)
                return;

            if (Boss == guildMember)
                return;

            if (Boss != null)
            {
                Boss.RankId = 0;
                Boss.Rights = GuildRightsBitEnum.GUILD_RIGHT_NONE;

                if (m_members.Count > 1)
                {
                    // <b>%1</b> a remplacé <b>%2</b>  au poste de meneur de la guilde <b>%3</b>
                    BasicHandler.SendTextInformationMessage(m_clients, TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 199, 
                                                                 guildMember.CharacterRecord.Name, Boss.CharacterRecord.Name, Name);
                }

                UpdateMember(Boss);
            }
            
            Boss = guildMember;
            Boss.RankId = 1;
            Boss.Rights = GuildRightsBitEnum.GUILD_RIGHT_BOSS;

            UpdateMember(Boss);
        }

        public bool KickMember(GuildMember kickedMember)
        {
            if (!RemoveMember(kickedMember))
                return false;

            GuildHandler.SendGuildMemberLeavingMessage(m_clients, kickedMember, true);

            if (kickedMember.IsBoss)
            {
                if (m_members.Count > 1)
                    return false;

                GuildManager.Instance.DeleteGuild(kickedMember.Guild);
            }


            if (kickedMember.IsConnected)
            {
                kickedMember.Character.GuildMember = null;

                // Vous avez quitté la guilde.
                kickedMember.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 176);
            }

            return true;
        }

        public bool KickMember(Character kicker, GuildMember kickedMember)
        {
            if (kicker.Guild != kickedMember.Guild)
                return false;

            if (kicker.GuildMember != kickedMember && (!kicker.GuildMember.HasRight(GuildRightsBitEnum.GUILD_RIGHT_BAN_MEMBERS) || kickedMember.IsBoss))
                return false;

            if (!KickMember(kickedMember))
                return false;


            // Vous avez banni <b>%1</b> de votre guilde.
            kicker.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 177, kickedMember.CharacterRecord.Name);

            return true;
        }

        public bool ChangeParameters(GuildMember member, short rank, byte xpPercent, uint rights)
        {
            if (rank == 1)
            {
                SetBoss(member);
            }
            else
            {
                member.RankId = rank;
                member.Rights = (GuildRightsBitEnum) rights;
            }

            member.GivenPercent = xpPercent;

            UpdateMember(member);

            if (member.IsConnected)
                GuildHandler.SendGuildMembershipMessage(member.Character.Client, member);

            return true;
        }

        public bool ChangeParameters(Character modifier, GuildMember member, short rank, byte xpPercent, uint rights)
        {
            if (modifier.Guild != member.Guild)
                return false;

            if (modifier.GuildMember != member && modifier.GuildMember.IsBoss && rank == 1)
            {
                SetBoss(member);
            }
            else
            {
                if (modifier.GuildMember == member || !member.IsBoss)
                {
                    if (modifier.GuildMember.HasRight(GuildRightsBitEnum.GUILD_RIGHT_MANAGE_RANKS))
                        member.RankId = rank;

                    if (modifier.GuildMember.HasRight(GuildRightsBitEnum.GUILD_RIGHT_MANAGE_RIGHTS))
                        member.Rights = (GuildRightsBitEnum)rights;
                }
            }

            if (modifier.GuildMember.HasRight(GuildRightsBitEnum.GUILD_RIGHT_MANAGE_XP_CONTRIBUTION) ||
                (modifier.GuildMember == member &&
                 modifier.GuildMember.HasRight(GuildRightsBitEnum.GUILD_RIGHT_MANAGE_MY_XP_CONTRIBUTION)))
            {
                member.GivenPercent = (byte) (xpPercent < 90 ? xpPercent : 90);
            }

            UpdateMember(member);

            if (member.IsConnected)
                GuildHandler.SendGuildMembershipMessage(member.Character.Client, member);

            return true;
        }

        public void Save(ORM.Database database)
        {
            if (Record.IsNew)
                database.Insert(Record);
            else
                database.Update(Record);

            foreach (var member in Members.Where(x => !x.IsConnected && x.IsDirty))
            {
                member.Save(database);
            }

            IsDirty = false;
            Record.IsNew = false;
        }

        protected void UpdateMember(GuildMember member)
        {
            GuildHandler.SendGuildInformationsMemberUpdateMessage(m_clients, member);
        }

        public bool CanAddMember()
        {
            return m_members.Count < MaxMembersNumber;
        }

        public GuildMember TryGetMember(int id)
        {
            return m_members.FirstOrDefault(x => x.Id == id);
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
            m_members.Add(member);
            character.GuildMember = member;

            if (m_members.Count == 1)
                SetBoss(member);

            OnMemberAdded(member);

            return true;
        }

        public bool RemoveMember(GuildMember member)
        {
            if (member == null || !m_members.Contains(member))
                return false;

            m_members.Remove(member);

            OnMemberRemoved(member);
            return true;
        }

        protected virtual void OnMemberAdded(GuildMember member)
        {
            BindMemberEvents(member);
            GuildManager.Instance.RegisterGuildMember(member);

            if (member.IsConnected)
            {
                GuildHandler.SendGuildJoinedMessage(member.Character.Client, member);
                GuildHandler.SendGuildInformationsMembersMessage(member.Character.Client, this);
                GuildHandler.SendGuildInformationsGeneralMessage(member.Character.Client, this);
                member.Character.RefreshActor();
            }

            UpdateMember(member);
        }

        protected virtual void OnMemberRemoved(GuildMember member)
        {
            UnBindMemberEvents(member);
            GuildManager.Instance.DeleteGuildMember(member);

            if (!member.IsConnected)
                return;

            member.Character.GuildMember = null;
            member.Character.RefreshActor();
            GuildHandler.SendGuildLeftMessage(member.Character.Client);
        }

        protected virtual void OnLevelChanged()
        {
            ExperienceLevelFloor = ExperienceManager.Instance.GetGuildLevelExperience(Level);
            ExperienceNextLevelFloor = ExperienceManager.Instance.GetGuildNextLevelExperience(Level);

            //Votre guilde passe niveau %1
            BasicHandler.SendTextInformationMessage(m_clients, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 208, Level);
        }

        private void OnMemberConnected(GuildMember member)
        {
            m_clients.Add(member.Character.Client);

            UpdateMember(member);

            foreach (var connectedMember in Members.Where(x => x.IsConnected))
            {
                if (connectedMember != member && connectedMember.Character.WarnOnGuildConnection)
                    // Un membre de votre guilde, {player,%1,%2}, est en ligne.
                    connectedMember.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE,
                                                                     224, member.CharacterRecord.Name);
            }
        }

        private void OnMemberDisconnected(GuildMember member, Character character)
        {
            m_clients.Remove(character.Client);

            UpdateMember(member);
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

        public BasicGuildInformations GetBasicGuildInformations()
        {
            return new BasicGuildInformations(Id, Name);
        }
    }
}
