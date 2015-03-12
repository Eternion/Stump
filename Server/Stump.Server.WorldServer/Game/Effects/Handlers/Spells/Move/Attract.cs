using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_Attract)]
    public class Attract : Pull
    {
        public Attract(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var orientation = Caster.Position.Point.OrientationTo(TargetedPoint);
            Distance = Spell.CurrentSpellLevel.Range;

            for (var i = 1; i <= Distance; i++)
            {
                var cell = Caster.Position.Point.GetCellInDirection(orientation, (short)i);

                var fighter = Fight.GetOneFighter(Fight.Map.Cells[cell.CellId]);

                if (fighter == null)
                    continue;

                AddAffectedActor(fighter);
                break;
            }

            return base.Apply();
        }
    }
}
