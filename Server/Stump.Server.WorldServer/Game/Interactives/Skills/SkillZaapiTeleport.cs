using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Interactives;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator("ZaapiTeleport", typeof(Skill), typeof(int), typeof(InteractiveSkillRecord), typeof(InteractiveObject))]
    public class SkillZaapiTeleport : Skill
    {
        public SkillZaapiTeleport(int id, InteractiveSkillRecord record, InteractiveObject interactiveObject)
            : base(id, record, interactiveObject)
        {
        }

        public override bool IsEnabled(Character character)
        {
            return true;
        }

        public override void Execute(Character character)
        {
            var dialog = new ZaapiDialog(character, InteractiveObject);

            dialog.Open();
        }
    }
}
