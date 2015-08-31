using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Guilds;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator("GuildCreation", typeof(Skill), typeof(int), typeof(InteractiveSkillRecord), typeof(InteractiveObject))]
    public class SkillGuildCreation : Skill
    {
        public SkillGuildCreation(int id, InteractiveSkillRecord record, InteractiveObject interactiveObject)
            : base (id, record, interactiveObject)
        {
        }

        public override bool IsEnabled(Character character)
        {
            return Record.IsConditionFilled(character);
        }

        public override void Execute(Character character)
        {
            if (character.IsBusy())
                return;

            var panel = new GuildCreationPanel(character);
            panel.Open();
        }
    }
}
