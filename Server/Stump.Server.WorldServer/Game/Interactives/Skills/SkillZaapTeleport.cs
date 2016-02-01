using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Dialogs.Interactives;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator((int)SkillTemplateEnum.BASE_ZAAP_USE, typeof(Skill), typeof(int), typeof(InteractiveSkillTemplate), typeof(InteractiveObject))]
    [Discriminator("ZaapTeleport", typeof(Skill), typeof(int), typeof(InteractiveCustomSkillRecord), typeof(InteractiveObject))]
    public class SkillZaapTeleport : Skill
    {
        public SkillZaapTeleport(int id, InteractiveSkillTemplate record, InteractiveObject interactiveObject)
            : base(id, record, interactiveObject)
        {
        }

        public SkillZaapTeleport(int id, InteractiveCustomSkillRecord record, InteractiveObject interactiveObject)
            : base(id, record.Template, interactiveObject)
        {
        }

        public override int StartExecute(Character character)
        {
            var dialog = new ZaapDialog(character, InteractiveObject, character.KnownZaaps);
            dialog.Open();

            return base.StartExecute(character);
        }
    }
}