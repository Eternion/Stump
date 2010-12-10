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

namespace Stump.Server.WorldServer.Entities
{
    public class StatsHealth : StatsData
    {
        private static readonly Func<Entity, int, int, int, int, int, int> FormuleLife =
            (owner, valueBase, valueEquiped, valueGiven, valueBonus, damageTaken) => { return valueBase + valueEquiped + valueBonus + owner.Stats["Vitality"] - damageTaken; };

        protected new Func<Entity, int, int, int, int, int, int> m_formule;

        public StatsHealth(Entity owner, int valueBase, int damageTaken)
            : base(owner, "Health", valueBase)
        {
            m_formule = FormuleLife;
            DamageTaken = damageTaken;
        }

        public int DamageTaken
        {
            get;
            set;
        }

        /// <summary>
        ///   Addition of values
        /// </summary>
        public override int Total
        {
            get
            {
                if (m_formule != null)
                {
                    int result = m_formule(Owner, m_valueBase, m_valueEquiped, m_valueGiven, m_valueBonus, DamageTaken);

                    return result;
                }

                return 0;
            }
        }

        /// <summary>
        ///   Addition of values
        /// </summary>
        /// <remarks>
        ///   Value can't be lesser than 0
        /// </remarks>
        public override int TotalSafe
        {
            get
            {
                if (m_formule != null)
                {
                    int result = m_formule(Owner, m_valueBase, m_valueEquiped, m_valueGiven, m_valueBonus, DamageTaken);

                    return result < 0 ? 0 : result;
                }

                return 0;
            }
        }


        /// <summary>
        ///   Additions of values without using damages taken;
        /// </summary>
        public int TotalMax
        {
            get
            {
                if (m_formule != null)
                {
                    int result = m_formule(Owner, m_valueBase, m_valueEquiped, m_valueGiven, m_valueBonus, 0);

                    return result;
                }

                return 0;
            }
        }
    }
}