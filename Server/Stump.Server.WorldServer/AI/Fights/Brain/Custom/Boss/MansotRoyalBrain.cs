using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Boss
{
    [BrainIdentifier((int)MonsterIdEnum.MANSOT_ROYAL_2848)]
    public class MansotRoyalBrain : Brain
    {
        public MansotRoyalBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.Fight.FightStarted += OnFightStarted;
        }

        private void OnFightStarted(IFight fight)
        {
            Fighter.CastSpell(new Spell((int)SpellIdEnum.MANSOMURE, 1), Fighter.Cell, true, true);

            fight.FightStarted -= OnFightStarted;
        }
    }
}