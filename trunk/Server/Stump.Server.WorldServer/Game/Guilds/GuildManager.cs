using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Stump.Core.Pool;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.BaseServer.Initialization;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Items;
using NetworkGuildEmblem = Stump.DofusProtocol.Types.GuildEmblem;

namespace Stump.Server.WorldServer.Game.Guilds
{
    public class GuildManager : DataManager<GuildManager>, ISaveable
    {
        private UniqueIdProvider m_idProvider;
        private Dictionary<int, Guild> m_guilds;
        private Dictionary<int, GuildMember> m_guildsMembers;
        private readonly Stack<Guild> m_guildsToDelete = new Stack<Guild>();
        private readonly Stack<GuildMember> m_guildsMemberToDelete = new Stack<GuildMember>();

        private readonly object m_lock = new object();

        [Initialization(InitializationPass.Sixth)]
        public override void Initialize()
        {
            m_guilds = Database.Query<GuildRecord>(GuildRelator.FetchQuery).ToList().Select(x => new Guild(x, FindGuildMembers(x.Id))).ToDictionary(x => x.Id);
            m_guildsMembers = m_guilds.Values.SelectMany(x => x.Members).ToDictionary(x => x.Id);
            m_idProvider = m_guilds.Any() ? new UniqueIdProvider(m_guilds.Select(x => x.Value.Id).Max()) : new UniqueIdProvider(1);

            World.Instance.RegisterSaveableInstance(this);
        }

        public bool DoesNameExist(string name)
        {
            return m_guilds.Any(x => x.Value.Name == name);
        }

        public bool DoesEmblemExist(NetworkGuildEmblem emblem)
        {
            return m_guilds.Any(x => x.Value.Emblem.DoesEmblemMatch(emblem));
        }

        public bool DoesEmblemExist(GuildEmblem emblem)
        {
            return m_guilds.Any(x => x.Value.Emblem.DoesEmblemMatch(emblem));
        }

        public GuildMember[] FindGuildMembers(int guildId)
        {
            return
                Database.Query<GuildMemberRecord>(string.Format(GuildMemberRelator.FetchByGuildId, guildId))
                        .Select(x => new GuildMember(x)).ToArray();
        }

        public Guild TryGetGuild(int id)
        {
            lock (m_lock)
            {
                Guild guild;
                return m_guilds.TryGetValue(id, out guild) ? guild : null;
            }
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
                var guild = new Guild(m_idProvider.Pop(), name);
                m_guilds.Add(guild.Id, guild);

                return guild;
            }
        }

        public GuildCreationResultEnum CreateGuild(Character character, string name, NetworkGuildEmblem emblem)
        {
            if (DoesNameExist(name))
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_NAME_ALREADY_EXISTS;

            if (DoesEmblemExist(emblem))
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_EMBLEM_ALREADY_EXISTS;

            var guildalogemme = character.Inventory.TryGetItem(ItemManager.Instance.TryGetTemplate(1575));
            if (guildalogemme == null)
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_REQUIREMENT_UNMET;

            character.Inventory.RemoveItem(guildalogemme, 1);

            var guild = CreateGuild(name);
            if (guild == null)
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_CANCEL;

            guild.Emblem.ChangeEmblem(emblem);

            GuildMember member;
            if (!guild.TryAddMember(character, out member))
            {
                DeleteGuild(guild);
                return GuildCreationResultEnum.GUILD_CREATE_ERROR_CANCEL;
            }

            character.GuildMember = member;
            character.RefreshActor();

            return GuildCreationResultEnum.GUILD_CREATE_OK;
        }

        public bool DeleteGuild(Guild guild)
        {
            lock (m_lock)
            {
                foreach (var member in guild.Members)
                {
                    DeleteGuildMember(member);
                }

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
                m_guildsMemberToDelete.Push(member);
                return true;
            }
        }

        public void Save()
        {
            lock (m_lock)
            {
                foreach (var guild in m_guilds.Values.Where(guild => guild.IsDirty))
                {
                    if (guild.Record.IsNew)
                        Database.Insert(guild.Record);
                    else
                        Database.Update(guild.Record);

                    guild.IsDirty = false;
                }

                /*foreach (var guildMember in m_guildsMembers.Values.Where(guildMember => guildMember.IsDirty))
                {
                    if (guildMember.Record.IsNew)
                        Database.Insert(guildMember.Record);
                    else
                        Database.Update(guildMember.Record);

                    guildMember.IsDirty = false;
                }*/

                while (m_guildsToDelete.Count > 0)
                {
                    var guild = m_guildsToDelete.Pop();

                    Database.Delete(guild.Record);
                }

                while (m_guildsMemberToDelete.Count > 0)
                {
                    var member = m_guildsMemberToDelete.Pop();

                    Database.Delete(member.Record);
                }
            }
        }
    }
}
