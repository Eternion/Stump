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
	
	public class InventoryPresetItemUpdateMessage : Message
	{
		public const uint protocolId = 6168;
		internal Boolean _isInitialized = false;
		public uint presetId = 0;
		public PresetItem presetItem;
		
		public InventoryPresetItemUpdateMessage()
		{
			this.presetItem = new PresetItem();
		}
		
		public InventoryPresetItemUpdateMessage(uint arg1, PresetItem arg2)
			: this()
		{
			initInventoryPresetItemUpdateMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6168;
		}
		
		public InventoryPresetItemUpdateMessage initInventoryPresetItemUpdateMessage(uint arg1 = 0, PresetItem arg2 = null)
		{
			this.presetId = arg1;
			this.presetItem = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.presetId = 0;
			this.presetItem = new PresetItem();
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
			this.serializeAs_InventoryPresetItemUpdateMessage(arg1);
		}
		
		public void serializeAs_InventoryPresetItemUpdateMessage(BigEndianWriter arg1)
		{
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element presetId.");
			}
			arg1.WriteByte((byte)this.presetId);
			this.presetItem.serializeAs_PresetItem(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InventoryPresetItemUpdateMessage(arg1);
		}
		
		public void deserializeAs_InventoryPresetItemUpdateMessage(BigEndianReader arg1)
		{
			this.presetId = (uint)arg1.ReadByte();
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element of InventoryPresetItemUpdateMessage.presetId.");
			}
			this.presetItem = new PresetItem();
			this.presetItem.deserialize(arg1);
		}
		
	}
}
