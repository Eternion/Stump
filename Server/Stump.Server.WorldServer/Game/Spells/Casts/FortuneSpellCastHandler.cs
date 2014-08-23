using System.Linq;
using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Buffs;

namespace Stump.Server.WorldServer.Game.Spells.Casts
{
    [SpellCastHandler(SpellIdEnum.Fortune)]
    public class FortuneCastHandler : DefaultSpellCastHandler
    {
        private FightActor[] m_affectedActors; 

        public FortuneCastHandler(FightActor caster, Spell spell, Cell targetedCell, bool critical)
            : base(caster, spell, targetedCell, critical)
        {
        }

        public override void Execute()
        {
            if (!m_initialized)
                Initialize();

            // apply Handlers[0] this turn (Effect_AddDodge)
            Handlers[0].Apply();

            // delay Handlers[1] until next turn
            m_affectedActors = Handlers[1].GetAffectedActors().ToArray();
            foreach (var target in m_affectedActors)
            {
                var id = target.PopNextBuffId();
                var buff = new TriggerBuff(id, target, Caster, Handlers[1].Dice, Spell, false, false, BuffTriggerType.TURN_BEGIN, FortuneBuffTrigger);
                target.AddAndApplyBuff(buff);
            }
        }

        private void FortuneBuffTrigger(TriggerBuff buff, BuffTriggerType trigger, object token)
        {
            Handlers[1].SetAffectedActors(m_affectedActors);
            Handlers[1].Apply();
        }
    }
}