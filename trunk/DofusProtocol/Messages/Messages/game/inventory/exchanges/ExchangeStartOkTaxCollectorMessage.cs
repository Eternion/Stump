using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeStartOkTaxCollectorMessage : Message
	{
		public const uint protocolId = 5780;
		internal Boolean _isInitialized = false;
		public int collectorId = 0;
		public List<ObjectItem> objectsInfos;
		public uint goldInfo = 0;
		
		public ExchangeStartOkTaxCollectorMessage()
		{
			this.@objectsInfos = new List<ObjectItem>();
		}
		
		public ExchangeStartOkTaxCollectorMessage(int arg1, List<ObjectItem> arg2, uint arg3)
			: this()
		{
			initExchangeStartOkTaxCollectorMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5780;
		}
		
		public ExchangeStartOkTaxCollectorMessage initExchangeStartOkTaxCollectorMessage(int arg1 = 0, List<ObjectItem> arg2 = null, uint arg3 = 0)
		{
			this.collectorId = arg1;
			this.@objectsInfos = arg2;
			this.goldInfo = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.collectorId = 0;
			this.@objectsInfos = new List<ObjectItem>();
			this.goldInfo = 0;
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
			this.serializeAs_ExchangeStartOkTaxCollectorMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartOkTaxCollectorMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.collectorId);
			arg1.WriteShort((short)this.@objectsInfos.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectsInfos.Count )
			{
				this.@objectsInfos[loc1].serializeAs_ObjectItem(arg1);
				++loc1;
			}
			if ( this.goldInfo < 0 )
			{
				throw new Exception("Forbidden value (" + this.goldInfo + ") on element goldInfo.");
			}
			arg1.WriteInt((int)this.goldInfo);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartOkTaxCollectorMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartOkTaxCollectorMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.collectorId = (int)arg1.ReadInt();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItem()) as ObjectItem).deserialize(arg1);
				this.@objectsInfos.Add((ObjectItem)loc3);
				++loc2;
			}
			this.goldInfo = (uint)arg1.ReadInt();
			if ( this.goldInfo < 0 )
			{
				throw new Exception("Forbidden value (" + this.goldInfo + ") on element of ExchangeStartOkTaxCollectorMessage.goldInfo.");
			}
		}
		
	}
}
