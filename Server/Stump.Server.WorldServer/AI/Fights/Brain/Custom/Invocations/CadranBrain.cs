using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Spells;
using System.Linq;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Invocations
{
    [BrainIdentifier(3960)]
    public class CadranBrain : Brain
    {
        public CadranBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.Team.FighterAdded += OnFighterAdded;
        }

        void OnFighterAdded(FightTeam team, FightActor fighter)
        {
            if (fighter != Fighter)
                return;

            fighter.CastSpell(new Spell((int)SpellIdEnum.OSCILLATION, 1), fighter.Cell, true, true);
        }
    }
}
