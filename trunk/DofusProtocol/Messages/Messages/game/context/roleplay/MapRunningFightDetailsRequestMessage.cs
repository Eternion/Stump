using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MapRunningFightDetailsRequestMessage : Message
	{
		public const uint protocolId = 5750;
		internal Boolean _isInitialized = false;
		public uint fightId = 0;
		
		public MapRunningFightDetailsRequestMessage()
		{
		}
		
		public MapRunningFightDetailsRequestMessage(uint arg1)
			: this()
		{
			initMapRunningFightDetailsRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5750;
		}
		
		public MapRunningFightDetailsRequestMessage initMapRunningFightDetailsRequestMessage(uint arg1 = 0)
		{
			this.fightId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
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
			this.serializeAs_MapRunningFightDetailsRequestMessage(arg1);
		}
		
		public void serializeAs_MapRunningFightDetailsRequestMessage(BigEndianWriter arg1)
		{
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element fightId.");
			}
			arg1.WriteInt((int)this.fightId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MapRunningFightDetailsRequestMessage(arg1);
		}
		
		public void deserializeAs_MapRunningFightDetailsRequestMessage(BigEndianReader arg1)
		{
			this.fightId = (uint)arg1.ReadInt();
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element of MapRunningFightDetailsRequestMessage.fightId.");
			}
		}
		
	}
}
