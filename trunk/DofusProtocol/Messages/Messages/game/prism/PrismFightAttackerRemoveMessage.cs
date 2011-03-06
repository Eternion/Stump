using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PrismFightAttackerRemoveMessage : Message
	{
		public const uint protocolId = 5897;
		internal Boolean _isInitialized = false;
		public double fightId = 0;
		public uint fighterToRemoveId = 0;
		
		public PrismFightAttackerRemoveMessage()
		{
		}
		
		public PrismFightAttackerRemoveMessage(double arg1, uint arg2)
			: this()
		{
			initPrismFightAttackerRemoveMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5897;
		}
		
		public PrismFightAttackerRemoveMessage initPrismFightAttackerRemoveMessage(double arg1 = 0, uint arg2 = 0)
		{
			this.fightId = arg1;
			this.fighterToRemoveId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.fighterToRemoveId = 0;
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
			this.serializeAs_PrismFightAttackerRemoveMessage(arg1);
		}
		
		public void serializeAs_PrismFightAttackerRemoveMessage(BigEndianWriter arg1)
		{
			arg1.WriteDouble(this.fightId);
			if ( this.fighterToRemoveId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fighterToRemoveId + ") on element fighterToRemoveId.");
			}
			arg1.WriteInt((int)this.fighterToRemoveId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismFightAttackerRemoveMessage(arg1);
		}
		
		public void deserializeAs_PrismFightAttackerRemoveMessage(BigEndianReader arg1)
		{
			this.fightId = (double)arg1.ReadDouble();
			this.fighterToRemoveId = (uint)arg1.ReadInt();
			if ( this.fighterToRemoveId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fighterToRemoveId + ") on element of PrismFightAttackerRemoveMessage.fighterToRemoveId.");
			}
		}
		
	}
}
