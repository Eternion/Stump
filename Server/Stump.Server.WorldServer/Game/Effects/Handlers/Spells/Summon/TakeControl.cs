using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Summon
{
    [EffectHandler(EffectsEnum.Effect_TakeControl)]
    public class TakeControl : SpellEffectHandler
    {
        public TakeControl(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                if (!(actor is SummonedMonster))
                    continue;

                var id = Caster.PopNextBuffId();
                var controlBuff = new TakeControlBuff(id, Caster, Caster, this, Spell, FightDispellableEnum.DISPELLABLE_BY_DEATH, actor as SummonedMonster)
                {
                    Duration = (short)Dice.Duration
                };

                Caster.AddBuff(controlBuff);
            }

            return true;
        }
    }
}
