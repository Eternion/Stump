using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Move
{
    [EffectHandler(EffectsEnum.Effect_SymetricTargetTeleport)]
    [EffectHandler(EffectsEnum.Effect_SymetricCasterTeleport)]
    public class SymetricTeleport : SpellEffectHandler
    {
        public SymetricTeleport(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            var target = GetAffectedActors().FirstOrDefault();

            if (target == null)
                return false;

            var casterPoint = Caster.Position.Point;
            var targetPoint = target.Position.Point;

            if (Effect.EffectId == EffectsEnum.Effect_SymetricCasterTeleport)
            {
                casterPoint = target.Position.Point;
                targetPoint = Caster.Position.Point;
            }

            var distance = casterPoint.EuclideanDistanceTo(targetPoint);
            var direction = casterPoint.OrientationTo(targetPoint, true);

            var cell = targetPoint.GetCellInDirection(direction, (short)distance);

            if (cell == null)
                return false;

            var dstCell = Map.GetCell(cell.CellId);

            if (dstCell == null)
                return false;

            if (!dstCell.Walkable)
                return false;

            if (!Fight.IsCellFree(dstCell))
            {
                var fighter = Fight.GetOneFighter(dstCell);
                if (fighter != null)
                {
                    var caster = Caster;

                    if (Effect.EffectId == EffectsEnum.Effect_SymetricCasterTeleport)
                        caster = target;

                    if (!caster.Telefrag(fighter))
                        return false;

                    EffectDice effectAddAP = null;
                    EffectDice effectAddState = null;

                    switch (Spell.Id)
                    {
                        case (int)SpellIdEnum.TÉLÉPORTATION_88:
                            effectAddAP = Spell.CurrentSpellLevel.Effects[1];
                            effectAddState = Spell.CurrentSpellLevel.Effects[2];
                            break;
                        case (int)SpellIdEnum.FRAPPE_DE_XÉLOR:
                            effectAddAP = Spell.CurrentSpellLevel.Effects[2];
                            effectAddState = Spell.CurrentSpellLevel.Effects[3];
                            break;
                    }

                    if (effectAddAP == null || effectAddState == null)
                        return false;

                    if (!ApplyEffect(effectAddAP, Caster))
                        return false;

                    if (!ApplyEffect(effectAddState, fighter))
                        return false;

                    if (caster != Caster)
                    {
                        if (!ApplyEffect(effectAddState, caster))
                            return false;
                    }
                }
            }
            else
            {
                Caster.Position.Cell = dstCell;
                Fight.ForEach(entry => ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(entry.Client, Caster, Caster, dstCell), true);
            }

            return true;
        }

        private bool ApplyEffect(EffectDice effect, FightActor target)
        {
            var handler = EffectManager.Instance.GetSpellEffectHandler(effect, Caster, Spell, target.Position.Cell, Critical);
            handler.AddAffectedActor(target);

            if (!handler.CanApply())
                return false;

            handler.Apply();

            return true;
        }
    }
}
