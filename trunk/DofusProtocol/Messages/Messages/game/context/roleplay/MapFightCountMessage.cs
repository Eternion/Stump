using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MapFightCountMessage : Message
	{
		public const uint protocolId = 210;
		internal Boolean _isInitialized = false;
		public uint fightCount = 0;
		
		public MapFightCountMessage()
		{
		}
		
		public MapFightCountMessage(uint arg1)
			: this()
		{
			initMapFightCountMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 210;
		}
		
		public MapFightCountMessage initMapFightCountMessage(uint arg1 = 0)
		{
			this.fightCount = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightCount = 0;
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
			this.serializeAs_MapFightCountMessage(arg1);
		}
		
		public void serializeAs_MapFightCountMessage(BigEndianWriter arg1)
		{
			if ( this.fightCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightCount + ") on element fightCount.");
			}
			arg1.WriteShort((short)this.fightCount);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MapFightCountMessage(arg1);
		}
		
		public void deserializeAs_MapFightCountMessage(BigEndianReader arg1)
		{
			this.fightCount = (uint)arg1.ReadShort();
			if ( this.fightCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightCount + ") on element of MapFightCountMessage.fightCount.");
			}
		}
		
	}
}
