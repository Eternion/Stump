using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Summons
{
    [BrainIdentifier((int)MonsterIdEnum.ARBRE_282)]
    public class TreeBrain : Brain
    {
        public TreeBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.Team.FighterAdded += OnFighterAdded;
        }

        void OnFighterAdded(FightTeam team, FightActor fighter)
        {
            if (Fighter != fighter)
                return;

            Fighter.CastSpell(new Spell((int)SpellIdEnum.FEUILLAGE, 1), Fighter.Cell, true);
            fighter.Team.FighterAdded -= OnFighterAdded;
        }
    }
}