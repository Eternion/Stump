using Stump.Server.WorldServer.Database.Spells;
using Stump.Server.WorldServer.Game.Actors.Fight;

namespace Stump.Server.WorldServer.Game.Fights.History
{
    public class SpellHistoryEntry
    {
        readonly int? m_cooldownDuration;

        public SpellHistoryEntry(SpellHistory history, SpellLevelTemplate spell, FightActor caster, FightActor target, int castRound, int? cooldownDuration)
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
        }

        public SpellLevelTemplate Spell
        {
            get;
        }

        public FightActor Caster
        {
            get;
        }

        public FightActor Target
        {
            get;
        }

        public int CastRound
        {
            get;
        }

        public int CooldownDuration => m_cooldownDuration != null ? (int)m_cooldownDuration : (int)Spell.MinCastInterval;

        public int GetElapsedRounds(int currentRound) => currentRound - CastRound;

        public bool IsCooldownActive(int currentRound) => GetElapsedRounds(currentRound) < CooldownDuration;

        public bool IsGlobalCooldownActive(int currentRound) => GetElapsedRounds(currentRound) < (Spell.GlobalCooldown == -1 ? (int)Spell.MinCastInterval : Spell.GlobalCooldown);
    }
}