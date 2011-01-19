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
using Stump.Server.WorldServer.Fights;
using Stump.Server.WorldServer.Groups;
using Stump.Server.WorldServer.Spells;

namespace Stump.Server.WorldServer.Entities
{
    public interface ILivingEntity
    {
        /// <summary>
        ///   Set or get Level of the character.
        /// </summary>
        int Level
        {
            get;
            set;
        }

        int BaseHealth
        {
            get;
            set;
        }

        int DamageTaken
        {
            get;
            set;
        }

        StatsFields Stats
        {
            get;
        }

        /// <summary>
        ///   Spell container of this entity.
        /// </summary>
        SpellCollection Spells
        {
            get;
        }

        bool IsInFight
        {
            get;
        }

        Fight Fight
        {
            get;
        }

        FightGroup FightGroup
        {
            get;
        }

        FightGroupMember Fighter
        {
            get;
        }
    }
}