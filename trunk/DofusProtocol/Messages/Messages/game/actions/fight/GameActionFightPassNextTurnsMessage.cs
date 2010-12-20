using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightPassNextTurnsMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 5529;
		internal Boolean _isInitialized = false;
		public int targetId = 0;
		public uint turnCount = 0;
		
		public GameActionFightPassNextTurnsMessage()
		{
		}
		
		public GameActionFightPassNextTurnsMessage(uint arg1, int arg2, int arg3, uint arg4)
			: this()
		{
			initGameActionFightPassNextTurnsMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5529;
		}
		
		public GameActionFightPassNextTurnsMessage initGameActionFightPassNextTurnsMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 0)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.targetId = arg3;
			this.turnCount = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.targetId = 0;
			this.turnCount = 0;
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameActionFightPassNextTurnsMessage(arg1);
		}
		
		public void serializeAs_GameActionFightPassNextTurnsMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			arg1.WriteInt((int)this.targetId);
			if ( this.turnCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.turnCount + ") on element turnCount.");
			}
			arg1.WriteByte((byte)this.turnCount);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightPassNextTurnsMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightPassNextTurnsMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.targetId = (int)arg1.ReadInt();
			this.turnCount = (uint)arg1.ReadByte();
			if ( this.turnCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.turnCount + ") on element of GameActionFightPassNextTurnsMessage.turnCount.");
			}
		}
		
	}
}
