using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;
using NetworkGuildEmblem = Stump.DofusProtocol.Types.GuildEmblem;
using Stump.Core.Cache;
using Stump.DofusProtocol.Types;

namespace Stump.Server.WorldServer.Game.Guilds
{
    public class GuildManager : DataManager<GuildManager>, ISaveable
    {
        UniqueIdProvider m_idProvider;
        Dictionary<int, Guild> m_guilds;
        Dictionary<int, EmblemRecord> m_emblems;
        Dictionary<int, GuildMember> m_guildsMembers;
        readonly Stack<Guild> m_guildsToDelete = new Stack<Guild>();
        readonly Stack<GuildMember> m_membersToDelete = new Stack<GuildMember>();
        ObjectValidator<GuildInformations[]> m_cachedGuilds;

        readonly object m_lock = new object();

        [Initialization(InitializationPass.Sixth)]
        public override void Initialize()
        {
            m_emblems = Database.Query<EmblemRecord>(EmblemRelator.FetchQuery).ToDictionary(x => x.Id);
            m_guildsMembers = Database.Fetch<GuildMemberRecord, CharacterRecord, GuildMemberRecord>(new GuildMemberRelator().Map,
                    GuildMemberRelator.FetchQuery).ToDictionary(x => x.CharacterId, x => new GuildMember(x));

            var membersByGuilds = m_guildsMembers.Values.GroupBy(x => x.Record.GuildId).ToDictionary(x => x.Key);
            m_guilds =
                Database.Query<GuildRecord>(GuildRelator.FetchQuery)
                        .Select(
                            x =>
                                new Guild(x,
                                    membersByGuilds.ContainsKey(x.Id)
                                        ? membersByGuilds[x.Id]
                                        : Enumerable.Empty<GuildMember>()))
                        .ToDictionary(x => x.Id);
            m_idProvider = m_guilds.Any()
                ? new UniqueIdProvider(m_guilds.Select(x => x.Value.Id).Max())
                : new UniqueIdProvider(1);


            foreach (var guild in m_guilds.Where(x => x.Value.Members.Count == 0).ToList())
                DeleteGuild(guild.Value);
            m_cachedGuilds = new ObjectValidator<GuildInformations[]>(BuildCachedGuilds, TimeSpan.FromMinutes(10));
            World.Instance.RegisterSaveableInstance(this);
        }

        public bool DoesNameExist(string name) => m_guilds.Any(x => string.Equals(x.Value.Name, name, StringComparison.CurrentCultureIgnoreCase));

        public bool DoesEmblemExist(NetworkGuildEmblem emblem) => m_guilds.Any(x => x.Value.Emblem.DoesEmblemMatch(emblem));

        public bool DoesEmblemExist(GuildEmblem emblem) => m_guilds.Any(x => x.Value.Emblem.DoesEmblemMatch(emblem));

        public List<Guild> GetGuilds() => m_guilds.Select(x => x.Value).ToList();

        public Guild TryGetGuild(int id)
        {
            lock (m_lock)
            {
                Guild guild;
                return m_guilds.TryGetValue(id, out guild) ? guild : null;
            }
        }

        public Guild TryGetGuild(string name)
        {
            lock (m_lock)
            {
                return m_guilds.FirstOrDefault(x => string.Equals(x.Value.Name, name, StringComparison.OrdinalIgnoreCase)).Value;
            }
        }

        public EmblemRecord TryGetEmblem(int id)
        {
            EmblemRecord record;
            return m_emblems.TryGetValue(id, out record) ? record : null;
        }

        public GuildMember TryGetGuildMember(int characterId)
        {
            lock (m_lock)
            {
                GuildMember guildMember;
                return m_guildsMembers.TryGetValue(characterId, out guildMember) ? guildMember : null;
            }
        }

        public Guild CreateGuild(string name)
        {
            lock (m_lock)
            {
                var id = m_idProvider.Pop();

                var record = new GuildRecord
                {
                    Id = id,
                    Name = name,
                    CreationDate = DateTime.Now,
                    IsNew = true,
                    Experience = 0,
                    Boost = 0,
                    Prospecting = 100,
                    Wisdom = 0,
                    Pods = 1000,
                    MaxTaxCollectors = 1,
                    EmblemBackgroundColor = Color.White.ToArgb(),
                    EmblemBackgroundShape = 1,
                    EmblemForegroundColor = Color.Black.ToArgb(),
                    EmblemForegroundShape = 1,
                    Spells = new int[0],
                };

                var guild = new Guild(record, new GuildMember[0]);
                m_guilds.Add(guild.Id, guild);

                return guild;
            }
        }

        public SocialGroupCreationResultEnum CreateGuild(Character character, string name, NetworkGuildEmblem emblem)
        {
            var guildalogemme = character.Inventory.TryGetItem(ItemManager.Instance.TryGetTemplate(ItemIdEnum.GUILDALOGEMME_1575));
            if (guildalogemme == null && !character.IsGameMaster())
                return SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_ERROR_REQUIREMENT_UNMET;

            if (!Regex.IsMatch(name, "^\\b[A-Z][A-Za-z\\s-']{4,30}\\b$", RegexOptions.Compiled) || Regex.IsMatch(name, "^\\s\\s$"))
            {
                return SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_ERROR_NAME_INVALID;
            }

            if (emblem.symbolShape >= 324)
                return SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_ERROR_EMBLEM_INVALID;

            if (DoesNameExist(name))
                return SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_ERROR_NAME_ALREADY_EXISTS;

            if (DoesEmblemExist(emblem))
                return SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_ERROR_EMBLEM_ALREADY_EXISTS;

            if (!character.IsGameMaster())
                character.Inventory.RemoveItem(guildalogemme, 1);

            var guild = CreateGuild(name);
            if (guild == null)
                return SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_ERROR_CANCEL;

            guild.Emblem.ChangeEmblem(emblem);

            GuildMember member;
            if (!guild.TryAddMember(character, out member))
            {
                DeleteGuild(guild);
                return SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_ERROR_CANCEL;
            }

            character.GuildMember = member;
            character.RefreshActor();

            return SocialGroupCreationResultEnum.SOCIAL_GROUP_CREATE_OK;
        }

        public bool DeleteGuild(Guild guild)
        {
            lock (m_lock)
            {
                guild.RemoveGuildMembers();
                guild.RemoveTaxCollectors();

                m_guilds.Remove(guild.Id);
                m_guildsToDelete.Push(guild);

                return true;
            }
        }

        public void RegisterGuildMember(GuildMember member)
        {
            lock (m_lock)
            {
                m_guildsMembers.Add(member.Id, member);
            }
        }

        public bool DeleteGuildMember(GuildMember member)
        {
            lock (m_lock)
            {
                m_guildsMembers.Remove(member.Id);
                m_membersToDelete.Push(member);

                return true;
            }
        }


        private GuildInformations[] BuildCachedGuilds() => m_guilds.Values.Select(x => x.GetGuildInformations()).ToArray();

        public GuildInformations[] GetCachedGuilds() => m_cachedGuilds;

        public void Save()
        {
            lock (m_lock)
            {
                foreach (var guild in m_guilds.Values.Where(guild => guild.IsDirty))
                {
                    guild.Save(Database);
                }

                while (m_guildsToDelete.Count > 0)
                {
                    var guild = m_guildsToDelete.Pop();

                    Database.Delete(guild.Record);
                }

                while (m_membersToDelete.Count > 0)
                {
                    var member = m_membersToDelete.Pop();

                    Database.Delete(member.Record);
                }
            }
        }
    }
}
