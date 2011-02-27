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

namespace Stump.Server.DataProvider.Data.Spells
{
    public class SpellLevelTemplate
    {
        public SpellTemplate Spell { get; set; }

        public uint ApCost { get; set; }

        public uint MinRange { get; set; }

        public uint Range { get; set; }

        public bool CastInLine { get; set; }

        public bool CastTestLos { get; set; }

        public uint CriticalHitProbability { get; set; }

        public uint CriticalFailureProbability { get; set; }

        public bool NeedFreeCell { get; set; }

        public bool NeedFreeTrapCell { get; set; }

        public bool RangeCanBeBoosted { get; set; }

        public uint MaxCastPerTurn { get; set; }

        public uint MaxCastPerTarget { get; set; }

        public uint MinCastInterval { get; set; }

        public uint MinPlayerLevel { get; set; }

        public bool CriticalFailureEndsTurn { get; set; }

        public List<int> StatsRequired { get; set; }

        public List<int> StatsForbidden { get; set; }

        //public EffectBase[] Effects
        //{
        //    get;
        //    set;
        //}

        //public EffectBase[] CriticalEffects
        //{
        //    get;
        //    set;
        //}

    }
}

