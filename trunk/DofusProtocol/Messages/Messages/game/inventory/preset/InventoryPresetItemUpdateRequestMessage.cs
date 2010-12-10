using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InventoryPresetItemUpdateRequestMessage : Message
	{
		public const uint protocolId = 6210;
		internal Boolean _isInitialized = false;
		public uint presetId = 0;
		public uint position = 63;
		public uint objUid = 0;
		
		public InventoryPresetItemUpdateRequestMessage()
		{
		}
		
		public InventoryPresetItemUpdateRequestMessage(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initInventoryPresetItemUpdateRequestMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6210;
		}
		
		public InventoryPresetItemUpdateRequestMessage initInventoryPresetItemUpdateRequestMessage(uint arg1 = 0, uint arg2 = 63, uint arg3 = 0)
		{
			this.presetId = arg1;
			this.position = arg2;
			this.objUid = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.presetId = 0;
			this.position = 63;
			this.objUid = 0;
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
			this.serializeAs_InventoryPresetItemUpdateRequestMessage(arg1);
		}
		
		public void serializeAs_InventoryPresetItemUpdateRequestMessage(BigEndianWriter arg1)
		{
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element presetId.");
			}
			arg1.WriteByte((byte)this.presetId);
			arg1.WriteByte((byte)this.position);
			if ( this.objUid < 0 )
			{
				throw new Exception("Forbidden value (" + this.objUid + ") on element objUid.");
			}
			arg1.WriteInt((int)this.objUid);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InventoryPresetItemUpdateRequestMessage(arg1);
		}
		
		public void deserializeAs_InventoryPresetItemUpdateRequestMessage(BigEndianReader arg1)
		{
			this.presetId = (uint)arg1.ReadByte();
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element of InventoryPresetItemUpdateRequestMessage.presetId.");
			}
			this.position = (uint)arg1.ReadByte();
			if ( this.position < 0 || this.position > 255 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element of InventoryPresetItemUpdateRequestMessage.position.");
			}
			this.objUid = (uint)arg1.ReadInt();
			if ( this.objUid < 0 )
			{
				throw new Exception("Forbidden value (" + this.objUid + ") on element of InventoryPresetItemUpdateRequestMessage.objUid.");
			}
		}
		
	}
}
