using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Boss
{
    [BrainIdentifier((int)MonsterIdEnum.OBSIDIANTRE_2924)]
    public class ObsidiantreBrain : Brain
    {
        public ObsidiantreBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.GetAlive += OnGetAlive;
        }

        private void OnGetAlive(FightActor obj)
        {
            Fighter.CastAutoSpell(new Spell((int)SpellIdEnum.OBLIGATION, 1), Fighter.Cell);
        }
    }

    [BrainIdentifier((int)MonsterIdEnum.POUGNETTE_2951)]
    public class PougnetteBrain : Brain
    {
        public PougnetteBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.GetAlive += OnGetAlive;
        }

        private void OnGetAlive(FightActor obj)
        {
            Fighter.CastAutoSpell(new Spell((int)SpellIdEnum.CLAKO, 1), Fighter.Cell);
        }
    }
}
