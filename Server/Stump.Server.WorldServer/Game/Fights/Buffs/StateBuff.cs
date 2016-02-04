using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs
{
    public class StateBuff : Buff
    {
        public StateBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool dispelable, SpellState state)
            : base(id, target, caster, effect, spell, false, dispelable)
        {
            State = state;
        }

        public StateBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool dispelable, short customActionId, SpellState state)
            : base(id, target, caster, effect, spell, false, dispelable, customActionId)
        {
            State = state;
        }

        public SpellState State
        {
            get;
            private set;
        }

        public override void Apply()
        {
            base.Apply();
            Target.AddState(State);
        }

        public override void Dispell()
        {
            base.Dispell();
            Target.RemoveState(State);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            if (Delay == 0)
                return new FightTemporaryBoostStateEffect(Id, Target.Id, Duration, (sbyte)(Dispellable ? 1 : 0), (short)Spell.Id, Effect.Id, 0, 1, (short)State.Id);

            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, (short)(Duration + Delay), (sbyte)(Dispellable ? 1 : 0), (short)Spell.Id, Effect.Id, 0, (short)values[0], (short)values[1], (short)values[2], Delay);
        }
    }
}