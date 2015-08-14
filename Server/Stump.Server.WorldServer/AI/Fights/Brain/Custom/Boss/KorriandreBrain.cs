using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Boss
{
    [BrainIdentifier(2968)]
    public class KorriandreBrain : Brain
    {
        public KorriandreBrain(AIFighter fighter) : base(fighter)
        {
            fighter.Fight.TurnStarted += OnTurnStarted;
        }

        private void OnTurnStarted(IFight fight, FightActor player)
        {
            if (!(player is CharacterFighter) || player.Team == Fighter.Team)
                return;

            var spell = new Spell((int)SpellIdEnum.DAIPIPAY, 1);
            player.CastSpell(spell, player.Cell, true);
        }
    }
}
