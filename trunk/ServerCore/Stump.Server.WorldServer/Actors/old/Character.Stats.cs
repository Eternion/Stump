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
using Stump.Server.WorldServer.Threshold;

namespace Stump.Server.WorldServer.Entities
{
    public partial class Character
    {

        public int StatsPoint
        {
            get;
            private set;
        }

        public int SpellsPoints
        {
            get;
            private set;
        }

        public uint Energy
        {
            get;
            private set;
        }

        public uint EnergyMax
        {
            get;
            private set;
        }

        public long Experience
        {
            get;
            private set;
        }

        public void GainExperience(long points)
        {
            Experience += points;

            NotifyExperienceGained(points);
            NotifyExperienceModified();
        }

        public void SetExperience(long experience)
        {
            Experience = experience;

            NotifyExperienceModified();
        }

        private void OnExperienceModified(Character character)
        {
            var level = (int)ThresholdManager.CharacterExp.GetLevel(Experience);
            var difference = level - Level;

            if (difference > 0)
            {
                NotifyLeveledUp((uint)difference);

                Level = level;
                ExperienceMax = ThresholdManager.CharacterExp.GetUpperBound(Level);
            }
            else if (level < Level)
            {
                NotifyLeveledDown((uint)(-difference));

                Level = level;
                ExperienceMax = ThresholdManager.CharacterExp.GetUpperBound(Level);
            }
        }
    }
}