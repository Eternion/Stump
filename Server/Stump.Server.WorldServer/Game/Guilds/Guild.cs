using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using NLog;
using Stump.Core.Attributes;
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Actors.RolePlay.TaxCollectors;
using Stump.Server.WorldServer.Game.Items;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Basic;
using Stump.Server.WorldServer.Handlers.TaxCollector;
using Stump.Server.WorldServer.Handlers.Guilds;
using GuildMemberNetwork = Stump.DofusProtocol.Types.GuildMember;
using NetworkGuildEmblem = Stump.DofusProtocol.Types.GuildEmblem;

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

        public static readonly short[] TAX_COLLECTOR_SPELLS =
        {
            (short) SpellIdEnum.ROCHER,
            (short) SpellIdEnum.VAGUE,
            (short) SpellIdEnum.CYCLONE,
            (short) SpellIdEnum.FLAMME,
            (short) SpellIdEnum.DÉSTABILISATION,
            (short) SpellIdEnum.DÉSENVOUTEMENT,
            (short) SpellIdEnum.MOT_SOIGNANT_459,
            (short) SpellIdEnum.ARMURE_AQUEUSE_451,
            (short) SpellIdEnum.ARMURE_TERRESTRE_453,
            (short) SpellIdEnum.ARMURE_VENTEUSE_454,
            (short) SpellIdEnum.ARMURE_INCANDESCENTE_452,
            (short) SpellIdEnum.COMPULSION_DE_MASSE,
        };

        [Variable(true)] public static int MaxMembersNumber = 50;

        [Variable(true)] public static int MaxGuildXP = 300000;

        private readonly List<GuildMember> m_members = new List<GuildMember>();
        private readonly WorldClientCollection m_clients = new WorldClientCollection();
        private readonly List<TaxCollectorNpc> m_taxCollectors = new List<TaxCollectorNpc>();
        private readonly Spell[] m_spells = new Spell[TAX_COLLECTOR_SPELLS.Length];
        private bool m_isDirty;
        private readonly object m_lock = new object();

        public Guild(int id, string name)
        {
            Record = new GuildRecord();

            Id = id;
            Name = name;
            Level = 1;
            Boost = 0;
            TaxCollectorProspecting = 100;
            TaxCollectorWisdom = 0;
            TaxCollectorPods = 1000;
            MaxTaxCollectors = 1;
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

            if (m_members.Count == 0)
            {
                logger.Error("Guild {0} ({1}) is empty", Id, Name);
                return;
            }

            foreach (var member in m_members)
            {
                BindMemberEvents(member);
                member.BindGuild(this);
            }

            if (Boss == null)
            {
                logger.Error("There is at no boss in guild {0} ({1}) -> Promote new Boss", Id, Name);
                var newBoss = Members.OrderBy(x => x.RankId).FirstOrDefault();
                if (newBoss != null)
                    newBoss.RankId = 1;
            }

            // load spells
            for (var i = 0; i < record.Spells.Length && i < TAX_COLLECTOR_SPELLS.Length; i++)
            {
                if (record.Spells[i] == 0)
                    continue;
                
                m_spells[i] = new Spell(TAX_COLLECTOR_SPELLS[i], (byte)record.Spells[i]);
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
            get { return Members.FirstOrDefault(x => x.RankId == 1); }
        }

        public long Experience
        {
            get { return Record.Experience; }
            protected set
            {
                Record.Experience = value;
                IsDirty = true;
            }
        }

        public uint Boost
        {
            get { return Record.Boost; }
            protected set
            {
                Record.Boost = value;
                IsDirty = true;
            }
        }

        public int TaxCollectorProspecting
        {
            get { return Record.Prospecting; }
            protected set
            {
                Record.Prospecting = value;
                IsDirty = true;
            }
        }

        public int TaxCollectorWisdom
        {
            get { return Record.Wisdom; }
            protected set
            {
                Record.Wisdom = value;
                IsDirty = true;
            }
        }

        public int TaxCollectorPods
        {
            get { return Record.Pods; }
            protected set
            {
                Record.Pods = value;
                IsDirty = true;
            }
        }

        public int TaxCollectorHealth
        {
            get { return 100*Level; }
        }

        public int TaxCollectorResistance
        {
            get { return Level > 50 ? 50 : Level; }
        }

        public int TaxCollectorDamageBonuses
        {
            get { return Level; }
        }

        public int MaxTaxCollectors
        {
            get { return Record.MaxTaxCollectors; }
            protected set
            {
                Record.MaxTaxCollectors = value;
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
            get { return (short) (1000 + (Level*100)); }
        }

        public bool IsDirty
        {
            get { return m_isDirty || Emblem.IsDirty; }
            set
            {
                m_isDirty = value;

                if (!value)
                    Emblem.IsDirty = false;
            }
        }

        public void AddTaxCollector(TaxCollectorNpc taxCollector)
        {
            m_taxCollectors.Add(taxCollector);
            //TaxCollectorHandler.SendTaxCollectorMovementAddMessage(taxCollector.Guild.Clients, taxCollector);
        }

        public void RemoveTaxCollector(TaxCollectorNpc taxCollector)
        {
            m_taxCollectors.Remove(taxCollector);
            TaxCollectorManager.Instance.RemoveTaxCollectorSpawn(taxCollector);
            TaxCollectorHandler.SendTaxCollectorMovementRemoveMessage(taxCollector.Guild.Clients, taxCollector);
        }

        public void RemoveGuildMember(GuildMember member)
        {
            m_members.Remove(member);
            GuildManager.Instance.DeleteGuildMember(member);
        }

        public void RemoveTaxCollectors()
        {
            foreach (var taxCollector in m_taxCollectors.ToArray())
            {
                RemoveTaxCollector(taxCollector);
            }
        }

        public void RemoveGuildMembers()
        {
            foreach (var member in m_members.ToArray())
            {
                RemoveGuildMember(member);
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

            return (long) (amount*XP_PER_GAP[0][1]*0.01);
        }

        public void AddXP(long experience)
        {
            lock (m_lock)
            {
                Experience += experience;

                var level = ExperienceManager.Instance.GetGuildLevel(Experience);

                if (level == Level)
                    return;

                if (level > Level)
                    Boost += (uint) ((level - Level)*5);

                Level = level;
                OnLevelChanged();
            }
        }

        public void SetXP(long experience)
        {
            lock (m_lock)
            {
                Experience = experience;

                var level = ExperienceManager.Instance.GetGuildLevel(Experience);

                if (level == Level) return;

                Level = level;
                OnLevelChanged();
            }
        }

        public bool UpgradeTaxCollectorPods()
        {
            lock (m_lock)
            {
                if (TaxCollectorPods >= 5000)
                    return false;

                if (Boost <= 0)
                    return false;

                Boost -= 1;
                TaxCollectorPods += 20;

                if (TaxCollectorPods > 5000)
                    TaxCollectorPods = 5000;

                return true;
            }
        }

        public bool UpgradeTaxCollectorProspecting()
        {
            lock (m_lock)
            {
                if (TaxCollectorProspecting >= 500)
                    return false;

                if (Boost <= 0)
                    return false;

                Boost -= 1;
                TaxCollectorProspecting += 1;

                if (TaxCollectorProspecting > 500)
                    TaxCollectorProspecting = 500;

                return true;
            }
        }

        public bool UpgradeTaxCollectorWisdom()
        {
            lock (m_lock)
            {
                if (TaxCollectorWisdom >= 400)
                    return false;

                if (Boost <= 0)
                    return false;

                Boost -= 1;
                TaxCollectorWisdom += 1;

                if (TaxCollectorWisdom > 400)
                    TaxCollectorWisdom = 400;

                return true;
            }
        }

        public bool UpgradeMaxTaxCollectors()
        {
            lock (m_lock)
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
        }

        public bool UpgradeSpell(int spellId)
        {
            lock (m_lock)
            {
                var spellIndex = Array.IndexOf(TAX_COLLECTOR_SPELLS, (short) spellId);

                if (spellIndex == -1)
                    return false;

                if (Boost < 5)
                    return false;

                var spell = m_spells[spellIndex];

                if (spell == null)
                {
                    var template = SpellManager.Instance.GetSpellTemplate(spellId);

                    if (template == null)
                    {
                        logger.Error("Cannot boost tax collector spell {0}, template not found", spellId);
                        return false;
                    }

                    m_spells[spellIndex] = new Spell(template, 1);
                }
                else
                {
                    if (!spell.BoostSpell())
                        return false;
                }


                Boost -= 5;


                return true;
            }
        }

        public bool UnBoostSpell(int spellId)
        {
           lock (m_lock)
            {
                var spellIndex = Array.IndexOf(TAX_COLLECTOR_SPELLS, (short) spellId);

                if (spellIndex == -1)
                    return false;

                var spell = m_spells[spellIndex];

                if (spell == null)
                    return false;

                if (!spell.UnBoostSpell())
                    return false;

                Boost += 5;
                return true;
            }
        }

        public ReadOnlyCollection<Spell> GetTaxCollectorSpells()
        {
            return m_spells.Where(x => x != null).ToList().AsReadOnly();
        }

        public int[] GetTaxCollectorSpellsLevels() // faster
        {
            return m_spells.Select(x => x == null ? 0 : x.CurrentLevel).ToArray();
        }

        public GuildCreationResultEnum SetGuildName(Character character, string name)
        {
            var potion = character.Inventory.TryGetItem(ItemManager.Instance.TryGetTemplate(ItemIdEnum.GuildNameChangePotion));
            if (potion == null)
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_REQUIREMENT_UNMET;

            if (!Regex.IsMatch(name, "^([A-Z][a-z\u00E0-\u00FC']{2,14}(\\s|-)?)([A-Z]?[a-z\u00E0-\u00FC']{1,15}(\\s|-)?){0,2}([A-Z]?[a-z\u00E0-\u00FC']{1,15})?$", RegexOptions.Compiled))
            {
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_NAME_INVALID;
            }

            if (GuildManager.Instance.DoesNameExist(name))
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_NAME_ALREADY_EXISTS;

            character.Inventory.RemoveItem(potion, 1);

            Name = name;

            foreach (var taxCollector in TaxCollectors)
            {
                taxCollector.RefreshLook();
                taxCollector.Map.Refresh(taxCollector);
            }

            foreach (var client in Clients)
            {
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 383);
                GuildHandler.SendGuildMembershipMessage(client, client.Character.GuildMember);

                client.Character.RefreshActor();
            }

            return GuildCreationResultEnum.GUILD_CREATE_OK;
        }

        public GuildCreationResultEnum SetGuildEmblem(Character character, NetworkGuildEmblem emblem)
        {
            var potion = character.Inventory.TryGetItem(ItemManager.Instance.TryGetTemplate(ItemIdEnum.GuildEmblemChangePotion));
            if (potion == null)
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_REQUIREMENT_UNMET;

            if (GuildManager.Instance.DoesEmblemExist(emblem))
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_EMBLEM_ALREADY_EXISTS;

            character.Inventory.RemoveItem(potion, 1);

            Emblem.ChangeEmblem(emblem);

            foreach (var taxCollector in TaxCollectors)
            {
                taxCollector.RefreshLook();
                taxCollector.Map.Refresh(taxCollector);
            }

            foreach (var client in Clients)
            {
                client.Character.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 382);
                GuildHandler.SendGuildMembershipMessage(client, client.Character.GuildMember);

                client.Character.RefreshActor();
            }

            return GuildCreationResultEnum.GUILD_CREATE_OK;
        }

        public void SetBoss(GuildMember guildMember)
        {
            lock (m_lock)
            {
                if (guildMember.Guild != this)
                    return;

                if (Boss == guildMember)
                    return;

                WorldServer.Instance.IOTaskPool.AddMessage(() =>
                {
                    if (Boss != null)
                    {
                        var oldBoss = Boss;

                        oldBoss.RankId = 0;
                        oldBoss.Rights = GuildRightsBitEnum.GUILD_RIGHT_NONE;

                        // <b>%1</b> a remplacé <b>%2</b>  au poste de meneur de la guilde <b>%3</b>
                        BasicHandler.SendTextInformationMessage(m_clients,
                            TextInformationTypeEnum.TEXT_INFORMATION_ERROR, 199,
                            guildMember.Name, oldBoss.Name, Name);

                        UpdateMember(oldBoss);
                        oldBoss.Save(WorldServer.Instance.DBAccessor.Database);
                    }

                    guildMember.RankId = 1;
                    guildMember.Rights = GuildRightsBitEnum.GUILD_RIGHT_BOSS;

                    UpdateMember(guildMember);
                    guildMember.Save(WorldServer.Instance.DBAccessor.Database);
                });
            }
        }

        public bool KickMember(GuildMember kickedMember, bool kicked)
        {
            lock (m_lock)
            {
                if (kickedMember.IsBoss && m_members.Count > 1)
                    return false;

                if (!RemoveMember(kickedMember))
                    return false;

                foreach (var client in m_clients)
                {
                    GuildHandler.SendGuildMemberLeavingMessage(client, kickedMember, true);
                }

                if (kickedMember.IsBoss && m_members.Count == 0)
                    GuildManager.Instance.DeleteGuild(kickedMember.Guild);

                return true;
            }
        }

        public bool KickMember(Character kicker, GuildMember kickedMember)
        {
            lock (m_lock)
            {
                if (kicker.Guild != kickedMember.Guild)
                    return false;

                if (kicker.GuildMember != kickedMember &&
                    (!kicker.GuildMember.HasRight(GuildRightsBitEnum.GUILD_RIGHT_BAN_MEMBERS) || kickedMember.IsBoss))
                    return false;

                if (kicker.GuildMember.Id != kickedMember.Id)
                {
                    // Vous avez banni <b>%1</b> de votre guilde.
                    kicker.SendInformationMessage(TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 177,
                        kickedMember.Name);
                }

                return KickMember(kickedMember, kickedMember.Id == kicker.GuildMember.Id);
            }
        }

        public bool ChangeParameters(GuildMember member, short rank, byte xpPercent, uint rights)
        {
            lock (m_lock)
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
        }

        public bool ChangeParameters(Character modifier, GuildMember member, short rank, byte xpPercent, uint rights)
        {
            lock (m_lock)
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
                        {
                            if (rank >= 0 && rank <= 35)
                                member.RankId = rank;
                        }

                        if (modifier.GuildMember.HasRight(GuildRightsBitEnum.GUILD_RIGHT_MANAGE_RIGHTS))
                            member.Rights = (GuildRightsBitEnum) rights;
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
        }

        public void Save(ORM.Database database)
        {
            Record.Spells = GetTaxCollectorSpellsLevels();

            WorldServer.Instance.IOTaskPool.AddMessage(() =>
            {
                if (Record.IsNew)
                    database.Insert(Record);
                else
                    database.Update(Record);

                IsDirty = false;
                Record.IsNew = false;

                foreach (var member in Members.Where(x => !x.IsConnected && x.IsDirty))
                {
                    member.Save(database);
                }
            });
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
            lock (m_lock)
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
        }

        public bool RemoveMember(GuildMember member)
        {
            lock (m_lock)
            {
                if (member == null || !m_members.Contains(member))
                    return false;

                m_members.Remove(member);

                if (member.IsConnected)
                    m_clients.Remove(member.Character.Client);

                OnMemberRemoved(member);
                return true;
            }
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
            BasicHandler.SendTextInformationMessage(m_clients, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 208,
                Level);

            m_clients.Send(new GuildLevelUpMessage(Level));
        }

        private void OnMemberConnected(GuildMember member)
        {
            //Un membre de votre guilde, {player,%1,%2}, est en ligne.
            BasicHandler.SendTextInformationMessage(m_clients, TextInformationTypeEnum.TEXT_INFORMATION_MESSAGE, 224,
                member.Character.Name);

            m_clients.Add(member.Character.Client);

            UpdateMember(member);

            m_clients.Send(new GuildMemberOnlineStatusMessage(member.Id, true));
        }

        private void OnMemberDisconnected(GuildMember member, Character character)
        {
            m_clients.Remove(character.Client);

            UpdateMember(member);

            m_clients.Send(new GuildMemberOnlineStatusMessage(member.Id, false));
            m_clients.Send(new GuildMemberLeavingMessage(false, member.Id));
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

