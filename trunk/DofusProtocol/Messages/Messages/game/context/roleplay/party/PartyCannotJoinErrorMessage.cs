using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartyCannotJoinErrorMessage : Message
	{
		public const uint protocolId = 5583;
		internal Boolean _isInitialized = false;
		public uint reason = 0;
		
		public PartyCannotJoinErrorMessage()
		{
		}
		
		public PartyCannotJoinErrorMessage(uint arg1)
			: this()
		{
			initPartyCannotJoinErrorMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5583;
		}
		
		public PartyCannotJoinErrorMessage initPartyCannotJoinErrorMessage(uint arg1 = 0)
		{
			this.reason = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.reason = 0;
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
			this.serializeAs_PartyCannotJoinErrorMessage(arg1);
		}
		
		public void serializeAs_PartyCannotJoinErrorMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.reason);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyCannotJoinErrorMessage(arg1);
		}
		
		public void deserializeAs_PartyCannotJoinErrorMessage(BigEndianReader arg1)
		{
			this.reason = (uint)arg1.ReadByte();
			if ( this.reason < 0 )
			{
				throw new Exception("Forbidden value (" + this.reason + ") on element of PartyCannotJoinErrorMessage.reason.");
			}
		}
		
	}
}
