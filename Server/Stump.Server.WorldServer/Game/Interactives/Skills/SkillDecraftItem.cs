using Stump.DofusProtocol.Enums;
using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Craft;
using Stump.Server.WorldServer.Game.Exchanges.Craft.Runes;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator((int)SkillTemplateEnum.BASE_181, typeof(Skill), typeof(int), typeof(InteractiveSkillTemplate), typeof(InteractiveObject))]
    public class SkillDecraftItem : Skill
    {
        public SkillDecraftItem(int id, InteractiveSkillTemplate record, InteractiveObject interactiveObject)
            : base(id, record, interactiveObject)
        {
        }

        public override int StartExecute(Character character)
        {
            var trade = new RuneTrade(character);
            trade.Open();
            return 0;
        }
    }
}