using Stump.Server.BaseServer.Database;
using Stump.Server.WorldServer.Database.Interactives;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Game.Exchanges.Paddock;
using Stump.Server.WorldServer.Game.Maps.Paddocks;

namespace Stump.Server.WorldServer.Game.Interactives.Skills
{
    [Discriminator("Paddock", typeof(Skill), typeof(int), typeof(InteractiveSkillRecord), typeof(InteractiveObject))]
    public class SkillPaddock : Skill
    {
        public SkillPaddock(int id, InteractiveSkillRecord record, InteractiveObject interactiveObject)
            : base (id, record, interactiveObject)
        {
        }

        public override bool IsEnabled(Character character)
        {
            return Record.IsConditionFilled(character);
        }

        public override void Execute(Character character)
        {
            if (character.IsBusy())
                return;

            var paddock = PaddockManager.Instance.GetPaddock(InteractiveObject.Map.Id);
            if (paddock == null)
                return;

            var exchange = new PaddockExchange(character, paddock);
            exchange.Open();
        }
    }
}
