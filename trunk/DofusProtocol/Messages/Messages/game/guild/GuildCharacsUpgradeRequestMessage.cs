using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildCharacsUpgradeRequestMessage : Message
	{
		public const uint protocolId = 5706;
		internal Boolean _isInitialized = false;
		public uint charaTypeTarget = 0;
		
		public GuildCharacsUpgradeRequestMessage()
		{
		}
		
		public GuildCharacsUpgradeRequestMessage(uint arg1)
			: this()
		{
			initGuildCharacsUpgradeRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5706;
		}
		
		public GuildCharacsUpgradeRequestMessage initGuildCharacsUpgradeRequestMessage(uint arg1 = 0)
		{
			this.charaTypeTarget = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.charaTypeTarget = 0;
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
			this.serializeAs_GuildCharacsUpgradeRequestMessage(arg1);
		}
		
		public void serializeAs_GuildCharacsUpgradeRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.charaTypeTarget);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildCharacsUpgradeRequestMessage(arg1);
		}
		
		public void deserializeAs_GuildCharacsUpgradeRequestMessage(BigEndianReader arg1)
		{
			this.charaTypeTarget = (uint)arg1.ReadByte();
			if ( this.charaTypeTarget < 0 )
			{
				throw new Exception("Forbidden value (" + this.charaTypeTarget + ") on element of GuildCharacsUpgradeRequestMessage.charaTypeTarget.");
			}
		}
		
	}
}
