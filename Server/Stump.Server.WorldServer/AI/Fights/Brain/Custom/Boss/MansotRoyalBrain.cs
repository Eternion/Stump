using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Game.Effects;

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

            foreach (var fighter in Fighter.Team.GetAllFighters())
            {
                fighter.Dead += OnActorDead;
            }
        }

        private void OnActorDead(FightActor actor, FightActor killer)
        {
            if (Fighter == actor)
            {
                Fighter.LifePointsChanged += OnLifePointsChanged;
                return;
            }

            if (Fighter.IsDead())
                return;

            var mansomonHandler = SpellManager.Instance.GetSpellCastHandler(Fighter, new Spell((int)SpellIdEnum.MANSOMON, 1), Fighter.Cell, false);
            mansomonHandler.Initialize();

            foreach (var handler in mansomonHandler.GetEffectHandlers())
            {
                handler.AddAffectedActor(Fighter);
            }

            mansomonHandler.Execute();
        }

        private void OnLifePointsChanged(FightActor fighter, int delta, int shieldDamages, int permanentDamages, FightActor from, EffectSchoolEnum school)
        {
            if (fighter != Fighter)
                return;

            if (delta <= 0)
                return;

            var spell = new Spell((int)SpellIdEnum.MANSOMURE, 1);
            Fighter.CastSpell(spell, Fighter.Cell, true, true);
        }
    }
}
