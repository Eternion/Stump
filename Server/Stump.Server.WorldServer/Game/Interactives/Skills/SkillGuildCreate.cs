using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Guilds;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator("GuildCreation", typeof(Skill), typeof(int), typeof(InteractiveCustomSkillRecord), typeof(InteractiveObject))]
    public class SkillGuildCreation : CustomSkill
    {
        public SkillGuildCreation(int id, InteractiveCustomSkillRecord record, InteractiveObject interactiveObject)
            : base (id, record, interactiveObject)
        {
        }

        public override int StartExecute(Character character)
        {

            var panel = new GuildCreationPanel(character);
            panel.Open();

            return base.StartExecute(character);
        }
    }
}
