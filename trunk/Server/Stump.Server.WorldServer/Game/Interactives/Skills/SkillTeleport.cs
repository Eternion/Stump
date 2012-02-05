using Stump.Server.WorldServer.Database.Interactives.Skills;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    public class SkillTeleport : Skill
    {
        public SkillTeleport(int id, SkillTeleportRecord record, InteractiveObject interactiveObject)
            : base (id, record, interactiveObject)
        {
            TeleportRecord = record;
        }

        public SkillTeleportRecord TeleportRecord
        {
            get;
            private set;
        }

        public override bool IsEnabled(Character character)
        {
            return TeleportRecord.IsConditionFilled(character);
        }

        public override void Execute(Character character)
        {
            character.Teleport(TeleportRecord.GetPosition());
        }
    }
}