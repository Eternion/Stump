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
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;
using Stump.Server.WorldServer.Global.Maps;

namespace Stump.Server.WorldServer.Entities
{
    public class TaxCollector : Actor, IInventoryOwner, ITrader,IAttackable
    {

        protected TaxCollector(long id, ExtendedLook look, VectorIsometric position, uint firstNameId, uint lastNameId)
            :base(id,look,position)
        {
            FirstNameId = firstNameId;
            LastNameId = lastNameId;
            //Guild = Guild;
        }

        public uint FirstNameId
        {
            get;
            set;
        }

        public uint LastNameId
        {
            get;
            set;
        }

        //public Guild Guild
        //{
        //    get;
        //    set;
        //}


        public GameRolePlayTaxCollectorInformations ToGameRolePlayTaxCollectorInformations()
        {
           return null;
         //   return new GameRolePlayTaxCollectorInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(),);
        }

    }
}