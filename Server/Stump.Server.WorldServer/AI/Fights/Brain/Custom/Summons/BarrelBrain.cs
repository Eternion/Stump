using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Summons
{
    [BrainIdentifier((int)MonsterIdEnum.TONNEAU_2727)]
    public class BarrelBrain : Brain
    {
        public BarrelBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.Fight.TurnStarted += OnTurnStarted;
        }

        private void OnTurnStarted(IFight fight, FightActor player)
        {
            if (player != Fighter)
                return;

            if (!(Fighter is SummonedMonster))
                return;

            var barrel = (SummonedMonster) Fighter;

            var spellBeuverie = barrel.Spells.FirstOrDefault(x => x.Value.Template.Id == (int)SpellIdEnum.BEUVERIE).Value;

            if (spellBeuverie == null)
                return;

            if (player.IsCarried())
                return;

            if (!barrel.Summoner.HasState((int) SpellStatesEnum.SAOUL_1) ||
                !barrel.Summoner.Position.Point.IsOnSameLine(barrel.Position.Point))
                return;

            var beuverieHandler = SpellManager.Instance.GetSpellCastHandler(Fighter, spellBeuverie, barrel.Summoner.Cell, false);

            using (Fighter.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL))
                beuverieHandler.Execute();
        }
    }
}
