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
            Target.AddState(State);
        }

        public override void Dispell()
        {
            Target.RemoveState(State);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporaryBoostStateEffect(Id, Target.Id, Duration, (sbyte)(Dispelable ? 1 : 0), (short) Spell.Id, 0, 1, (short) State.Id);
        }
    }
}