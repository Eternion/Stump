using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class InvisibilityBuff : Buff
    {
        public InvisibilityBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable)
            : base(id, target, caster, effect, spell, critical, dispelable)
        {
        }

        public InvisibilityBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable, short customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, customActionId)
        {
        }

        public override void Apply()
        {
            Target.SetInvisibilityState(GameActionFightInvisibilityStateEnum.INVISIBLE);
        }

        public override void Dispell()
        {
            Target.SetInvisibilityState(GameActionFightInvisibilityStateEnum.VISIBLE);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte)( Dispelable ? 1 : 0 ), (short) Spell.Id, 0, 1);
        }
    }
}