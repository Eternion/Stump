using Stump.Server.WorldServer.Database.Interactives.Skills;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Interactives;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    public class SkillZaapTeleport : Skill
    {
        public SkillZaapTeleport(int id, SkillRecord record, InteractiveObject interactiveObject)
            : base(id, record, interactiveObject)
        {
        }

        public override bool IsEnabled(Character character)
        {
            return true;
        }

        public override void Execute(Character character)
        {
            var dialog = new ZaapDialog(character, InteractiveObject, character.KnownZaaps);

            dialog.Open();
        }
    }
}