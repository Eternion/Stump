using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ShowCellMessage : Message
	{
		public const uint protocolId = 5612;
		internal Boolean _isInitialized = false;
		public int sourceId = 0;
		public uint cellId = 0;
		
		public ShowCellMessage()
		{
		}
		
		public ShowCellMessage(int arg1, uint arg2)
			: this()
		{
			initShowCellMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5612;
		}
		
		public ShowCellMessage initShowCellMessage(int arg1 = 0, uint arg2 = 0)
		{
			this.sourceId = arg1;
			this.cellId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.sourceId = 0;
			this.cellId = 0;
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
			this.serializeAs_ShowCellMessage(arg1);
		}
		
		public void serializeAs_ShowCellMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.sourceId);
			if ( this.cellId < 0 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element cellId.");
			}
			arg1.WriteShort((short)this.cellId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ShowCellMessage(arg1);
		}
		
		public void deserializeAs_ShowCellMessage(BigEndianReader arg1)
		{
			this.sourceId = (int)arg1.ReadInt();
			this.cellId = (uint)arg1.ReadShort();
			if ( this.cellId < 0 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element of ShowCellMessage.cellId.");
			}
		}
		
	}
}
