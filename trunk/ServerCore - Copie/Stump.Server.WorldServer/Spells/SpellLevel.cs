
using System;
using System.Collections.Generic;
using System.Linq;
using Stump.DofusProtocol.D2oClasses;
using Stump.Server.WorldServer.Effects;

namespace Stump.Server.WorldServer.Spells
{
    public class SpellLevel
    {
        #region Fields

        private EffectBase[] m_criticalEffect;
        private EffectBase[] m_effects;

        #endregion

        #region Properties

        public Spell Spell
        {
            get;
            internal set;
        }

        public uint ApCost
        {
            get;
            set;
        }

        public uint MinRange
        {
            get;
            set;
        }

        public uint Range
        {
            get;
            set;
        }

        public bool CastInLine
        {
            get;
            set;
        }

        public bool CastTestLos
        {
            get;
            set;
        }

        public uint CriticalHitProbability
        {
            get;
            set;
        }

        public uint CriticalFailureProbability
        {
            get;
            set;
        }

        public bool NeedFreeCell
        {
            get;
            set;
        }

        public bool NeedFreeTrapCell
        {
            get;
            set;
        }

        public bool RangeCanBeBoosted
        {
            get;
            set;
        }

        public uint MaxCastPerTurn
        {
            get;
            set;
        }

        public uint MaxCastPerTarget
        {
            get;
            set;
        }

        public uint MinCastInterval
        {
            get;
            set;
        }

        public uint MinPlayerLevel
        {
            get;
            set;
        }

        public bool CriticalFailureEndsTurn
        {
            get;
            set;
        }

        public bool HideEffects
        {
            get;
            set;
        }

        public List<int> StatsRequired
        {
            get;
            set;
        }

        public List<int> StatsForbidden
        {
            get;
            set;
        }

        public int Level
        {
            get;
            internal set;
        }

        #endregion

        public void SetEffects(IEnumerable<EffectInstance> effects)
        {
            m_effects = EffectManager.ConvertExportedEffect(effects);
        }

        public void SetEffects(IEnumerable<EffectBase> effects)
        {
            m_effects = effects.ToArray();
        }

        public EffectBase[] GetEffects()
        {
            return m_effects;
        }

        public void SetCriticalEffects(IEnumerable<EffectInstance> effects)
        {
            m_criticalEffect = EffectManager.ConvertExportedEffect(effects);
        }

        public void SetCriticalEffects(IEnumerable<EffectBase> effects)
        {
            m_criticalEffect = effects.ToArray();
        }

        public EffectBase[] GetCriticalEffects()
        {
            return m_criticalEffect;
        }
    }
}