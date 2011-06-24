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
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Entities;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.World.Entities.Actors;

namespace Stump.Server.WorldServer.World.Entities.Monsters
{
    public class MonsterGroup : Actor,IAttackable, IAligned
    {

        protected MonsterGroup(long id, ExtendedLook look, VectorIsometric position, uint firstNameId, uint lastNameId)
            :base(id,"",look,position)
        {
            //MainCreature = mainCreature;
            // Monsters = monsters;
            //AgeBonus = ageBonus;
            //AlignmentSide = alignmentSide;
        }

        //public Monster MainCreature
        //{
        //    get;
        //    set;
        //}

        //public List<Monster> Monsters
        //{
        //    get;
        //    set;
        //}

        //public int Agebonus
        //{
        //    get;
        //    set;
        //}

        //public int AlignmentSide
        //{
        //    get;
        //    set;
        //}


        public GameRolePlayGroupMonsterInformations ToGameRolePlayGroupMonsterInformations()
        {
            return new GameRolePlayGroupMonsterInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(),Monster.GID, Monster.Level, Monsters.ToMonsterInGroupInformations(),AgeBonus, AlignmentSide));
        }

        public void Attack()
        {
            throw new NotImplementedException();
        }

        public uint Level
        {
            get { throw new NotImplementedException(); }
        }
    }
}