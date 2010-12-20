using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class HouseBuyRequestMessage : Message
	{
		public const uint protocolId = 5738;
		internal Boolean _isInitialized = false;
		public uint proposedPrice = 0;
		
		public HouseBuyRequestMessage()
		{
		}
		
		public HouseBuyRequestMessage(uint arg1)
			: this()
		{
			initHouseBuyRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5738;
		}
		
		public HouseBuyRequestMessage initHouseBuyRequestMessage(uint arg1 = 0)
		{
			this.proposedPrice = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.proposedPrice = 0;
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
			this.serializeAs_HouseBuyRequestMessage(arg1);
		}
		
		public void serializeAs_HouseBuyRequestMessage(BigEndianWriter arg1)
		{
			if ( this.proposedPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.proposedPrice + ") on element proposedPrice.");
			}
			arg1.WriteInt((int)this.proposedPrice);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseBuyRequestMessage(arg1);
		}
		
		public void deserializeAs_HouseBuyRequestMessage(BigEndianReader arg1)
		{
			this.proposedPrice = (uint)arg1.ReadInt();
			if ( this.proposedPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.proposedPrice + ") on element of HouseBuyRequestMessage.proposedPrice.");
			}
		}
		
	}
}
