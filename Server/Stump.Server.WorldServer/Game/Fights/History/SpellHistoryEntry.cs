using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.History
{
    public class SpellHistoryEntry
    {
        private readonly int m_cooldownDuration;

        public SpellHistoryEntry(SpellHistory history, SpellLevelTemplate spell, FightActor caster, FightActor target, int castRound, int cooldownDuration)
        {
            History = history;
            Spell = spell;
            Caster = caster;
            Target = target;
            CastRound = castRound;
            m_cooldownDuration = cooldownDuration;
        }

        public SpellHistory History
        {
            get;
            private set;
        }

        public SpellLevelTemplate Spell
        {
            get;
            private set;
        }

        public FightActor Caster
        {
            get;
            private set;
        }

        public FightActor Target
        {
            get;
            private set;
        }

        public int CastRound
        {
            get;
            private set;
        }

        public int CooldownDuration
        {
            get { return m_cooldownDuration > 0 ? m_cooldownDuration : (int)Spell.MinCastInterval; }
        }

        public int GetElapsedRounds(int currentRound)
        {
            return currentRound - CastRound;
        }

        public bool IsGlobalCooldownActive(int currentRound)
        {
            return GetElapsedRounds(currentRound) < CooldownDuration;
        }
    }
}