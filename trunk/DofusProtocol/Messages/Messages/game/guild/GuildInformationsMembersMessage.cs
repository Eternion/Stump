using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildInformationsMembersMessage : Message
	{
		public const uint protocolId = 5558;
		internal Boolean _isInitialized = false;
		public List<GuildMember> members;
		
		public GuildInformationsMembersMessage()
		{
			this.members = new List<GuildMember>();
		}
		
		public GuildInformationsMembersMessage(List<GuildMember> arg1)
			: this()
		{
			initGuildInformationsMembersMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5558;
		}
		
		public GuildInformationsMembersMessage initGuildInformationsMembersMessage(List<GuildMember> arg1)
		{
			this.members = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.members = new List<GuildMember>();
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
			this.serializeAs_GuildInformationsMembersMessage(arg1);
		}
		
		public void serializeAs_GuildInformationsMembersMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.members.Count);
			var loc1 = 0;
			while ( loc1 < this.members.Count )
			{
				this.members[loc1].serializeAs_GuildMember(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildInformationsMembersMessage(arg1);
		}
		
		public void deserializeAs_GuildInformationsMembersMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new GuildMember()) as GuildMember).deserialize(arg1);
				this.members.Add((GuildMember)loc3);
				++loc2;
			}
		}
		
	}
}
