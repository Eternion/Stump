using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Boss
{
    [BrainIdentifier((int)MonsterIdEnum.ROYALMOUTH_2854)]
    public class RoyalMouthBrain : Brain
    {
        public RoyalMouthBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.Team.FighterAdded += OnFighterAdded;
        }

        void OnFighterAdded(FightTeam team, FightActor fighter)
        {
            if (fighter != Fighter)
                return;

            Fighter.CastSpell(new Spell((int)SpellIdEnum.INIMOUTH, 1), Fighter.Cell, true);

            fighter.Team.FighterAdded -= OnFighterAdded;
        }
    }
}
