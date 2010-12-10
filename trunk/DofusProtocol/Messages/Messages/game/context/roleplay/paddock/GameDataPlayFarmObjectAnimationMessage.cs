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
	
	public class GameDataPlayFarmObjectAnimationMessage : Message
	{
		public const uint protocolId = 6026;
		internal Boolean _isInitialized = false;
		public List<uint> cellId;
		
		public GameDataPlayFarmObjectAnimationMessage()
		{
			this.cellId = new List<uint>();
		}
		
		public GameDataPlayFarmObjectAnimationMessage(List<uint> arg1)
			: this()
		{
			initGameDataPlayFarmObjectAnimationMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6026;
		}
		
		public GameDataPlayFarmObjectAnimationMessage initGameDataPlayFarmObjectAnimationMessage(List<uint> arg1)
		{
			this.cellId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.cellId = new List<uint>();
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
			this.serializeAs_GameDataPlayFarmObjectAnimationMessage(arg1);
		}
		
		public void serializeAs_GameDataPlayFarmObjectAnimationMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.cellId.Count);
			var loc1 = 0;
			while ( loc1 < this.cellId.Count )
			{
				if ( this.cellId[loc1] < 0 || this.cellId[loc1] > 559 )
				{
					throw new Exception("Forbidden value (" + this.cellId[loc1] + ") on element 1 (starting at 1) of cellId.");
				}
				arg1.WriteShort((short)this.cellId[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameDataPlayFarmObjectAnimationMessage(arg1);
		}
		
		public void deserializeAs_GameDataPlayFarmObjectAnimationMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadShort()) < 0 || loc3 > 559 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of cellId.");
				}
				this.cellId.Add((uint)loc3);
				++loc2;
			}
		}
		
	}
}
