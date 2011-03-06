using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PaddockToSellListRequestMessage : Message
	{
		public const uint protocolId = 6141;
		internal Boolean _isInitialized = false;
		public uint pageIndex = 0;
		
		public PaddockToSellListRequestMessage()
		{
		}
		
		public PaddockToSellListRequestMessage(uint arg1)
			: this()
		{
			initPaddockToSellListRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6141;
		}
		
		public PaddockToSellListRequestMessage initPaddockToSellListRequestMessage(uint arg1 = 0)
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
			this.serializeAs_PaddockToSellListRequestMessage(arg1);
		}
		
		public void serializeAs_PaddockToSellListRequestMessage(BigEndianWriter arg1)
		{
			if ( this.pageIndex < 0 )
			{
				throw new Exception("Forbidden value (" + this.pageIndex + ") on element pageIndex.");
			}
			arg1.WriteShort((short)this.pageIndex);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockToSellListRequestMessage(arg1);
		}
		
		public void deserializeAs_PaddockToSellListRequestMessage(BigEndianReader arg1)
		{
			this.pageIndex = (uint)arg1.ReadShort();
			if ( this.pageIndex < 0 )
			{
				throw new Exception("Forbidden value (" + this.pageIndex + ") on element of PaddockToSellListRequestMessage.pageIndex.");
			}
		}
		
	}
}
