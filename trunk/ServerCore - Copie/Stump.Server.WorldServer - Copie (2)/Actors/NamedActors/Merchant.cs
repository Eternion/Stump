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
using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Classes;
using Stump.DofusProtocol.Classes.Custom;
using Stump.Server.WorldServer.Global;

namespace Stump.Server.WorldServer.Entities
{
    public class Merchant : NamedActor, ITrader
    {

        public Merchant(long id, ExtendedLook look, VectorIsometric position, string name, uint sellType)
            : base(id, look, position, name)
        {
            SellType = sellType;
           // Guild = guild;
        }

        public uint SellType
        {
            get;
            set;
        }

        //public Guild Guild
        //{
        //    get;
        //    set;
        //}

        public GameRolePlayMerchantInformations ToGameRolePlayMerchantInformations()
        {
            //if(Guild == null)
            return new GameRolePlayMerchantInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), Name, SellType);
            //else
               // return new GameRolePlayMerchantWithGuildInformations((int)Id, Look.EntityLook, GetEntityDispositionInformations(), Name, SellType, Guild.GetInfo());
        }

    }
}