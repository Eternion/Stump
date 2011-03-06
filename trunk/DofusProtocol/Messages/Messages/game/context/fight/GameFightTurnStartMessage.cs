using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightTurnStartMessage : Message
	{
		public const uint protocolId = 714;
		internal Boolean _isInitialized = false;
		public int id = 0;
		public uint waitTime = 0;
		
		public GameFightTurnStartMessage()
		{
		}
		
		public GameFightTurnStartMessage(int arg1, uint arg2)
			: this()
		{
			initGameFightTurnStartMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 714;
		}
		
		public GameFightTurnStartMessage initGameFightTurnStartMessage(int arg1 = 0, uint arg2 = 0)
		{
			this.id = arg1;
			this.waitTime = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.id = 0;
			this.waitTime = 0;
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
			this.serializeAs_GameFightTurnStartMessage(arg1);
		}
		
		public void serializeAs_GameFightTurnStartMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.id);
			if ( this.waitTime < 0 )
			{
				throw new Exception("Forbidden value (" + this.waitTime + ") on element waitTime.");
			}
			arg1.WriteInt((int)this.waitTime);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightTurnStartMessage(arg1);
		}
		
		public void deserializeAs_GameFightTurnStartMessage(BigEndianReader arg1)
		{
			this.id = (int)arg1.ReadInt();
			this.waitTime = (uint)arg1.ReadInt();
			if ( this.waitTime < 0 )
			{
				throw new Exception("Forbidden value (" + this.waitTime + ") on element of GameFightTurnStartMessage.waitTime.");
			}
		}
		
	}
}
