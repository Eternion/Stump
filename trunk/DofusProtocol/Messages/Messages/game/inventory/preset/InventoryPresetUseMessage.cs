using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InventoryPresetUseMessage : Message
	{
		public const uint protocolId = 6167;
		internal Boolean _isInitialized = false;
		public uint presetId = 0;
		
		public InventoryPresetUseMessage()
		{
		}
		
		public InventoryPresetUseMessage(uint arg1)
			: this()
		{
			initInventoryPresetUseMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6167;
		}
		
		public InventoryPresetUseMessage initInventoryPresetUseMessage(uint arg1 = 0)
		{
			this.presetId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.presetId = 0;
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
			this.serializeAs_InventoryPresetUseMessage(arg1);
		}
		
		public void serializeAs_InventoryPresetUseMessage(BigEndianWriter arg1)
		{
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element presetId.");
			}
			arg1.WriteByte((byte)this.presetId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InventoryPresetUseMessage(arg1);
		}
		
		public void deserializeAs_InventoryPresetUseMessage(BigEndianReader arg1)
		{
			this.presetId = (uint)arg1.ReadByte();
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element of InventoryPresetUseMessage.presetId.");
			}
		}
		
	}
}
