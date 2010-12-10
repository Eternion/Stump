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
namespace Stump.Server.WorldServer.Global
{
    /// <summary>
    ///   Defines location (x, y) of every entities in the world.
    /// </summary>
    public sealed class Location
    {
        #region Fields

        /// <summary>
        ///   Empty location (x = 0, y = 0)
        /// </summary>
        public static readonly Location Empty = new Location();

        /// <summary>
        ///   X representation of entity's location
        /// </summary>
        private double m_x;

        /// <summary>
        ///   Y representation of entity's location
        /// </summary>
        private double m_y;

        #endregion

        #region Constructors

        /// <summary>
        ///   Default constructor
        /// </summary>
        public Location()
        {
            m_x = 0;
            m_y = 0;
        }

        public Location(double x, double y)
        {
            m_x = x;
            m_y = y;
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is Location)
            {
                var v = (Location) obj;
                if (v.m_x == m_x && v.m_y == m_y)
                    return obj.GetType().Equals(GetType());
            }
            return false;
        }

        #endregion

        #region Operators

        public static bool operator ==(Location u, Location v)
        {
            return u.X == v.m_x && u.m_y == v.m_y;
        }

        public static bool operator !=(Location u, Location v)
        {
            return u != v;
        }

        public static Location operator +(Location u, Location v)
        {
            return new Location(u.m_x + v.m_x, u.m_y + v.m_y);
        }

        public static Location operator -(Location u, Location v)
        {
            return new Location(u.m_x - v.m_x, u.m_y - v.m_y);
        }

        public static Location operator *(Location u, double a)
        {
            return new Location(a*u.m_x, a*u.m_y);
        }

        public static Location operator /(Location u, double a)
        {
            return new Location(u.m_x/a, u.m_y/a);
        }

        public static Location operator -(Location u)
        {
            return new Location(-u.m_x, -u.m_y);
        }

        #endregion

        #region Properties

        public double X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public double Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        #endregion
    }
}