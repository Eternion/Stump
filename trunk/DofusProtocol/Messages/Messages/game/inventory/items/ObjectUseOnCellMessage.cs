using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectUseOnCellMessage : ObjectUseMessage
	{
		public const uint protocolId = 3013;
		internal Boolean _isInitialized = false;
		public uint cells = 0;
		
		public ObjectUseOnCellMessage()
		{
		}
		
		public ObjectUseOnCellMessage(uint arg1, uint arg2)
			: this()
		{
			initObjectUseOnCellMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 3013;
		}
		
		public ObjectUseOnCellMessage initObjectUseOnCellMessage(uint arg1 = 0, uint arg2 = 0)
		{
			base.initObjectUseMessage(arg1);
			this.cells = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.cells = 0;
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
			this.serializeAs_ObjectUseOnCellMessage(arg1);
		}
		
		public void serializeAs_ObjectUseOnCellMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectUseMessage(arg1);
			if ( this.cells < 0 || this.cells > 559 )
			{
				throw new Exception("Forbidden value (" + this.cells + ") on element cells.");
			}
			arg1.WriteShort((short)this.cells);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectUseOnCellMessage(arg1);
		}
		
		public void deserializeAs_ObjectUseOnCellMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.cells = (uint)arg1.ReadShort();
			if ( this.cells < 0 || this.cells > 559 )
			{
				throw new Exception("Forbidden value (" + this.cells + ") on element of ObjectUseOnCellMessage.cells.");
			}
		}
		
	}
}
