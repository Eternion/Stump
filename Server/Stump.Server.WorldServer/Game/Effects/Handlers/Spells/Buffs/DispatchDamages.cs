using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_DispatchDamages)]
    public class DispatchDamages : SpellEffectHandler
    {
        public DispatchDamages(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                AddTriggerBuff(actor, false, BuffTriggerType.AfterDamaged, BuffTrigger);
            }

            return true;
        }

        void BuffTrigger(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            if (damage.IsWeaponAttack)
                return;

            if (damage.Source == null)
                return;

            var damages = (int)(damage.Amount * (Dice.DiceNum / 100.0));

            var reflectDamage = new Fights.Damage(damages)
            {
                Source = buff.Target,
                School = damage.School,
                IsCritical = damage.IsCritical,
                IgnoreDamageBoost = true,
                IgnoreDamageReduction = false,
                Spell = null
            };

            damage.Source.InflictDamage(reflectDamage);

            if (Spell.Id == (int)SpellIdEnum.PUTSCH)
            {
                var cells = buff.Target.Position.Point.GetAdjacentCells(x => !Fight.IsCellFree(buff.Target.Map.Cells[x]));

                foreach (var actor in cells.Select(cell => buff.Target.Fight.GetOneFighter(buff.Target.Map.Cells[cell.CellId])).Where(actor => actor != null))
                {
                    var baseDamage = new Fights.Damage(damages)
                    {
                        Source = buff.Target,
                        School = damage.School,
                        IsCritical = damage.IsCritical,
                        IgnoreDamageBoost = true,
                        IgnoreDamageReduction = false,
                        Spell = null
                    };

                    actor.InflictDamage(baseDamage);
                }
            }

            buff.Target.RemoveBuff(buff);
        }
    }
}