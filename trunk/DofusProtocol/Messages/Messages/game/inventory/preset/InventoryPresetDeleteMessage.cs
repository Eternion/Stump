using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InventoryPresetDeleteMessage : Message
	{
		public const uint protocolId = 6169;
		internal Boolean _isInitialized = false;
		public uint presetId = 0;
		
		public InventoryPresetDeleteMessage()
		{
		}
		
		public InventoryPresetDeleteMessage(uint arg1)
			: this()
		{
			initInventoryPresetDeleteMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6169;
		}
		
		public InventoryPresetDeleteMessage initInventoryPresetDeleteMessage(uint arg1 = 0)
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
			this.serializeAs_InventoryPresetDeleteMessage(arg1);
		}
		
		public void serializeAs_InventoryPresetDeleteMessage(BigEndianWriter arg1)
		{
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element presetId.");
			}
			arg1.WriteByte((byte)this.presetId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InventoryPresetDeleteMessage(arg1);
		}
		
		public void deserializeAs_InventoryPresetDeleteMessage(BigEndianReader arg1)
		{
			this.presetId = (uint)arg1.ReadByte();
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element of InventoryPresetDeleteMessage.presetId.");
			}
		}
		
	}
}
