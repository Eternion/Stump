using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeStartedMountStockMessage : Message
	{
		public const uint protocolId = 5984;
		internal Boolean _isInitialized = false;
		public List<ObjectItem> objectsInfos;
		
		public ExchangeStartedMountStockMessage()
		{
			this.@objectsInfos = new List<ObjectItem>();
		}
		
		public ExchangeStartedMountStockMessage(List<ObjectItem> arg1)
			: this()
		{
			initExchangeStartedMountStockMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5984;
		}
		
		public ExchangeStartedMountStockMessage initExchangeStartedMountStockMessage(List<ObjectItem> arg1)
		{
			this.@objectsInfos = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.@objectsInfos = new List<ObjectItem>();
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
			this.serializeAs_ExchangeStartedMountStockMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartedMountStockMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.@objectsInfos.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectsInfos.Count )
			{
				this.@objectsInfos[loc1].serializeAs_ObjectItem(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartedMountStockMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartedMountStockMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItem()) as ObjectItem).deserialize(arg1);
				this.@objectsInfos.Add((ObjectItem)loc3);
				++loc2;
			}
		}
		
	}
}
