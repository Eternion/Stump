using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InventoryPresetUpdateMessage : Message
	{
		public const uint protocolId = 6171;
		internal Boolean _isInitialized = false;
		public Preset preset;
		
		public InventoryPresetUpdateMessage()
		{
			this.preset = new Preset();
		}
		
		public InventoryPresetUpdateMessage(Preset arg1)
			: this()
		{
			initInventoryPresetUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6171;
		}
		
		public InventoryPresetUpdateMessage initInventoryPresetUpdateMessage(Preset arg1 = null)
		{
			this.preset = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.preset = new Preset();
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
			this.serializeAs_InventoryPresetUpdateMessage(arg1);
		}
		
		public void serializeAs_InventoryPresetUpdateMessage(BigEndianWriter arg1)
		{
			this.preset.serializeAs_Preset(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InventoryPresetUpdateMessage(arg1);
		}
		
		public void deserializeAs_InventoryPresetUpdateMessage(BigEndianReader arg1)
		{
			this.preset = new Preset();
			this.preset.deserialize(arg1);
		}
		
	}
}
