using Stump.DofusProtocol.Types;
using Stump.DofusProtocol.Types.Extensions;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class SkinBuff : Buff
    {

        public SkinBuff(int id, FightActor target, FightActor caster, EffectBase effect, EntityLook look, Spell spell, bool dispelable)
            : base(id, target, caster, effect, spell, false, dispelable)
        {
            Look = look;
        }

        public SkinBuff(int id, FightActor target, FightActor caster, EffectBase effect, EntityLook look, Spell spell, bool dispelable, short customActionId)
            : base(id, target, caster, effect, spell, false, dispelable, customActionId)
        {
            Look = look;
        }

        public EntityLook Look
        {
            get;
            set;
        }

        public EntityLook OriginalLook
        {
            get;
            private set;
        }

        public override void Apply()
        {
            OriginalLook = Target.Look.Copy();
            Target.Look = Look.Copy();
        }

        public override void Dispell()
        {
            Target.Look = OriginalLook.Copy();
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte)( Dispelable ? 1 : 0 ), (short) Spell.Id, 0, 1);
        }
    }
}