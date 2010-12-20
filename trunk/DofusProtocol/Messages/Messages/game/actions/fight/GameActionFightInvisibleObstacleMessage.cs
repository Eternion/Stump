using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameActionFightInvisibleObstacleMessage : AbstractGameActionMessage
	{
		public const uint protocolId = 5820;
		internal Boolean _isInitialized = false;
		public uint sourceSpellId = 0;
		
		public GameActionFightInvisibleObstacleMessage()
		{
		}
		
		public GameActionFightInvisibleObstacleMessage(uint arg1, int arg2, uint arg3)
			: this()
		{
			initGameActionFightInvisibleObstacleMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5820;
		}
		
		public GameActionFightInvisibleObstacleMessage initGameActionFightInvisibleObstacleMessage(uint arg1 = 0, int arg2 = 0, uint arg3 = 0)
		{
			base.initAbstractGameActionMessage(arg1, arg2);
			this.sourceSpellId = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.sourceSpellId = 0;
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
			this.serializeAs_GameActionFightInvisibleObstacleMessage(arg1);
		}
		
		public void serializeAs_GameActionFightInvisibleObstacleMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AbstractGameActionMessage(arg1);
			if ( this.sourceSpellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.sourceSpellId + ") on element sourceSpellId.");
			}
			arg1.WriteInt((int)this.sourceSpellId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionFightInvisibleObstacleMessage(arg1);
		}
		
		public void deserializeAs_GameActionFightInvisibleObstacleMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.sourceSpellId = (uint)arg1.ReadInt();
			if ( this.sourceSpellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.sourceSpellId + ") on element of GameActionFightInvisibleObstacleMessage.sourceSpellId.");
			}
		}
		
	}
}
