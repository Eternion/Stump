using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CinematicMessage : Message
	{
		public const uint protocolId = 6053;
		internal Boolean _isInitialized = false;
		public uint cinematicId = 0;
		
		public CinematicMessage()
		{
		}
		
		public CinematicMessage(uint arg1)
			: this()
		{
			initCinematicMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6053;
		}
		
		public CinematicMessage initCinematicMessage(uint arg1 = 0)
		{
			this.cinematicId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.cinematicId = 0;
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
			this.serializeAs_CinematicMessage(arg1);
		}
		
		public void serializeAs_CinematicMessage(BigEndianWriter arg1)
		{
			if ( this.cinematicId < 0 )
			{
				throw new Exception("Forbidden value (" + this.cinematicId + ") on element cinematicId.");
			}
			arg1.WriteShort((short)this.cinematicId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CinematicMessage(arg1);
		}
		
		public void deserializeAs_CinematicMessage(BigEndianReader arg1)
		{
			this.cinematicId = (uint)arg1.ReadShort();
			if ( this.cinematicId < 0 )
			{
				throw new Exception("Forbidden value (" + this.cinematicId + ") on element of CinematicMessage.cinematicId.");
			}
		}
		
	}
}
