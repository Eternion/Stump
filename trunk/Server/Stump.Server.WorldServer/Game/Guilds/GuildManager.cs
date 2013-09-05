using System.Collections.Generic;
using Stump.Server.BaseServer.Database;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.WorldServer.Database.Characters;

namespace Stump.Server.WorldServer.Game.Guilds
{
    public class GuildManager : DataManager<GuildManager>
    {
        /// <summary>
        /// Returns null if not found
        /// </summary>
        /// <param name="guildId"></param>
        /// <returns></returns>
        public GuildRecord FindById(int guildId)
        {
            return Database.FirstOrDefault<GuildRecord>(string.Format(GuildRelator.FetchById, guildId));
        }

        public List<GuildMember> GetGuildMembers(int guildId)
        {
            return Database.Fetch<GuildMember>(string.Format(CharacterRelator.FetchByGuildId, guildId));
        }
    }
}
