using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Spells;
using System.Linq;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Invocations
{
    [BrainIdentifier(3958)]
    public class SynchroBrain : Brain
    {
        bool m_initialized;

        public SynchroBrain(AIFighter fighter)
            : base(fighter)
        {
        }

        public override void Play()
        {
            if (m_initialized)
                return;

            var spellHandler = SpellManager.Instance.GetSpellCastHandler(Fighter, new Spell((int)SpellIdEnum.SYNCHRONISATION, 1), Fighter.Cell, false);
            spellHandler.Initialize();

            var handlers = spellHandler.GetEffectHandlers().ToArray();

            Fighter.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            handlers[1].Apply(); //SubAP Summoner
            handlers[2].Apply(); //BuffTrigger

            Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            m_initialized = true;
        }
    }
}
