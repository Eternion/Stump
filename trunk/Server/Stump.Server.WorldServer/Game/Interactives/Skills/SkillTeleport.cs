using Stump.Server.WorldServer.Database.Interactives.Skills;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    public class SkillTeleport : Skill
    {
        public SkillTeleport(int id, SkillTeleportTemplate template, InteractiveObject interactiveObject)
            : base (id, template, interactiveObject)
        {
            TeleportTemplate = template;
        }

        public SkillTeleportTemplate TeleportTemplate
        {
            get;
            private set;
        }

        public override bool IsEnabled(Character character)
        {
            return TeleportTemplate.IsConditionFilled(character);
        }

        public override void Execute(Character character)
        {
            character.Teleport(TeleportTemplate.GetPosition());
        }
    }
}