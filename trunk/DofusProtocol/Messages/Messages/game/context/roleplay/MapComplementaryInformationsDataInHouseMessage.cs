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
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MapComplementaryInformationsDataInHouseMessage : MapComplementaryInformationsDataMessage
	{
		public const uint protocolId = 6130;
		internal Boolean _isInitialized = false;
		public HouseInformationsInside currentHouse;
		
		public MapComplementaryInformationsDataInHouseMessage()
		{
			this.currentHouse = new HouseInformationsInside();
		}
		
		public MapComplementaryInformationsDataInHouseMessage(uint arg1, uint arg2, int arg3, List<HouseInformations> arg4, List<GameRolePlayActorInformations> arg5, List<InteractiveElement> arg6, List<StatedElement> arg7, List<MapObstacle> arg8, List<FightCommonInformations> arg9, HouseInformationsInside arg10)
			: this()
		{
			initMapComplementaryInformationsDataInHouseMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
		}
		
		public override uint getMessageId()
		{
			return 6130;
		}
		
		public MapComplementaryInformationsDataInHouseMessage initMapComplementaryInformationsDataInHouseMessage(uint arg1 = 0, uint arg2 = 0, int arg3 = 0, List<HouseInformations> arg4 = null, List<GameRolePlayActorInformations> arg5 = null, List<InteractiveElement> arg6 = null, List<StatedElement> arg7 = null, List<MapObstacle> arg8 = null, List<FightCommonInformations> arg9 = null, HouseInformationsInside arg10 = null)
		{
			base.initMapComplementaryInformationsDataMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
			this.currentHouse = arg10;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.currentHouse = new HouseInformationsInside();
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_MapComplementaryInformationsDataInHouseMessage(arg1);
		}
		
		public void serializeAs_MapComplementaryInformationsDataInHouseMessage(BigEndianWriter arg1)
		{
			base.serializeAs_MapComplementaryInformationsDataMessage(arg1);
			this.currentHouse.serializeAs_HouseInformationsInside(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MapComplementaryInformationsDataInHouseMessage(arg1);
		}
		
		public void deserializeAs_MapComplementaryInformationsDataInHouseMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.currentHouse = new HouseInformationsInside();
			this.currentHouse.deserialize(arg1);
		}
		
	}
}
