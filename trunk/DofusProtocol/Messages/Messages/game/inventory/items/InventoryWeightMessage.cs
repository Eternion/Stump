using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InventoryWeightMessage : Message
	{
		public const uint protocolId = 3009;
		internal Boolean _isInitialized = false;
		public uint weight = 0;
		public uint weightMax = 0;
		
		public InventoryWeightMessage()
		{
		}
		
		public InventoryWeightMessage(uint arg1, uint arg2)
			: this()
		{
			initInventoryWeightMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 3009;
		}
		
		public InventoryWeightMessage initInventoryWeightMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.weight = arg1;
			this.weightMax = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.weight = 0;
			this.weightMax = 0;
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
			this.serializeAs_InventoryWeightMessage(arg1);
		}
		
		public void serializeAs_InventoryWeightMessage(BigEndianWriter arg1)
		{
			if ( this.weight < 0 )
			{
				throw new Exception("Forbidden value (" + this.weight + ") on element weight.");
			}
			arg1.WriteInt((int)this.weight);
			if ( this.weightMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.weightMax + ") on element weightMax.");
			}
			arg1.WriteInt((int)this.weightMax);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InventoryWeightMessage(arg1);
		}
		
		public void deserializeAs_InventoryWeightMessage(BigEndianReader arg1)
		{
			this.weight = (uint)arg1.ReadInt();
			if ( this.weight < 0 )
			{
				throw new Exception("Forbidden value (" + this.weight + ") on element of InventoryWeightMessage.weight.");
			}
			this.weightMax = (uint)arg1.ReadInt();
			if ( this.weightMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.weightMax + ") on element of InventoryWeightMessage.weightMax.");
			}
		}
		
	}
}
