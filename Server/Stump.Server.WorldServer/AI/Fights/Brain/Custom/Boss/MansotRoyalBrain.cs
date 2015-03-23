using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Boss
{
    [BrainIdentifier(2848)]
    public class MansotRoyalBrain : Brain
    {
        public MansotRoyalBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.Fight.FightStarted += OnFightStarted;
        }

        private void OnFightStarted(IFight fight)
        {
            var spell = new Spell((int) SpellIdEnum.MANSOMURE, 1);
            Fighter.CastSpell(spell, Fighter.Cell, true, true);

            foreach (var fighter in Fighter.Team.GetAllFighters().Where(fighter => fighter != Fighter))
            {
                fighter.Dead += OnActorDead;
            }
        }

        private void OnActorDead(FightActor actor, FightActor killer)
        {
            var mansomonHandler = SpellManager.Instance.GetSpellCastHandler(Fighter, new Spell((int)SpellIdEnum.MANSOMON, 1), Fighter.Cell, false);
            mansomonHandler.Initialize();

            foreach (var handler in mansomonHandler.GetEffectHandlers())
            {
                handler.AddAffectedActor(Fighter);
            }

            mansomonHandler.Execute();
        }
    }
}
