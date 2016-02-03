using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;

using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_Advance)]
    public class Advance : Pull
    {
        public Advance(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
        }

        public override int Priority => -1;

        public override bool Apply()
        {
            var affectedActors = GetAffectedActors(x => x.Cell == TargetedCell && x.Position.Point.IsOnSameLine(CastPoint));

            if (!affectedActors.Any())
                return false;

            SetAffectedActors(affectedActors);

            return base.Apply();
        }
    }
}
