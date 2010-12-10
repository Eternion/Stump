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
	
	public class InventoryPresetItemRelinkErrorMessage : Message
	{
		public const uint protocolId = 6166;
		internal Boolean _isInitialized = false;
		public uint code = 1;
		
		public override uint getMessageId ()
		{
			return 6166;
		}
		
		public InventoryPresetItemRelinkErrorMessage initInventoryPresetItemRelinkErrorMessage (uint arg1 = 1)
		{
			this.code = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset ()
		{
			this.code = 1;
			this._isInitialized = false;
		}
		
		public override void pack (BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack (BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public void serialize (BigEndianWriter arg1)
		{
			this.serializeAs_InventoryPresetItemRelinkErrorMessage(arg1);
		}
		
		public void serializeAs_InventoryPresetItemRelinkErrorMessage (BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.code);
		}
		
		public void deserialize (BigEndianReader arg1)
		{
			this.deserializeAs_InventoryPresetItemRelinkErrorMessage(arg1);
		}
		
		public void deserializeAs_InventoryPresetItemRelinkErrorMessage (BigEndianReader arg1)
		{
			this.code = (uint)arg1.ReadByte();
			if ( this.code < 0 )
			{
				throw new Exception("Forbidden value (" + this.code + ") on element of InventoryPresetItemRelinkErrorMessage.code.");
			}
		}
		
	}
}
