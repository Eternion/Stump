using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Invocations
{
    [BrainIdentifier(2727)]
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
            var spellTournee = barrel.Spells.FirstOrDefault(x => x.Value.Template.Id == (int)SpellIdEnum.TOURNÉE_GÉNÉRALE).Value;

            if (spellBeuverie == null || spellTournee == null)
                return;

            if (player.IsCarried() && player.GetCarryingActor() == barrel.Summoner)
            {
                barrel.CastSpell(spellTournee, barrel.Cell);
                return;
            }

            if (!barrel.Summoner.HasState((int) SpellStatesEnum.Drunk) ||
                !barrel.Summoner.Position.Point.IsOnSameLine(barrel.Position.Point))
                return;

            var beuverieHandler = SpellManager.Instance.GetSpellCastHandler(Fighter,
                new Spell((int)SpellIdEnum.BEUVERIE, spellBeuverie.CurrentLevel), barrel.Summoner.Cell, false);

            Fighter.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);
            beuverieHandler.Execute();
            Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);
        }
    }
}
