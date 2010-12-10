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
namespace Stump.Server.WorldServer.Entities
{
    public sealed class MonsterGrade
    {
        #region Properties

        public int Grade
        {
            get;
            set;
        }

        public int MonsterId
        {
            get;
            set;
        }

        public int Level
        {
            get;
            set;
        }

        public int PaDodge
        {
            get;
            set;
        }

        public int PmDodge
        {
            get;
            set;
        }

        public int EarthResistance
        {
            get;
            set;
        }

        public int AirResistance
        {
            get;
            set;
        }

        public int FireResistance
        {
            get;
            set;
        }

        public int WaterResistance
        {
            get;
            set;
        }

        public int NeutralResistance
        {
            get;
            set;
        }

        public int LifePoints
        {
            get;
            set;
        }

        public int ActionPoints
        {
            get;
            set;
        }

        public int MovementPoints
        {
            get;
            set;
        }

        #endregion
    }
}