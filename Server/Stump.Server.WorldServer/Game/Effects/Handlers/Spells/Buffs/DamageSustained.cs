using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Stump.Server.WorldServer.Game.Spells;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_DamageSustained)]
    public class DamageSustained : SpellEffectHandler
    {
        public DamageSustained(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var actualBuff = actor.GetBuffs(x => x is DamageSustainedBuff).FirstOrDefault();
                if (actualBuff != null)
                    actualBuff.Dispell();

                var buff = new DamageSustainedBuff(actor.PopNextBuffId(), actor, Caster, Dice, Spell, Critical, true, Dice.DiceNum);
                actor.AddAndApplyBuff(buff);
            }

            return true;
        }
    }
}
