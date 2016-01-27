using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Fight;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Game.Spells;
using System.Linq;

namespace Stump.Server.WorldServer.AI.Fights.Brain.Custom.Invocations
{
    [BrainIdentifier(3958)]
    public class SynchroBrain : Brain
    {
        public SynchroBrain(AIFighter fighter)
            : base(fighter)
        {
            fighter.Team.FighterAdded += OnFighterAdded;
        }

        void OnFighterAdded(FightTeam team, FightActor fighter)
        {
            if (fighter != Fighter)
                return;

            var spellHandler = SpellManager.Instance.GetSpellCastHandler(Fighter, new Spell((int)SpellIdEnum.SYNCHRONISATION, 1), Fighter.Cell, false);
            spellHandler.Initialize();

            var handlers = spellHandler.GetEffectHandlers().ToArray();

            Fighter.Fight.StartSequence(SequenceTypeEnum.SEQUENCE_SPELL);

            handlers[1].Apply(); //SubAP Summoner
            handlers[2].Apply(); //BuffTrigger

            Fighter.Fight.EndSequence(SequenceTypeEnum.SEQUENCE_SPELL);
        }
    }
}
