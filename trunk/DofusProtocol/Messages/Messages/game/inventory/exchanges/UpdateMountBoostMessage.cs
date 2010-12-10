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
	
	public class UpdateMountBoostMessage : Message
	{
		public const uint protocolId = 6179;
		internal Boolean _isInitialized = false;
		public double rideId = 0;
		public List<UpdateMountBoost> boostToUpdateList;
		
		public UpdateMountBoostMessage()
		{
			this.boostToUpdateList = new List<UpdateMountBoost>();
		}
		
		public UpdateMountBoostMessage(double arg1, List<UpdateMountBoost> arg2)
			: this()
		{
			initUpdateMountBoostMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6179;
		}
		
		public UpdateMountBoostMessage initUpdateMountBoostMessage(double arg1 = 0, List<UpdateMountBoost> arg2 = null)
		{
			this.rideId = arg1;
			this.boostToUpdateList = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.rideId = 0;
			this.boostToUpdateList = new List<UpdateMountBoost>();
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
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_UpdateMountBoostMessage(arg1);
		}
		
		public void serializeAs_UpdateMountBoostMessage(BigEndianWriter arg1)
		{
			arg1.WriteDouble(this.rideId);
			arg1.WriteShort((short)this.boostToUpdateList.Count);
			var loc1 = 0;
			while ( loc1 < this.boostToUpdateList.Count )
			{
				arg1.WriteShort((short)this.boostToUpdateList[loc1].getTypeId());
				this.boostToUpdateList[loc1].serialize(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_UpdateMountBoostMessage(arg1);
		}
		
		public void deserializeAs_UpdateMountBoostMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			object loc4 = null;
			this.rideId = (double)arg1.ReadDouble();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = (ushort)arg1.ReadUShort();
				(( loc4 = ProtocolTypeManager.GetInstance<UpdateMountBoost>((uint)loc3)) as UpdateMountBoost).deserialize(arg1);
				this.boostToUpdateList.Add((UpdateMountBoost)loc4);
				++loc2;
			}
		}
		
	}
}
