using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights;
using Stump.Server.WorldServer.Game.Fights.Buffs;
using Stump.Server.WorldServer.Game.Fights.Triggers;

namespace Stump.Server.WorldServer.Game.Spells.Casts.Sram
{
    [SpellCastHandler(SpellIdEnum.CONCENTRATION_DE_CHAKRA)]
    public class ChakraConcentrationCastHandler : DefaultSpellCastHandler
    {
        private FightActor[] m_affectedActors;

        public ChakraConcentrationCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            if (Handlers.Length != 1)
                return;

            m_affectedActors = Handlers[0].GetAffectedActors().ToArray();
            foreach (var target in m_affectedActors)
            {
                var id = target.PopNextBuffId();
                var buff = new TriggerBuff(id, target, Caster, Handlers[0].Dice, Spell, false, false, BuffTriggerType.BEFORE_ATTACKED, ChakraConcentrationBuffTrigger)
                {
                    Duration = 1
                };

                target.AddAndApplyBuff(buff);
            }
        }

        private void ChakraConcentrationBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            var damage = token as Damage;
            if (damage == null || !(damage.MarkTrigger is Trap))
                return;

            var trap = damage.MarkTrigger as Trap;

            foreach (var handler in Handlers)
            {
                handler.SetAffectedActors(m_affectedActors.Where(x => trap.ContainsCell(x.Cell)));
                handler.Apply();
            }
        }
    }
}