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
                var character = World.Instance.GetCharacter(member.CharacterId);
                GuildMember guildMember;

                if (character != null)
                {
                    guildMember = new GuildMember(
                        member.CharacterId,
                        ExperienceManager.Instance.GetCharacterLevel(character.Experience),
                        character.Name,
                        (sbyte)character.Breed.Id,
                        character.Sex == SexTypeEnum.SEX_FEMALE,
                        member.RankId,
                        member.GivenExperience,
                        member.GivenPercent,
                        (uint)GuildRightsBitEnum.GUILD_RIGHT_BOSS, //Rights
                        1, //Connected
                        (sbyte) character.AlignmentSide,
                        0, //Hours since last connection
                        0, //Mood SmileyId(None = 0)
                        member.AccountId,
                        0); //AchievementPoints
                }
                else
                {
                    var characterRecord = Database.FirstOrDefault<CharacterRecord>(string.Format(GuildMemberRelator.FetchCharacterById, member.CharacterId));

                    guildMember = new GuildMember(
                        member.CharacterId,
                        ExperienceManager.Instance.GetCharacterLevel(characterRecord.Experience),
                        characterRecord.Name,
                        (sbyte)characterRecord.Breed,
                        characterRecord.Sex == SexTypeEnum.SEX_FEMALE,
                        member.RankId,
                        member.GivenExperience,
                        member.GivenPercent,
                        262148, //Rights
                        0, //Disconnected
                        (sbyte)characterRecord.AlignmentSide,
                        1, //Hours since last connection
                        0, //Mood SmileyId
                        member.AccountId,
                        0); //AchievementPoints   
                }

                guildMembers.Add(guildMember);
            }

            return guildMembers;
        }
    }
}
