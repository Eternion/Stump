using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs.Customs;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_DamageSharing)]
    public class DamageSharing : SpellEffectHandler
    {
        public DamageSharing(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        protected override bool InternalApply()
        {
            var actors = GetAffectedActors(x => x is CharacterFighter && x.IsFriendlyWith(Caster)).ToArray();

            if (actors.Count() <= 1)
                return false;

            foreach (var actor in actors)
            {
                var buffId = actor.PopNextBuffId();
                var buff = new FractionBuff(buffId, actor, Caster, this, Spell, Critical, FightDispellableEnum.DISPELLABLE, actors);

                actor.AddBuff(buff);
            }

            return true;
        }
    }
}