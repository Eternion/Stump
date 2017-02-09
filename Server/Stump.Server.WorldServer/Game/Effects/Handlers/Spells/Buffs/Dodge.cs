using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_Dodge)]
    public class Dodge : SpellEffectHandler
    {
        public Dodge(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical) : base(effect, caster, castHandler, targetedCell, critical)
        {
        }
        
        protected override bool InternalApply()
        {
            foreach (var actor in GetAffectedActors())
            {
                if (actor.GetBuffs(x => x is DodgeBuff).Any())
                    continue;

                var buff = new DodgeBuff(actor.PopNextBuffId(), actor, Caster, this, Spell, Critical, FightDispellableEnum.DISPELLABLE, Dice.DiceNum, Dice.DiceFace);
                actor.AddBuff(buff);
            }

            return true;
        }
    }
}