using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Boss
{
    [BrainIdentifier((int)MonsterIdEnum.EL_PIKO_4609)]
    public class ElPikoBrain : Brain
    {
        public ElPikoBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.GetAlive += OnGetAlive;
        }

        private void OnGetAlive(FightActor obj)
        {
            Fighter.CastAutoSpell(new Spell((int)SpellIdEnum.GUERILLERO_7199, 1), Fighter.Cell);
        }
    }
}
