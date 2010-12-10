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

namespace Stump.DofusProtocol.D2oClasses
{
    [AttributeAssociatedFile("SpellLevels")]
    public class SpellLevel
    {
        public int apCost;
        public bool castInLine;
        public bool castTestLos;
        public List<EffectInstanceDice> criticalEffect;
        public bool criticalFailureEndsTurn;
        public int criticalFailureProbability;
        public int criticalHitProbability;
        public List<EffectInstanceDice> effects;
        public bool hideEffects;
        public int id;
        public int maxCastPerTarget;
        public int maxCastPerTurn;
        public int minCastInterval;
        public int minPlayerLevel;
        public int minRange;
        public bool needFreeCell;
        public bool needFreeTrapCell;
        public int range;
        public bool rangeCanBeBoosted;
        public int spellBreed;
        public int spellId;
        public List<int> statesForbidden;
        public List<int> statesRequired;
    }
}