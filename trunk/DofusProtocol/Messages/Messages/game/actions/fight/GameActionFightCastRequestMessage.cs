using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightCastRequestMessage : Message
	{
		public const uint protocolId = 1005;
		internal Boolean _isInitialized = false;
		public uint spellId = 0;
		public int cellId = 0;
		
		public GameActionFightCastRequestMessage()
		{
		}
		
		public GameActionFightCastRequestMessage(uint arg1, int arg2)
			: this()
		{
			initGameActionFightCastRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 1005;
		}
		
		public GameActionFightCastRequestMessage initGameActionFightCastRequestMessage(uint arg1 = 0, int arg2 = 0)
		{
			this.spellId = arg1;
			this.cellId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.spellId = 0;
			this.cellId = 0;
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
			this.serializeAs_GameActionFightCastRequestMessage(arg1);
		}
		
		public void serializeAs_GameActionFightCastRequestMessage(BigEndianWriter arg1)
		{
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element spellId.");
			}
			arg1.WriteShort((short)this.spellId);
			if ( this.cellId < -1 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element cellId.");
			}
			arg1.WriteShort((short)this.cellId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightCastRequestMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightCastRequestMessage(BigEndianReader arg1)
		{
			this.spellId = (uint)arg1.ReadShort();
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element of GameActionFightCastRequestMessage.spellId.");
			}
			this.cellId = (int)arg1.ReadShort();
			if ( this.cellId < -1 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element of GameActionFightCastRequestMessage.cellId.");
			}
		}
		
	}
}
