using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Summons
{
    [BrainIdentifier((int)MonsterIdEnum.SAC_ANIME_237)]
    public class LivingBagBrain : Brain
    {
        public LivingBagBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.Team.FighterAdded += OnFighterAdded;
        }

        void OnFighterAdded(FightTeam team, FightActor fighter)
        {
            if (fighter != Fighter)
                return;

            fighter.CastSpell(new Spell((int)SpellIdEnum.SAC_RIFICE, (byte)fighter.Level), fighter.Cell, true, true, true, ignored: new[] { SpellCastResult.OK });
        }
    }
}
