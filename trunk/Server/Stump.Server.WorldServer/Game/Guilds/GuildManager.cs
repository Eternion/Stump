using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Guilds;
using Stump.Server.WorldServer.Database.Characters;
using Stump.Server.WorldServer.Database.Accounts;
using Stump.Server.WorldServer.Game.Accounts;
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

        public int FindGuildIdByCharacter(Character character)
        {
            return Database.FirstOrDefault<GuildMemberRecord>(string.Format(GuildMemberRelator.FindByCharacterId,
                                                                         character.Id)).GuildId;
        }

        public List<GuildMember> GetGuildMembers(int guildId)
        {
            var members = Database.Fetch<CharacterRecord>(string.Format(GuildMemberRelator.FetchByGuildId, guildId));

            var guildMembers = new List<GuildMember>();

            foreach (var member in members)
            {
                var guildMemberRecord =
                    Database.FirstOrDefault<GuildMemberRecord>(string.Format(GuildMemberRelator.FindByCharacterId,
                                                                             member.Id));
                var connectedCharacter = AccountManager.Instance.FindById(guildMemberRecord.AccountId).ConnectedCharacter;
                var isConnected = (sbyte) (connectedCharacter != null && (sbyte)connectedCharacter ==
                                             member.Id
                                                 ? 1
                                                 : 0);

                var guildMember = new GuildMember(
                    member.Id,
                    ExperienceManager.Instance.GetCharacterLevel(member.Experience),
                    member.Name,
                    (sbyte) member.Breed,
                    member.Sex == SexTypeEnum.SEX_FEMALE,
                    guildMemberRecord.RankId,
                    guildMemberRecord.GivenExperience,
                    guildMemberRecord.GivenPercent,
                    262148, //Rights
                    isConnected,
                    (sbyte) member.AlignmentSide,
                    0, //Hours since last connection
                    0, //Mood SmileyId
                    guildMemberRecord.AccountId,
                    0); //AchievementPoints

                guildMembers.Add(guildMember);
            }

            return guildMembers;
        }
    }
}
