using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InventoryPresetSaveMessage : Message
	{
		public const uint protocolId = 6165;
		internal Boolean _isInitialized = false;
		public uint presetId = 0;
		public uint symbolId = 0;
		public Boolean saveEquipment = false;
		
		public InventoryPresetSaveMessage()
		{
		}
		
		public InventoryPresetSaveMessage(uint arg1, uint arg2, Boolean arg3)
			: this()
		{
			initInventoryPresetSaveMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6165;
		}
		
		public InventoryPresetSaveMessage initInventoryPresetSaveMessage(uint arg1 = 0, uint arg2 = 0, Boolean arg3 = false)
		{
			this.presetId = arg1;
			this.symbolId = arg2;
			this.saveEquipment = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.presetId = 0;
			this.symbolId = 0;
			this.saveEquipment = false;
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
			this.serializeAs_InventoryPresetSaveMessage(arg1);
		}
		
		public void serializeAs_InventoryPresetSaveMessage(BigEndianWriter arg1)
		{
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element presetId.");
			}
			arg1.WriteByte((byte)this.presetId);
			if ( this.symbolId < 0 )
			{
				throw new Exception("Forbidden value (" + this.symbolId + ") on element symbolId.");
			}
			arg1.WriteByte((byte)this.symbolId);
			arg1.WriteBoolean(this.saveEquipment);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InventoryPresetSaveMessage(arg1);
		}
		
		public void deserializeAs_InventoryPresetSaveMessage(BigEndianReader arg1)
		{
			this.presetId = (uint)arg1.ReadByte();
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element of InventoryPresetSaveMessage.presetId.");
			}
			this.symbolId = (uint)arg1.ReadByte();
			if ( this.symbolId < 0 )
			{
				throw new Exception("Forbidden value (" + this.symbolId + ") on element of InventoryPresetSaveMessage.symbolId.");
			}
			this.saveEquipment = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
