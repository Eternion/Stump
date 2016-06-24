using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Interactives;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator("ZaapiTeleport", typeof(Skill), typeof(int), typeof(InteractiveCustomSkillRecord), typeof(InteractiveObject))]
    public class SkillZaapiTeleport : CustomSkill
    {
        public SkillZaapiTeleport(int id, InteractiveCustomSkillRecord record, InteractiveObject interactiveObject)
            : base(id, record, interactiveObject)
        {
        }

        public override bool IsEnabled(Character character) => base.IsEnabled(character) && Record.AreConditionsFilled(character);

        public override int StartExecute(Character character)
        {
            var dialog = new ZaapiDialog(character, InteractiveObject);
            dialog.Open();

            return base.StartExecute(character);
        }
    }
}
