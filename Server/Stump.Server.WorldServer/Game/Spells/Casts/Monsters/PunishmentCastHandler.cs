using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Buffs;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Monsters
{
    [SpellCastHandler(SpellIdEnum.PROPOLIS)]
    [SpellCastHandler(SpellIdEnum.GLOURSBI_BOULGA)]
    [SpellCastHandler(SpellIdEnum.RUSE_DU_WABBIT)]
    public class PunishmentCastHandler : DefaultSpellCastHandler
    {
        public PunishmentCastHandler(SpellCastInformations cast)
            : base(cast)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            var buffId = Caster.PopNextBuffId();
            var effect = Spell.CurrentSpellLevel.Effects[0];

            var buff = new TriggerBuff(buffId, Caster, Caster, Handlers[0], Spell, Spell, false, FightDispellableEnum.DISPELLABLE_BY_DEATH, 0, SpellBuffTrigger)
            {
                Duration = (short)effect.Duration
            };

            buff.SetTrigger(BuffTriggerType.AfterDamaged);
            Caster.AddBuff(buff);
        }

        private void SpellBuffTrigger(TriggerBuff buff, FightActor triggerrer, BuffTriggerType trigger, object token)
        {
            var damage = token as Damage;
            if (damage == null)
                return;

            if (damage.Source == null)
                return;

            var source = damage.Source;
            var target = buff.Target;

            if (damage.Source == target)
                return;

            if (target.Position.Point.IsAdjacentTo(source.Position.Point))
                return;

            foreach (var handler in Handlers)
            {
                handler.Apply();
            }
        }
    }
}
