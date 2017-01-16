using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Boss
{
    [BrainIdentifier((int)MonsterIdEnum.PROFESSEUR_XA_2992)]
    public class ProfesseurXaBrain : Brain
    {
        public ProfesseurXaBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.GetAlive += OnGetAlive;
        }

        private void OnGetAlive(FightActor obj)
        {
            Fighter.CastAutoSpell(new Spell((int)SpellIdEnum.TÉLÉPATHIE_2554, 1), Fighter.Cell);
        }
    }
}
