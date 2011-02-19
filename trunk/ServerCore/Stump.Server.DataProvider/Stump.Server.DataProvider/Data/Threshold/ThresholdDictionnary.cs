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
using System;
using System.Collections.Generic;
using System.Linq;

namespace Stump.Server.DataProvider.Data.Threshold
{
    public class ThresholdDictionnary
    {
        private readonly Dictionary<uint, long> m_levels;

        private readonly string m_name;

        public ThresholdDictionnary(string name, Dictionary<uint, long> thresholds)
        {
            m_name = name;
            m_levels = thresholds;
        }

        public string Name
        {
            get { return m_name; }
        }

        public long GetLowerBound(uint level)
        {
            if (m_levels.ContainsKey(level))
                return m_levels[level];
            throw new Exception("Level " + level + " not found in " + Name + " threshold");
        }

        public long GetLowerBound(long experience)
        {
            return GetLowerBound(GetLevel(experience));
        }

        public long GetUpperBound(uint level)
        {
            return GetLowerBound(level + 1);
        }

        public long GetUpperBound(long experience)
        {
            return GetUpperBound(GetLevel(experience));
        }

        public uint GetLevel(long experience)
        {
            try
            {
                return m_levels.First(l => l.Value > experience).Key - 1;
            }
            catch (InvalidOperationException ex)
            {
                throw new Exception(
                    string.Format("Experience {0} not bind to a level in {1} threshold", experience, Name), ex);
            }
        }
    }
}