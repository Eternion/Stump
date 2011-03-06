using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class DungeonLeftMessage : Message
	{
		public const uint protocolId = 6149;
		internal Boolean _isInitialized = false;
		public uint dungeonId = 0;
		
		public DungeonLeftMessage()
		{
		}
		
		public DungeonLeftMessage(uint arg1)
			: this()
		{
			initDungeonLeftMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6149;
		}
		
		public DungeonLeftMessage initDungeonLeftMessage(uint arg1 = 0)
		{
			this.dungeonId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.dungeonId = 0;
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
			this.serializeAs_DungeonLeftMessage(arg1);
		}
		
		public void serializeAs_DungeonLeftMessage(BigEndianWriter arg1)
		{
			if ( this.dungeonId < 0 )
			{
				throw new Exception("Forbidden value (" + this.dungeonId + ") on element dungeonId.");
			}
			arg1.WriteInt((int)this.dungeonId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_DungeonLeftMessage(arg1);
		}
		
		public void deserializeAs_DungeonLeftMessage(BigEndianReader arg1)
		{
			this.dungeonId = (uint)arg1.ReadInt();
			if ( this.dungeonId < 0 )
			{
				throw new Exception("Forbidden value (" + this.dungeonId + ") on element of DungeonLeftMessage.dungeonId.");
			}
		}
		
	}
}
