using System.Collections.Generic;
using NLog;
using Stump.Core.Attributes;
using Stump.Core.Collections;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Database.World;
using Stump.Server.WorldServer.Game.Actors.Fight;
using System.Linq;

namespace Stump.Server.WorldServer.Game.Fights.History
{
    public class SpellHistory
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [Variable]
        public static readonly int HistoryEntriesLimit = 60;

        private readonly LimitedStack<SpellHistoryEntry> m_underlyingStack = new LimitedStack<SpellHistoryEntry>(HistoryEntriesLimit);

        public SpellHistory(FightActor owner)
        {
            Owner = owner;
        }

        public FightActor Owner
        {
            get;
            private set;
        }

        private int CurrentRound
        {
            get { return Owner.Fight.TimeLine.RoundNumber; }
        }

        public void RegisterCastedSpell(SpellHistoryEntry entry)
        {
            m_underlyingStack.Push(entry);
        }

        public void RegisterCastedSpell(SpellLevelTemplate spell, FightActor target)
        {
            RegisterCastedSpell(new SpellHistoryEntry(this, spell, Owner, target, CurrentRound));
        }

        public bool CanCastSpell(SpellLevelTemplate spell, Cell targetedCell)
        {
            var mostRecentEntry = m_underlyingStack.LastOrDefault(entry => entry.Spell.Id == spell.Id);

            //check initial cooldown
            if (mostRecentEntry == null && CurrentRound < spell.InitialCooldown)
            {
                return false;
            }

            if (mostRecentEntry == null)
                return true;

            if (mostRecentEntry.IsGlobalCooldownActive(CurrentRound))
            {
                return false;
            }
            var castsThisRound = m_underlyingStack.Where(entry => entry.Spell.Id == spell.Id && entry.CastRound == CurrentRound).ToArray();

            if (castsThisRound.Length == 0)
                return true;

            if (spell.MaxCastPerTurn > 0 && castsThisRound.Length >= spell.MaxCastPerTurn)
            {
                return false;
            }

            var target = Owner.Fight.GetOneFighter(targetedCell);

            if (target == null)
                return true;

            var castsOnThisTarget = castsThisRound.Count(entry => entry.Target != null && entry.Target.Id == target.Id);

            if (spell.MaxCastPerTarget > 0 && castsOnThisTarget >= spell.MaxCastPerTarget)
            {
                return false;
            }

            return true;
        }
    }
}