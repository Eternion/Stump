using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightStartingMessage : Message
	{
		public const uint protocolId = 700;
		internal Boolean _isInitialized = false;
		public uint fightType = 0;
		
		public GameFightStartingMessage()
		{
		}
		
		public GameFightStartingMessage(uint arg1)
			: this()
		{
			initGameFightStartingMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 700;
		}
		
		public GameFightStartingMessage initGameFightStartingMessage(uint arg1 = 0)
		{
			this.fightType = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightType = 0;
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
			this.serializeAs_GameFightStartingMessage(arg1);
		}
		
		public void serializeAs_GameFightStartingMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.fightType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightStartingMessage(arg1);
		}
		
		public void deserializeAs_GameFightStartingMessage(BigEndianReader arg1)
		{
			this.fightType = (uint)arg1.ReadByte();
			if ( this.fightType < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightType + ") on element of GameFightStartingMessage.fightType.");
			}
		}
		
	}
}
