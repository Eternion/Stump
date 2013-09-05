using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

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
            var members = Database.Fetch<CharacterRecord>(string.Format(CharacterRelator.FetchByGuildId, guildId));

            var guildMembers = new List<GuildMember>();

            foreach (var member in members)
            {
                Database.Connection.ChangeDatabase("stump_auth");
                
                var acctId =
                    Database.ExecuteScalar<int>(string.Format("SELECT 1 FROM worlds_characters WHERE CharacterId={0})",
                                                              member.Id));

                Database.Connection.ChangeDatabase("stump_world");

                var guildMember = new GuildMember(
                    member.Id,
                    ExperienceManager.Instance.GetCharacterLevel(member.Experience),
                    member.Name,
                    (sbyte) member.Breed,
                    member.Sex == SexTypeEnum.SEX_FEMALE,
                    1,
                    1000,
                    90,
                    262148,
                    1,
                    0,
                    1,
                    1,
                    acctId,
                    0);

                guildMembers.Add(guildMember);
            }

            return guildMembers;
        }
    }
}
