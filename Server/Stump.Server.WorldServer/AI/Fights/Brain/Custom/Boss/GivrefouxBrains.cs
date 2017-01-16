using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Boss
{
    [BrainIdentifier((int)MonsterIdEnum.YOKAI_GIVREFOUX_2888)]
    public class YokaiBrain : Brain
    {
        public YokaiBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.GetAlive += OnGetAlive;
        }

        private void OnGetAlive(FightActor obj)
        {
            Fighter.CastAutoSpell(new Spell((int)SpellIdEnum.FOURRURE, 1), Fighter.Cell);
        }
    }

    [BrainIdentifier((int)MonsterIdEnum.YOMI_GIVREFOUX_2891)]
    public class YomiBrain : Brain
    {
        public YomiBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.GetAlive += OnGetAlive;
        }

        private void OnGetAlive(FightActor obj)
        {
            Fighter.CastAutoSpell(new Spell((int)SpellIdEnum.FOUDRE, 1), Fighter.Cell);
        }
    }

    [BrainIdentifier((int)MonsterIdEnum.TENGU_GIVREFOUX_2967)]
    public class TenguBrain : Brain
    {
        public TenguBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.GetAlive += OnGetAlive;
        }

        private void OnGetAlive(FightActor obj)
        {
            Fighter.CastAutoSpell(new Spell((int)SpellIdEnum.CROQUETTE, 1), Fighter.Cell);
        }
    }

    [BrainIdentifier((int)MonsterIdEnum.FUJI_GIVREFOUX_2970)]
    public class FujiBrain : Brain
    {
        public FujiBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.GetAlive += OnGetAlive;
        }

        private void OnGetAlive(FightActor obj)
        {
            Fighter.CastAutoSpell(new Spell((int)SpellIdEnum.INSTINCT_MATERNEL, 1), Fighter.Cell);
        }
    }
}
