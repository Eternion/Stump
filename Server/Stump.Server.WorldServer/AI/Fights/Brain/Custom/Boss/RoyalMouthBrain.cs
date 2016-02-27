using System;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Boss
{
    [BrainIdentifier((int)MonsterIdEnum.ROYALMOUTH_2854)]
    public class RoyalMouthBrain : Brain
    {
        public RoyalMouthBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.Fight.TurnStarted += OnTurnStarted;
        }

        void OnTurnStarted(IFight fight, FightActor fighter)
        {
            if (fighter != Fighter)
                return;

            Fighter.CastSpell(new Spell((int)SpellIdEnum.INIMOUTH, 1), Fighter.Cell, true, true);

            fighter.Fight.TurnStarted -= OnTurnStarted;
        }
    }
}
