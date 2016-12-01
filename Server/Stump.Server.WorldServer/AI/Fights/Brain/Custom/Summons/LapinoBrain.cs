using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Summons
{
    [BrainIdentifier((int)MonsterIdEnum.LAPINO_39)]
    public class LapinoBrain : Brain
    {
        public LapinoBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.Team.FighterAdded += OnFighterAdded;
        }

        void OnFighterAdded(FightTeam team, FightActor fighter)
        {
            if (Fighter != fighter)
                return;

            Fighter.CastSpell(new Spell((int)SpellIdEnum.MOT_STIMULANT_126, 1), Fighter.Summoner.Cell, true, true, ignored: new[] { SpellCastResult.OK });
            fighter.Team.FighterAdded -= OnFighterAdded;
        }
    }
}
