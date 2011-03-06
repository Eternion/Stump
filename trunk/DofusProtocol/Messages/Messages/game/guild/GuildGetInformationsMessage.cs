using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildGetInformationsMessage : Message
	{
		public const uint protocolId = 5550;
		internal Boolean _isInitialized = false;
		public uint infoType = 0;
		
		public GuildGetInformationsMessage()
		{
		}
		
		public GuildGetInformationsMessage(uint arg1)
			: this()
		{
			initGuildGetInformationsMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5550;
		}
		
		public GuildGetInformationsMessage initGuildGetInformationsMessage(uint arg1 = 0)
		{
			this.infoType = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.infoType = 0;
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
			this.serializeAs_GuildGetInformationsMessage(arg1);
		}
		
		public void serializeAs_GuildGetInformationsMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.infoType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildGetInformationsMessage(arg1);
		}
		
		public void deserializeAs_GuildGetInformationsMessage(BigEndianReader arg1)
		{
			this.infoType = (uint)arg1.ReadByte();
			if ( this.infoType < 0 )
			{
				throw new Exception("Forbidden value (" + this.infoType + ") on element of GuildGetInformationsMessage.infoType.");
			}
		}
		
	}
}
