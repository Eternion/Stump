using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Craft;
using Stump.Server.WorldServer.Game.Exchanges.Craft.Runes;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    public class SkillRuneCraft : Skill
    {
        public SkillRuneCraft(int id, InteractiveSkillTemplate skillTemplate, InteractiveObject interactiveObject)
            : base(id, skillTemplate, interactiveObject)
        {
        }

        public override int StartExecute(Character character)
        {
            var dialog = new SingleRuneMagicCraftDialog(character, InteractiveObject, this);
            dialog.Open();

            return base.StartExecute(character);
        }
    }
}