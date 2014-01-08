using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
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

        private static readonly short[] TAX_COLLECTOR_SPELLS =
        {
            462, //Déstabilisation
            461, //Compulsion de masse
            460, //Désenvoûtement
            459, //Mot soignant
            458, //Rocher
            457, //Vague
            456, //Cyclone
            455, //Flamme
            454, //Armure Venteuse
            453, //Armure Terrestre
            452, //Armure Incandescente
            451  //Armure Aqueuse
        };

        private static readonly short[][] TAX_COLLECTOR_SPELLS_LEVELS =
        {
            new short[] { 2308, 2309, 2310, 2311, 2312 }, //Déstabilisation
            new short[] { 2303, 2304, 2305, 2306, 2307 }, //Compulsion de masse
            new short[] { 2298, 2299, 2300, 2301, 2302 }, //Désenvoûtement
            new short[] { 2293, 2294, 2295, 2296, 2297 }, //Mot soignant
            new short[] { 2288, 2289, 2290, 2291, 2292 }, //Rocher
            new short[] { 2283, 2284, 2285, 2286, 2287 }, //Vague
            new short[] { 2278, 2279, 2280, 2281, 2282 }, //Cyclone
            new short[] { 2273, 2274, 2275, 2276, 2277 }, //Flamme
            new short[] { 2268, 2269, 2270, 2271, 2272 }, //Armure Venteuse
            new short[] { 2263, 2264, 2265, 2266, 2267 }, //Armure Terrestre
            new short[] { 2258, 2259, 2260, 2261, 2262 }, //Armure Incandescente
            new short[] { 2253, 2254, 2255, 2256, 2257 }  //Armure Aqueuse
        };

        [Variable(true)]
        public static int MaxMembersNumber = 50;

        private readonly List<GuildMember> m_members = new List<GuildMember>();
        private readonly WorldClientCollection m_clients = new WorldClientCollection();
        private readonly List<TaxCollectorNpc> m_taxCollectors = new List<TaxCollectorNpc>(); 
        private bool m_isDirty;
        private readonly object m_xpLock = new object();

        public Guild(int id, string name)
        {
            Record = new GuildRecord();

            Id = id;
            Name = name;
            Level = 1;
            Boost = 0;
            Prospecting = 100;
            Wisdom = 0;
            Pods = 1000;
            MaxTaxCollectors = 1;
            Spells = new List<sbyte> {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
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

                BindMemberEvents(member);
                member.BindGuild(this);
            }

            if (m_members.Count == 0)
            {
                logger.Error("Guild {0} ({1}) is empty", Id, Name);
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

        public ReadOnlyCollection<TaxCollectorNpc> TaxCollectors
        {
            get { return m_taxCollectors.AsReadOnly(); }
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
            protected set {
                Record.Experience = value;
                IsDirty = true;
            }
        }

        public short Boost
        {
            get { return Record.Boost; }
            protected set {
                Record.Boost = value;
                IsDirty = true;
            }
        }

        public short Prospecting
        {
            get { return Record.Prospecting; }
            protected set
            {
                Record.Prospecting = value;
                IsDirty = true;
            }
        }

        public short Wisdom
        {
            get { return Record.Wisdom; }
            protected set
            {
                Record.Wisdom = value;
                IsDirty = true;
            }
        }

        public short Pods
        {
            get { return Record.Pods; }
            protected set
            {
                Record.Pods = value;
                IsDirty = true;
            }
        }

        public sbyte MaxTaxCollectors
        {
            get { return Record.MaxTaxCollectors; }
            protected set
            {
                Record.MaxTaxCollectors = value;
                IsDirty = true;
            }
        }

        public List<sbyte> Spells
        {
            get { return Record.Spells; }
            protected set
            {
                Record.Spells = value;
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

        public short HireCost
        {
            get { return (short)(1000 + (Level * 100)); }
        }

        public bool IsDirty
        {
            get { return m_isDirty || Emblem.IsDirty; }
            set { m_isDirty = value;

                if (!value)
                    Emblem.IsDirty = false;
            }
        }

        public void AddTaxCollector(TaxCollectorNpc taxCollector)
        {
            m_taxCollectors.Add(taxCollector);
        }

        public void RemoveTaxCollector(TaxCollectorNpc taxCollector)
        {
            m_taxCollectors.Remove(taxCollector);
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

                if (level == Level)
                    return;

                if (level > Level)
                    Boost += (short)((level - Level) * 5);

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

        public bool UpgradePods()
        {
            if (Pods >= 5000)
                return false;

            if (Boost <= 0)
                return false;

            Boost -= 1;
            Pods += 20;

            if (Pods > 5000)
                Pods = 5000;

            return true;
        }

        public bool UpgradeProspecting()
        {
            if (Prospecting >= 500)
                return false;

            if (Boost <= 0)
                return false;

            Boost -= 1;
            Prospecting += 1;

            if (Prospecting > 500)
                Prospecting = 500;

            return true;
        }

        public bool UpgradeWisdom()
        {
            if (Wisdom >= 400)
                return false;

            if (Boost <= 0)
                return false;

            Boost -= 1;
            Wisdom += 1;

            if (Wisdom > 400)
                Wisdom = 400;

            return true;
        }

        public bool UpgradeMaxTaxCollectors()
        {
            if (MaxTaxCollectors >= 50)
                return false;

            if (Boost < 10)
                return false;

            Boost -= 10;
            MaxTaxCollectors += 1;

            if (MaxTaxCollectors > 50)
                MaxTaxCollectors = 50;

            return true;
        }

        public bool UpgradeSpell(int spellId)
        {
            if (!TAX_COLLECTOR_SPELLS.Contains((short)spellId))
                return false;

            if (Boost < 5)
                return false;

            var i = 0;
            foreach(var spell in TAX_COLLECTOR_SPELLS)
            {
                if (spell == spellId)
                {
                    if (Spells[i] < 5)
                    {
                        Boost -= 5;
                        Spells[i] += 1;
                    }
                }

                i++;
            }

            return true;
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
                                                                 guildMember.Name, Boss.Name, Name);
                }

                UpdateMember(Boss);
            }
            
            Boss = guildMember;
            Boss.RankId = 1;
            Boss.Rights = GuildRightsBitEnum.GUILD_RIGHT_BOSS;

            UpdateMember(Boss);
        }

        public bool KickMember(GuildMember kickedMember, bool kicked)
        {
            if (!RemoveMember(kickedMember))
                return false;

            foreach (var client in m_clients)
            {
                GuildHandler.SendGuildMemberLeavingMessage(client, kickedMember, kicked);   
            }

            if (!kickedMember.IsBoss)
                return true;

            if (m_members.Count > 1)
                return false;

            GuildManager.Instance.DeleteGuild(kickedMember.Guild);

            return true;
        }

        public bool KickMember(Character kicker, GuildMember kickedMember)
        {
            if (kicker.Guild != kickedMember.Guild)
                return false;

            if (kicker.GuildMember != kickedMember && (!kicker.GuildMember.HasRight(GuildRightsBitEnum.GUILD_RIGHT_BAN_MEMBERS) || kickedMember.IsBoss))
                return false;

            if (!KickMember(kickedMember, kickedMember.Character.Id == kicker.Id))
                return false;

            if (kicker.Id != kickedMember.Character.Id)
            {
                // Vous avez banni <b>%1</b> de votre guilde.
                kicker.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 177, kickedMember.Name);
            }

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

            m_clients.Add(character.Client);

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
            m_clients.Remove(member.Character.Client);

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
            GuildManager.Instance.DeleteGuildMember(member);
            UnBindMemberEvents(member);

            if (!member.IsConnected)
                return;

            var character = member.Character;

            character.GuildMember = null;
            character.RefreshActor();

            // Vous avez quitté la guilde.
            character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 176);
            GuildHandler.SendGuildLeftMessage(character.Client);
        }

        protected virtual void OnLevelChanged()
        {
            ExperienceLevelFloor = ExperienceManager.Instance.GetGuildLevelExperience(Level);
            ExperienceNextLevelFloor = ExperienceManager.Instance.GetGuildNextLevelExperience(Level);

            //Votre guilde passe niveau %1
            BasicHandler.SendTextInformationMessage(m_clients, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 208, Level);

            m_clients.Send(new GuildLevelUpMessage(Level));
        }

        private void OnMemberConnected(GuildMember member)
        {
            //Un membre de votre guilde, {player,%1,%2}, est en ligne.
            BasicHandler.SendTextInformationMessage(m_clients, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 224, member.Character.Name);

            m_clients.Add(member.Character.Client);

            UpdateMember(member);

            m_clients.Send(new GuildMemberOnlineStatusMessage(member.Id, true));
        }

        private void OnMemberDisconnected(GuildMember member, Character character)
        {
            m_clients.Remove(character.Client);

            UpdateMember(member);

            m_clients.Send(new GuildMemberOnlineStatusMessage(member.Id, false));
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

        public GuildInfosUpgradeMessage GetGuildInfosUpgrade()
        {
            return new GuildInfosUpgradeMessage(MaxTaxCollectors, (sbyte)TaxCollectorManager.Instance.GetTaxCollectorSpawns(Id).Count(), (short)(100 * Level), (short)(1 * Level), Pods, Prospecting, Wisdom, Boost,
                TAX_COLLECTOR_SPELLS, Spells);
        }
    }
}
