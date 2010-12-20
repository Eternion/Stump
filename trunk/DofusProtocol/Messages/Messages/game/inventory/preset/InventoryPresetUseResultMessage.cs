using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InventoryPresetUseResultMessage : Message
	{
		public const uint protocolId = 6163;
		internal Boolean _isInitialized = false;
		public uint presetId = 0;
		public uint code = 3;
		public List<uint> unlinkedPosition;
		
		public InventoryPresetUseResultMessage()
		{
			this.unlinkedPosition = new List<uint>();
		}
		
		public InventoryPresetUseResultMessage(uint arg1, uint arg2, List<uint> arg3)
			: this()
		{
			initInventoryPresetUseResultMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6163;
		}
		
		public InventoryPresetUseResultMessage initInventoryPresetUseResultMessage(uint arg1 = 0, uint arg2 = 3, List<uint> arg3 = null)
		{
			this.presetId = arg1;
			this.code = arg2;
			this.unlinkedPosition = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.presetId = 0;
			this.code = 3;
			this.unlinkedPosition = new List<uint>();
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
			this.serializeAs_InventoryPresetUseResultMessage(arg1);
		}
		
		public void serializeAs_InventoryPresetUseResultMessage(BigEndianWriter arg1)
		{
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element presetId.");
			}
			arg1.WriteByte((byte)this.presetId);
			arg1.WriteByte((byte)this.code);
			arg1.WriteShort((short)this.unlinkedPosition.Count);
			var loc1 = 0;
			while ( loc1 < this.unlinkedPosition.Count )
			{
				arg1.WriteByte((byte)this.unlinkedPosition[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InventoryPresetUseResultMessage(arg1);
		}
		
		public void deserializeAs_InventoryPresetUseResultMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			this.presetId = (uint)arg1.ReadByte();
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element of InventoryPresetUseResultMessage.presetId.");
			}
			this.code = (uint)arg1.ReadByte();
			if ( this.code < 0 )
			{
				throw new Exception("Forbidden value (" + this.code + ") on element of InventoryPresetUseResultMessage.code.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadByte()) < 0 || loc3 > 255 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of unlinkedPosition.");
				}
				this.unlinkedPosition.Add((uint)loc3);
				++loc2;
			}
		}
		
	}
}
