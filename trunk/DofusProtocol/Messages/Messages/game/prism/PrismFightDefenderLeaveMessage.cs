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
	
	public class PrismFightDefenderLeaveMessage : Message
	{
		public const uint protocolId = 5892;
		internal Boolean _isInitialized = false;
		public double fightId = 0;
		public uint fighterToRemoveId = 0;
		public uint successor = 0;
		
		public PrismFightDefenderLeaveMessage()
		{
		}
		
		public PrismFightDefenderLeaveMessage(double arg1, uint arg2, uint arg3)
			: this()
		{
			initPrismFightDefenderLeaveMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5892;
		}
		
		public PrismFightDefenderLeaveMessage initPrismFightDefenderLeaveMessage(double arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.fightId = arg1;
			this.fighterToRemoveId = arg2;
			this.successor = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.fighterToRemoveId = 0;
			this.successor = 0;
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
			this.serializeAs_PrismFightDefenderLeaveMessage(arg1);
		}
		
		public void serializeAs_PrismFightDefenderLeaveMessage(BigEndianWriter arg1)
		{
			arg1.WriteDouble(this.fightId);
			if ( this.fighterToRemoveId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fighterToRemoveId + ") on element fighterToRemoveId.");
			}
			arg1.WriteInt((int)this.fighterToRemoveId);
			if ( this.successor < 0 )
			{
				throw new Exception("Forbidden value (" + this.successor + ") on element successor.");
			}
			arg1.WriteInt((int)this.successor);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismFightDefenderLeaveMessage(arg1);
		}
		
		public void deserializeAs_PrismFightDefenderLeaveMessage(BigEndianReader arg1)
		{
			this.fightId = (double)arg1.ReadDouble();
			this.fighterToRemoveId = (uint)arg1.ReadInt();
			if ( this.fighterToRemoveId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fighterToRemoveId + ") on element of PrismFightDefenderLeaveMessage.fighterToRemoveId.");
			}
			this.successor = (uint)arg1.ReadInt();
			if ( this.successor < 0 )
			{
				throw new Exception("Forbidden value (" + this.successor + ") on element of PrismFightDefenderLeaveMessage.successor.");
			}
		}
		
	}
}
