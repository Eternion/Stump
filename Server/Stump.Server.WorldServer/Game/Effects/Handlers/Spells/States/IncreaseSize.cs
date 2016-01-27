using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.States
{
    [EffectHandler(EffectsEnum.Effect_IncreaseSize)]
    public class IncreaseSize : SpellEffectHandler
    {
        public IncreaseSize(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var look = actor.Look.Clone();

                look.AddScale(Dice.DiceNum);

                var buff = new SkinBuff(actor.PopNextBuffId(), actor, Caster, Dice, look, Spell, true);
                actor.AddBuff(buff);
            }

            return true;
        }
    }
}
