using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Boss
{
    [BrainIdentifier((int)MonsterIdEnum.KORRIANDRE_2968)]
    public class KorriandreBrain : Brain
    {
        public KorriandreBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.GetAlive += OnGetAlive;
        }

        private void OnGetAlive(FightActor obj)
        {
            Fighter.CastAutoSpell(new Spell((int)SpellIdEnum.KÈTE, 1), Fighter.Cell);
        }
    }

    [BrainIdentifier((int)MonsterIdEnum.SPORAKNE_2969)]
    public class SporakneBrain : Brain
    {
        public SporakneBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.GetAlive += OnGetAlive;
        }

        private void OnGetAlive(FightActor obj)
        {
            Fighter.CastAutoSpell(new Spell((int)SpellIdEnum.HAIMJI, 1), Fighter.Cell);
        }
    }

    [BrainIdentifier((int)MonsterIdEnum.MERULETTE_2900)]
    public class MeruletteBrain : Brain
    {
        public MeruletteBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.GetAlive += OnGetAlive;
        }

        private void OnGetAlive(FightActor obj)
        {
            Fighter.CastAutoSpell(new Spell((int)SpellIdEnum.MÉRULE_TRAÇON, 1), Fighter.Cell);
        }
    }
}
