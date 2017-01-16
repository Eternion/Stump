using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Boss
{
    [BrainIdentifier((int)MonsterIdEnum.KOLOSSO_2986)]
    public class KolossoBrain : Brain
    {
        public KolossoBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.GetAlive += OnGetAlive;
        }

        private void OnGetAlive(FightActor obj)
        {
            Fighter.CastAutoSpell(new Spell((int)SpellIdEnum.PIAUTRE, 1), Fighter.Cell);
        }
    }
}
