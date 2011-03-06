using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameRolePlayPlayerLifeStatusMessage : Message
	{
		public const uint protocolId = 5996;
		internal Boolean _isInitialized = false;
		public uint state = 0;
		
		public GameRolePlayPlayerLifeStatusMessage()
		{
		}
		
		public GameRolePlayPlayerLifeStatusMessage(uint arg1)
			: this()
		{
			initGameRolePlayPlayerLifeStatusMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5996;
		}
		
		public GameRolePlayPlayerLifeStatusMessage initGameRolePlayPlayerLifeStatusMessage(uint arg1 = 0)
		{
			this.state = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.state = 0;
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
			this.serializeAs_GameRolePlayPlayerLifeStatusMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayPlayerLifeStatusMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.state);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayPlayerLifeStatusMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayPlayerLifeStatusMessage(BigEndianReader arg1)
		{
			this.state = (uint)arg1.ReadByte();
			if ( this.state < 0 )
			{
				throw new Exception("Forbidden value (" + this.state + ") on element of GameRolePlayPlayerLifeStatusMessage.state.");
			}
		}
		
	}
}
