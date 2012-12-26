using Stump.Server.WorldServer.Database;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    public class SkillZaapSave : Skill
    {
        public SkillZaapSave(int id, InteractiveSkillRecord record, InteractiveObject interactiveObject)
            : base(id, record, interactiveObject)
        {
        }

        public override bool IsEnabled(Character character)
        {
            return true;
        }

        public override void Execute(Character character)
        {
            character.SetSpawnPoint(InteractiveObject.Map);
        }
    }
}