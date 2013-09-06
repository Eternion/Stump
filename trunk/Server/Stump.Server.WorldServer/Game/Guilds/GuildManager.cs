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
            var guildMember = Database.FirstOrDefault<GuildMemberRecord>(string.Format(GuildMemberRelator.FindByCharacterId,
                                                                         character.Id));
            return guildMember == null ? 0 : guildMember.GuildId;
        }

        public List<GuildMember> GetGuildMembers(int guildId)
        {
            var members = Database.Fetch<GuildMemberRecord>(string.Format(GuildMemberRelator.FetchByGuildId, guildId));

            var guildMembers = new List<GuildMember>();

            foreach (var member in members)
            {
                var characterRecord = Database.FirstOrDefault<CharacterRecord>(string.Format(GuildMemberRelator.FetchCharacterById, member.CharacterId));

                var connectedCharacter = AccountManager.Instance.FindById(member.AccountId).ConnectedCharacter;
                var isConnected = (sbyte) (connectedCharacter != null && (sbyte)connectedCharacter ==
                                             member.CharacterId
                                                 ? 1
                                                 : 0);

                var guildMember = new GuildMember(
                    member.CharacterId,
                    ExperienceManager.Instance.GetCharacterLevel(characterRecord.Experience),
                    characterRecord.Name,
                    (sbyte)characterRecord.Breed,
                    characterRecord.Sex == SexTypeEnum.SEX_FEMALE,
                    member.RankId,
                    member.GivenExperience,
                    member.GivenPercent,
                    262148, //Rights
                    isConnected,
                    (sbyte)characterRecord.AlignmentSide,
                    0, //Hours since last connection
                    0, //Mood SmileyId
                    member.AccountId,
                    0); //AchievementPoints

                guildMembers.Add(guildMember);
            }

            return guildMembers;
        }
    }
}
