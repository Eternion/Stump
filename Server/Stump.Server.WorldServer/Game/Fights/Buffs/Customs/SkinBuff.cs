using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Actors.Look;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class SkinBuff : Buff
    {

        public SkinBuff(int id, FightActor target, FightActor caster, EffectBase effect, ActorLook look, Spell spell, bool dispelable)
            : base(id, target, caster, effect, spell, false, dispelable)
        {
            Look = look;
        }

        public SkinBuff(int id, FightActor target, FightActor caster, EffectBase effect, ActorLook look, Spell spell, bool dispelable, int priority, short customActionId)
            : base(id, target, caster, effect, spell, false, dispelable, priority, customActionId)
        {
            Look = look;
        }

        public ActorLook Look
        {
            get;
            set;
        }

        public ActorLook OriginalLook
        {
            get;
            private set;
        }

        public override void Apply()
        {
            base.Apply();
            OriginalLook = Target.Look.Clone();
            Target.Look = Look.Clone();

            ActionsHandler.SendGameActionFightChangeLookMessage(Target.Fight.Clients, Caster, Target, Target.Look);
        }

        public override void Dispell()
        {
            base.Dispell();
            Target.Look = OriginalLook.Clone();

            ActionsHandler.SendGameActionFightChangeLookMessage(Target.Fight.Clients, Caster, Target, Target.Look);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var values = Effect.GetValues();

            if (Delay == 0)
                return new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte)(Dispellable ? FightDispellableEnum.DISPELLABLE : FightDispellableEnum.DISPELLABLE_BY_DEATH), (short) Spell.Id, Effect.Id, 0, (short)values[2]);

            return new FightTriggeredEffect(Id, Target.Id, (short)(Duration + Delay), (sbyte)(Dispellable ? FightDispellableEnum.DISPELLABLE : FightDispellableEnum.DISPELLABLE_BY_DEATH), (short)Spell.Id, Effect.Id, 0, (short)values[0], (short)values[1], (short)values[2], Delay);
        }
    }
}