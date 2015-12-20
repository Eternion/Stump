using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Craft;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    public class SkillCraft : Skill
    {
        public SkillCraft(int id, InteractiveSkillTemplate skillTemplate, InteractiveObject interactiveObject) : base(id, skillTemplate, interactiveObject)
        {
        }

        public override bool IsEnabled(Character character)
        {
            return true;
        }

        public override int StartExecute(Character character)
        {
            var dialog = new CraftDialog(character, InteractiveObject, this);
            dialog.Open();

            return 0;
        }
    }
}