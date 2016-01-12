using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Effects.Instances;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Spell = Stump.Server.WorldServer.Game.Spells.Spell;

namespace Stump.Server.WorldServer.Game.Effects.Handlers.Spells.Buffs
{
    [EffectHandler(EffectsEnum.Effect_Putsch)]
    public class Putsch : SpellEffectHandler
    {
        public Putsch(EffectDice effect, FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(effect, caster, spell, targetedCell, critical)
        {
        }

        public override bool Apply()
        {
            foreach (var actor in GetAffectedActors())
            {
                var buffId = actor.PopNextBuffId();
                var buff = new TriggerBuff(buffId, actor, Caster, Dice, Spell, Spell, false, false, PutschBuffTrigger);
                buff.SetTrigger(BuffTriggerType.AfterDamaged);
                actor.AddBuff(buff);
            }

            return true;
        }

        private void PutschBuffTrigger(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var damage = token as Fights.Damage;
            if (damage == null)
                return;

            if (damage.Spell == null)
                return;

            if (damage.Spell.Id == 0)
                return;

            if (damage.Source == null)
                return;

            var target = buff.Target;
            var damages = (int)(damage.Amount * (20 / 100d));
            var cells = target.Position.Point.GetAdjacentCells(x => !Fight.IsCellFree(target.Map.Cells[x]));

            foreach (var actor in cells.Select(cell => target.Fight.GetOneFighter(target.Map.Cells[cell.CellId])).Where(actor => actor != null))
            {
                var putschDamage = new Fights.Damage(damages)
                {
                    Source = target,
                    School = damage.School,
                    IsCritical = damage.IsCritical,
                    IgnoreDamageBoost = true,
                    IgnoreDamageReduction = false,
                    Spell = null
                };

                actor.InflictDamage(putschDamage);
            }

            var putschTargetDamage = new Fights.Damage(damages)
            {
                Source = damage.Source,
                School = damage.School,
                IsCritical = damage.IsCritical,
                IgnoreDamageBoost = true,
                IgnoreDamageReduction = false,
                Spell = null
            };

            target.InflictDamage(putschTargetDamage);
        }
    }
}
