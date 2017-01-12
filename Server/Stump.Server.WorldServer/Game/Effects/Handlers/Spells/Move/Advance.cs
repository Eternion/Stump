using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells.Casts;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_Advance)]
    public class Advance : Push
    {
        public Advance(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
            Pull = true;
            DamagesDisabled = true;
        }

        protected override bool InternalApply()
        {
            var affectedActors = GetAffectedActors();

            if (!affectedActors.Any())
                return false;

            SetAffectedActors(new[] { Caster });

            return base.InternalApply();
        }
    }
}
