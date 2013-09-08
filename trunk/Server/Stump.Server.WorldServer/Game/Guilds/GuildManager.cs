using System.Collections.Generic;
using System.Linq;
using Stump.Core.Pool;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Database.Guilds;

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

        public override void Initialize()
        {
            m_guilds =
                Database.Query<GuildRecord>(GuildRelator.FetchQuery).Select(x => new Guild(x, FindGuildMembers(x.Id))).ToDictionary(x => x.Id);
            m_guildsMembers = m_guilds.Values.SelectMany(x => x.Members).ToDictionary(x => x.Id);
            m_idProvider = new UniqueIdProvider(m_guilds.Select(x => x.Value.Id).Max());

            World.Instance.RegisterSaveableInstance(this);
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
                    Database.Save(guild.Record);
                    guild.IsDirty = false;
                }

                foreach (var guildMember in m_guildsMembers.Values.Where(guildMember => guildMember.IsDirty))
                {
                    Database.Save(guildMember.Record);
                    guildMember.IsDirty = false;
                }

                while (m_guildsToDelete.Count > 0)
                {
                    var guild = m_guildsToDelete.Pop();

                    Database.Delete(guild);
                }

                while (m_guildsMemberToDelete.Count > 0)
                {
                    var member = m_guildsMemberToDelete.Pop();

                    Database.Delete(member);
                }
            }
        }
    }
}
