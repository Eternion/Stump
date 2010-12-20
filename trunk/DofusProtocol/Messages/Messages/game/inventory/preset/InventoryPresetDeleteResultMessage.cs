using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InventoryPresetDeleteResultMessage : Message
	{
		public const uint protocolId = 6173;
		internal Boolean _isInitialized = false;
		public uint presetId = 0;
		public uint code = 2;
		
		public InventoryPresetDeleteResultMessage()
		{
		}
		
		public InventoryPresetDeleteResultMessage(uint arg1, uint arg2)
			: this()
		{
			initInventoryPresetDeleteResultMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6173;
		}
		
		public InventoryPresetDeleteResultMessage initInventoryPresetDeleteResultMessage(uint arg1 = 0, uint arg2 = 2)
		{
			this.presetId = arg1;
			this.code = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.presetId = 0;
			this.code = 2;
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
			this.serializeAs_InventoryPresetDeleteResultMessage(arg1);
		}
		
		public void serializeAs_InventoryPresetDeleteResultMessage(BigEndianWriter arg1)
		{
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element presetId.");
			}
			arg1.WriteByte((byte)this.presetId);
			arg1.WriteByte((byte)this.code);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InventoryPresetDeleteResultMessage(arg1);
		}
		
		public void deserializeAs_InventoryPresetDeleteResultMessage(BigEndianReader arg1)
		{
			this.presetId = (uint)arg1.ReadByte();
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element of InventoryPresetDeleteResultMessage.presetId.");
			}
			this.code = (uint)arg1.ReadByte();
			if ( this.code < 0 )
			{
				throw new Exception("Forbidden value (" + this.code + ") on element of InventoryPresetDeleteResultMessage.code.");
			}
		}
		
	}
}
