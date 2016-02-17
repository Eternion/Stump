using Stump.DofusProtocol.Enums;
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

        public StateBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool dispelable, int priority, short customActionId, SpellState state)
            : base(id, target, caster, effect, spell, false, dispelable, priority, customActionId)
        {
            State = state;
        }

        public SpellState State
        {
            get;
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
                return new FightTemporaryBoostStateEffect(Id, Target.Id, Duration, (sbyte)(Dispellable ? FightDispellableEnum.DISPELLABLE : FightDispellableEnum.DISPELLABLE_BY_DEATH), (short)Spell.Id, Effect.Id, 0, 1, (short)State.Id);

            var values = new object[] { 0, 0, 0 };
            values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, (short)(Duration + Delay), (sbyte)(Dispellable ? FightDispellableEnum.DISPELLABLE : FightDispellableEnum.DISPELLABLE_BY_DEATH), (short)Spell.Id, Effect.Id, 0, (short)values[0], (short)values[1], (short)values[2], Delay);
        }
    }
}