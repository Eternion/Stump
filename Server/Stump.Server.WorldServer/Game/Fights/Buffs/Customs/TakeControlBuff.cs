using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;
using System;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class TakeControlBuff : Buff
    {
        public TakeControlBuff(int id, FightActor target, FightActor caster, SpellEffectHandler effect, Spell spell, FightDispellableEnum dispelable, SummonedMonster summon)
            : base(id, target, caster, effect, spell, false, dispelable)
        {
            Summon = summon;
        }

        public SummonedMonster Summon
        {
            get;
        }

        public override void Apply()
        {
            base.Apply();

            if (!(Caster is CharacterFighter))
                return;

            Summon.SetController(Caster as CharacterFighter);
        }

        public override void Dispell()
        {
            base.Dispell();
            Summon.SetController(null);
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var values = Effect.GetValues();

            if (Delay == 0)
                return new FightTemporaryBoostEffect(Id, Target.Id, Duration, (sbyte)Dispellable,
                    (short)Spell.Id, Effect.Id, 0, 0);

            return new FightTriggeredEffect(Id, Target.Id, Delay,
                (sbyte)Dispellable,
                (short)Spell.Id, Effect.Id, 0,
                (values.Length > 0 ? Convert.ToInt32(values[0]) : 0),
                (values.Length > 1 ? Convert.ToInt32(values[1]) : 0),
                (values.Length > 2 ? Convert.ToInt32(values[2]) : 0),
                Delay);
        }
    }
}
