using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PrismFightDefenderLeaveMessage : Message
	{
		public const uint protocolId = 5892;
		internal Boolean _isInitialized = false;
		public double fightId = 0;
		public uint fighterToRemoveId = 0;
		public uint successor = 0;
		
		public PrismFightDefenderLeaveMessage()
		{
		}
		
		public PrismFightDefenderLeaveMessage(double arg1, uint arg2, uint arg3)
			: this()
		{
			initPrismFightDefenderLeaveMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5892;
		}
		
		public PrismFightDefenderLeaveMessage initPrismFightDefenderLeaveMessage(double arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.fightId = arg1;
			this.fighterToRemoveId = arg2;
			this.successor = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.fighterToRemoveId = 0;
			this.successor = 0;
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
			this.serializeAs_PrismFightDefenderLeaveMessage(arg1);
		}
		
		public void serializeAs_PrismFightDefenderLeaveMessage(BigEndianWriter arg1)
		{
			arg1.WriteDouble(this.fightId);
			if ( this.fighterToRemoveId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fighterToRemoveId + ") on element fighterToRemoveId.");
			}
			arg1.WriteInt((int)this.fighterToRemoveId);
			if ( this.successor < 0 )
			{
				throw new Exception("Forbidden value (" + this.successor + ") on element successor.");
			}
			arg1.WriteInt((int)this.successor);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismFightDefenderLeaveMessage(arg1);
		}
		
		public void deserializeAs_PrismFightDefenderLeaveMessage(BigEndianReader arg1)
		{
			this.fightId = (double)arg1.ReadDouble();
			this.fighterToRemoveId = (uint)arg1.ReadInt();
			if ( this.fighterToRemoveId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fighterToRemoveId + ") on element of PrismFightDefenderLeaveMessage.fighterToRemoveId.");
			}
			this.successor = (uint)arg1.ReadInt();
			if ( this.successor < 0 )
			{
				throw new Exception("Forbidden value (" + this.successor + ") on element of PrismFightDefenderLeaveMessage.successor.");
			}
		}
		
	}
}
