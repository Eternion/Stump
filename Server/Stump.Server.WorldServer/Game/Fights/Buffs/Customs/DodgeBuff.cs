using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class DodgeBuff : Buff
    {
        public DodgeBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable, int dodgePercent, int backCellsCount) : base(id, target, caster, effect, spell, critical, dispelable)
        {
            DodgePercent = dodgePercent;
            BackCellsCount = backCellsCount;
        }

        public DodgeBuff(int id, FightActor target, FightActor caster, EffectBase effect, Spell spell, bool critical, bool dispelable, short customActionId, int dodgePercent, int backCellsCount) : base(id, target, caster, effect, spell, critical, dispelable, customActionId)
        {
            DodgePercent = dodgePercent;
            BackCellsCount = backCellsCount;
        }

        public int DodgePercent
        {
            get;
            set;
        }

        public int BackCellsCount
        {
            get;
            set;
        }

        public override void Apply()
        {
            var id = Target.PopNextBuffId();
            var buff = new TriggerBuff(id, Target, Caster, Effect as EffectDice, Spell, Critical, Dispellable,
                BuffTriggerType.BEFORE_ATTACKED, EvasionBuffTrigger);

            Target.AddAndApplyBuff(buff);
        }

        public override void Dispell()
        {
            /*var buffs = Target.GetBuffs(x => x.Effect.EffectId == EffectsEnum.Effect_Dodge);

            foreach(var buff in buffs)
                Target.RemoveBuff(buff);*/
        }

        private void EvasionBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var damage = token as Damage;
            if (damage == null)
                return;

            var target = buff.Target;
            var source = damage.Source;

            if (target == null)
                return;

            if (source == null)
                return;

            var cell = target.Position.Point;

            if (!cell.IsAdjacentTo(source.Position.Point))
                return;

            var casterDirection = cell.OrientationTo(source.Position.Point, false);
            var direction = cell.GetOppositeDirection(casterDirection);
            var nearestCell = cell.GetNearestCellInDirection(direction);

            if (nearestCell == null)
                return;

            var targetedCell = target.Map.Cells[nearestCell.CellId];

            damage.GenerateDamages();
            damage.Amount = 0;
            damage.IgnoreDamageBoost = true;
            damage.IgnoreDamageReduction = true;

            if (!target.Fight.IsCellFree(targetedCell))
            {
                var pushbackDamages = Formulas.FightFormulas.CalculatePushBackDamages(source, target, BackCellsCount);
                var pushDamage = new Damage(pushbackDamages)
                {
                    Source = target,
                    School = EffectSchoolEnum.Unknown,
                    IgnoreDamageBoost = true,
                    IgnoreDamageReduction = false
                };

                target.InflictDamage(pushDamage);
            }
            else
            {
                target.Position.Cell = targetedCell;
                target.Fight.ForEach(entry => ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(entry.Client, source, target, targetedCell));
            }
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, Duration, (sbyte)( Dispellable ? 0 : 1 ), (short)Spell.Id, 0, (short)values[0], (short)values[1], (short)values[2], 0);
        }
    }
}