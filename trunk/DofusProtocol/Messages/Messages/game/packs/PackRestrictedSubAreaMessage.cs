using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PackRestrictedSubAreaMessage : Message
	{
		public const uint protocolId = 6186;
		internal Boolean _isInitialized = false;
		public uint subAreaId = 0;
		
		public PackRestrictedSubAreaMessage()
		{
		}
		
		public PackRestrictedSubAreaMessage(uint arg1)
			: this()
		{
			initPackRestrictedSubAreaMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6186;
		}
		
		public PackRestrictedSubAreaMessage initPackRestrictedSubAreaMessage(uint arg1 = 0)
		{
			this.subAreaId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.subAreaId = 0;
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
			this.serializeAs_PackRestrictedSubAreaMessage(arg1);
		}
		
		public void serializeAs_PackRestrictedSubAreaMessage(BigEndianWriter arg1)
		{
			if ( this.subAreaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subAreaId + ") on element subAreaId.");
			}
			arg1.WriteInt((int)this.subAreaId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PackRestrictedSubAreaMessage(arg1);
		}
		
		public void deserializeAs_PackRestrictedSubAreaMessage(BigEndianReader arg1)
		{
			this.subAreaId = (uint)arg1.ReadInt();
			if ( this.subAreaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subAreaId + ") on element of PackRestrictedSubAreaMessage.subAreaId.");
			}
		}
		
	}
}
