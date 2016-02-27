using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Monsters
{
    [BrainIdentifier((int)MonsterIdEnum.SPORAKNE_2969)]
    public class SporakneBrain : Brain
    {
        public SporakneBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.Fight.TurnStarted += OnTurnStarted;
        }

        private void OnTurnStarted(IFight fight, FightActor fighter)
        {
            if (fighter != Fighter)
                return;

            fighter.CastSpell(new Spell((int)SpellIdEnum.HAIMJI, 1), fighter.Cell, true, true);
            fighter.Fight.TurnStarted -= OnTurnStarted;
        }
    }
}
