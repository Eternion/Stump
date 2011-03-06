using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class HouseToSellListRequestMessage : Message
	{
		public const uint protocolId = 6139;
		internal Boolean _isInitialized = false;
		public uint pageIndex = 0;
		
		public HouseToSellListRequestMessage()
		{
		}
		
		public HouseToSellListRequestMessage(uint arg1)
			: this()
		{
			initHouseToSellListRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6139;
		}
		
		public HouseToSellListRequestMessage initHouseToSellListRequestMessage(uint arg1 = 0)
		{
			this.pageIndex = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.pageIndex = 0;
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
			this.serializeAs_HouseToSellListRequestMessage(arg1);
		}
		
		public void serializeAs_HouseToSellListRequestMessage(BigEndianWriter arg1)
		{
			if ( this.pageIndex < 0 )
			{
				throw new Exception("Forbidden value (" + this.pageIndex + ") on element pageIndex.");
			}
			arg1.WriteShort((short)this.pageIndex);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseToSellListRequestMessage(arg1);
		}
		
		public void deserializeAs_HouseToSellListRequestMessage(BigEndianReader arg1)
		{
			this.pageIndex = (uint)arg1.ReadShort();
			if ( this.pageIndex < 0 )
			{
				throw new Exception("Forbidden value (" + this.pageIndex + ") on element of HouseToSellListRequestMessage.pageIndex.");
			}
		}
		
	}
}
