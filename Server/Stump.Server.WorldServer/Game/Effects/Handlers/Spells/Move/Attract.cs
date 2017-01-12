using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

using Stump.Server.WorldServer.Game.Spells.Casts;
namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_Attract)]
    public class Attract : Push
    {
        public Attract(EffectDice effect, FightActor caster, SpellCastHandler castHandler, Cell targetedCell, bool critical)
            : base(effect, caster, castHandler, targetedCell, critical)
        {
            Pull = true;
            DamagesDisabled = true;
        }

        protected override bool InternalApply()
        {
            var orientation = Caster.Position.Point.OrientationTo(TargetedPoint);
            Distance = (int)Spell.CurrentSpellLevel.Range;

            for (var i = 1; i <= Distance; i++)
            {
                var cell = Caster.Position.Point.GetCellInDirection(orientation, (short)i);

                var fighter = Fight.GetOneFighter(Fight.Map.Cells[cell.CellId]);

                if (fighter == null)
                    continue;

                Distance = (int)TargetedPoint.ManhattanDistanceTo(fighter.Position.Point);
                AddAffectedActor(fighter);
                break;
            }

            return base.InternalApply();
        }
    }
}
