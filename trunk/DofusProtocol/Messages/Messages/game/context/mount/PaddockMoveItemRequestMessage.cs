using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PaddockMoveItemRequestMessage : Message
	{
		public const uint protocolId = 6052;
		internal Boolean _isInitialized = false;
		public uint oldCellId = 0;
		public uint newCellId = 0;
		
		public PaddockMoveItemRequestMessage()
		{
		}
		
		public PaddockMoveItemRequestMessage(uint arg1, uint arg2)
			: this()
		{
			initPaddockMoveItemRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6052;
		}
		
		public PaddockMoveItemRequestMessage initPaddockMoveItemRequestMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.oldCellId = arg1;
			this.newCellId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.oldCellId = 0;
			this.newCellId = 0;
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
			this.serializeAs_PaddockMoveItemRequestMessage(arg1);
		}
		
		public void serializeAs_PaddockMoveItemRequestMessage(BigEndianWriter arg1)
		{
			if ( this.oldCellId < 0 || this.oldCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.oldCellId + ") on element oldCellId.");
			}
			arg1.WriteShort((short)this.oldCellId);
			if ( this.newCellId < 0 || this.newCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.newCellId + ") on element newCellId.");
			}
			arg1.WriteShort((short)this.newCellId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockMoveItemRequestMessage(arg1);
		}
		
		public void deserializeAs_PaddockMoveItemRequestMessage(BigEndianReader arg1)
		{
			this.oldCellId = (uint)arg1.ReadShort();
			if ( this.oldCellId < 0 || this.oldCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.oldCellId + ") on element of PaddockMoveItemRequestMessage.oldCellId.");
			}
			this.newCellId = (uint)arg1.ReadShort();
			if ( this.newCellId < 0 || this.newCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.newCellId + ") on element of PaddockMoveItemRequestMessage.newCellId.");
			}
		}
		
	}
}
