using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InventoryPresetItemUpdateErrorMessage : Message
	{
		public const uint protocolId = 6211;
		internal Boolean _isInitialized = false;
		public uint code = 1;
		
		public InventoryPresetItemUpdateErrorMessage()
		{
		}
		
		public InventoryPresetItemUpdateErrorMessage(uint arg1)
			: this()
		{
			initInventoryPresetItemUpdateErrorMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6211;
		}
		
		public InventoryPresetItemUpdateErrorMessage initInventoryPresetItemUpdateErrorMessage(uint arg1 = 1)
		{
			this.code = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.code = 1;
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
			this.serializeAs_InventoryPresetItemUpdateErrorMessage(arg1);
		}
		
		public void serializeAs_InventoryPresetItemUpdateErrorMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.code);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InventoryPresetItemUpdateErrorMessage(arg1);
		}
		
		public void deserializeAs_InventoryPresetItemUpdateErrorMessage(BigEndianReader arg1)
		{
			this.code = (uint)arg1.ReadByte();
			if ( this.code < 0 )
			{
				throw new Exception("Forbidden value (" + this.code + ") on element of InventoryPresetItemUpdateErrorMessage.code.");
			}
		}
		
	}
}
