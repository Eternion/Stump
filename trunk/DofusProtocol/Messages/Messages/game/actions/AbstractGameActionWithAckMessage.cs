using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class AbstractGameActionWithAckMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 1001;
		internal Boolean _isInitialized = false;
		public int waitAckId = 0;
		
		public AbstractGameActionWithAckMessage()
		{
		}
		
		public AbstractGameActionWithAckMessage(uint arg1, int arg2, int arg3)
			: this()
		{
			initAbstractGameActionWithAckMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 1001;
		}
		
		public AbstractGameActionWithAckMessage initAbstractGameActionWithAckMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.waitAckId = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.waitAckId = 0;
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
			this.serializeAs_AbstractGameActionWithAckMessage(arg1);
		}
		
		public void serializeAs_AbstractGameActionWithAckMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			arg1.WriteShort((short)this.waitAckId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AbstractGameActionWithAckMessage(arg1);
		}
		
		public void deserializeAs_AbstractGameActionWithAckMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.waitAckId = (int)arg1.ReadShort();
		}
		
	}
}
