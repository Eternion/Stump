using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Enums.Extensions;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Spells;
using Stump.Server.WorldServer.Handlers.Actions;
using System;
using Stump.Server.WorldServer.Game.Effects.Handlers.Spells;

namespace Stump.Server.WorldServer.Game.Fights.Buffs.Customs
{
    public class DodgeBuff : Buff
    {
        public DodgeBuff(int id, FightActor target, FightActor caster, SpellEffectHandler effect, Spell spell, bool critical, FightDispellableEnum dispelable, int dodgePercent, int backCellsCount)
            : base(id, target, caster, effect, spell, critical, dispelable)
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
            var buff = new TriggerBuff(id, Target, Caster, EffectHandler, Spell, Spell, Critical, Dispellable, Priority, EvasionBuffTrigger);

            buff.SetTrigger(BuffTriggerType.OnDamaged);
            Target.AddBuff(buff);
            base.Apply();
        }

        void EvasionBuffTrigger(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
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
            var direction = casterDirection.GetOpposedDirection();
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
                var pushbackDamages = Formulas.FightFormulas.CalculatePushBackDamages(source, target, BackCellsCount, 0);

                if (pushbackDamages > 0)
                {
                    var pushDamage = new Damage(pushbackDamages)
                    {
                        Source = target,
                        School = EffectSchoolEnum.Pushback,
                        IgnoreDamageBoost = true,
                        IgnoreDamageReduction = false
                    };

                    target.InflictDamage(pushDamage);
                }
            }
            else
            {
                target.Position.Cell = targetedCell;
                target.Fight.ForEach(entry => ActionsHandler.SendGameActionFightTeleportOnSameMapMessage(entry.Client, source, target, targetedCell));
            }
        }

        public override AbstractFightDispellableEffect GetAbstractFightDispellableEffect()
        {
            var turnDuration = Delay == 0 ? Duration : Delay;

            var values = Effect.GetValues();

            return new FightTriggeredEffect(Id, Target.Id, turnDuration,
                (sbyte)Dispellable,
                (short)Spell.Id, EffectFix?.ClientEffectId ?? Effect.Id, 0,
                (values.Length > 0 ? Convert.ToInt32(values[0]) : 0),
                (values.Length > 1 ? Convert.ToInt32(values[1]) : 0),
                (values.Length > 2 ? Convert.ToInt32(values[2]) : 0),
                Delay);
        }
    }
}