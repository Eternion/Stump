using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Summons
{
    [BrainIdentifier((int)MonsterIdEnum.LA_SACRIFIEE_116)]
    public class SacrifiedBrain : Brain
    {
        public SacrifiedBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.Team.FighterAdded += OnFighterAdded;
        }

        void OnFighterAdded(FightTeam team, FightActor fighter)
        {
            if (fighter != Fighter)
                return;

            Fighter.CastSpell(new Spell((int) SpellIdEnum.PRÉPARATION_POUPESQUE, 1), Fighter.Cell, true, true, ignored: new[] { SpellCastResult.OK });

            fighter.Team.FighterAdded -= OnFighterAdded;
        }
    }
}