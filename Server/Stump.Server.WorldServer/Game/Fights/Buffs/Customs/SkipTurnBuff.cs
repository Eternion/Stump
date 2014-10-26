using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class SkipTurnBuff : Buff
    {
        public SkipTurnBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable) : base(id, target, caster, effect, spell, critical, dispelable)
        {
        }

        public SkipTurnBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable, short customActionId)
            : base(id, target, caster, effect, spell, critical, dispelable, customActionId)
        {
        }

        public override void Apply()
        {
            
        }

        public override void Dispell()
        {
            
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            return new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte) (Dispellable ? 0 : 1), (short) Spell.Id, Effect.Id, 0, 0);
        }
    }
}