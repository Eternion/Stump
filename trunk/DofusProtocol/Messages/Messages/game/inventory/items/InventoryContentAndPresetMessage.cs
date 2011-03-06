using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InventoryContentAndPresetMessage : InventoryContentMessage
	{
		public const uint protocolId = 6162;
		internal Boolean _isInitialized = false;
		public List<Preset> presets;
		
		public InventoryContentAndPresetMessage()
		{
			this.presets = new List<Preset>();
		}
		
		public InventoryContentAndPresetMessage(List<ObjectItem> arg1, uint arg2, List<Preset> arg3)
			: this()
		{
			initInventoryContentAndPresetMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6162;
		}
		
		public InventoryContentAndPresetMessage initInventoryContentAndPresetMessage(List<ObjectItem> arg1, uint arg2 = 0, List<Preset> arg3 = null)
		{
			base.initInventoryContentMessage(arg1, arg2);
			this.presets = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.presets = new List<Preset>();
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_InventoryContentAndPresetMessage(arg1);
		}
		
		public void serializeAs_InventoryContentAndPresetMessage(BigEndianWriter arg1)
		{
			base.serializeAs_InventoryContentMessage(arg1);
			arg1.WriteShort((short)this.presets.Count);
			var loc1 = 0;
			while ( loc1 < this.presets.Count )
			{
				this.presets[loc1].serializeAs_Preset(arg1);
				++loc1;
			}
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InventoryContentAndPresetMessage(arg1);
		}
		
		public void deserializeAs_InventoryContentAndPresetMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			base.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new Preset()) as Preset).deserialize(arg1);
				this.presets.Add((Preset)loc3);
				++loc2;
			}
		}
		
	}
}
