// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
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
            set;
        }

        public int ApCost
        {
            get;
            set;
        }

        public int MinRange
        {
            get;
            set;
        }

        public int Range
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

        public int CriticalHitProbability
        {
            get;
            set;
        }

        public int CriticalFailureProbability
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

        public int MaxCastPerTurn
        {
            get;
            set;
        }

        public int MaxCastPerTarget
        {
            get;
            set;
        }

        public int MinCastInterval
        {
            get;
            set;
        }

        public int MinPlayerLevel
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