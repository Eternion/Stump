using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FriendOnlineInformations : FriendInformations
	{
		public const uint protocolId = 92;
		public String playerName = "";
		public uint level = 0;
		public int alignmentSide = 0;
		public int breed = 0;
		public Boolean sex = false;
		public BasicGuildInformations guildInfo;
		public int moodSmileyId = 0;
		
		public FriendOnlineInformations()
		{
			this.guildInfo = new BasicGuildInformations();
		}
		
		public FriendOnlineInformations(String arg1, uint arg2, uint arg3, String arg4, uint arg5, int arg6, int arg7, Boolean arg8, BasicGuildInformations arg9, int arg10)
			: this()
		{
			initFriendOnlineInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
		}
		
		public override uint getTypeId()
		{
			return 92;
		}
		
		public FriendOnlineInformations initFriendOnlineInformations(String arg1 = "", uint arg2 = 99, uint arg3 = 0, String arg4 = "", uint arg5 = 0, int arg6 = 0, int arg7 = 0, Boolean arg8 = false, BasicGuildInformations arg9 = null, int arg10 = 0)
		{
			base.initFriendInformations(arg1, arg2, arg3);
			this.playerName = arg4;
			this.level = arg5;
			this.alignmentSide = arg6;
			this.breed = arg7;
			this.sex = arg8;
			this.guildInfo = arg9;
			this.moodSmileyId = arg10;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.playerName = "";
			this.level = 0;
			this.alignmentSide = 0;
			this.breed = 0;
			this.sex = false;
			this.guildInfo = new BasicGuildInformations();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FriendOnlineInformations(arg1);
		}
		
		public void serializeAs_FriendOnlineInformations(BigEndianWriter arg1)
		{
			base.serializeAs_FriendInformations(arg1);
			arg1.WriteUTF((string)this.playerName);
			if ( this.level < 0 || this.level > 200 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element level.");
			}
			arg1.WriteShort((short)this.level);
			arg1.WriteByte((byte)this.alignmentSide);
			arg1.WriteByte((byte)this.breed);
			arg1.WriteBoolean(this.sex);
			this.guildInfo.serializeAs_BasicGuildInformations(arg1);
			arg1.WriteByte((byte)this.moodSmileyId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FriendOnlineInformations(arg1);
		}
		
		public void deserializeAs_FriendOnlineInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.playerName = (String)arg1.ReadUTF();
			this.level = (uint)arg1.ReadShort();
			if ( this.level < 0 || this.level > 200 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of FriendOnlineInformations.level.");
			}
			this.alignmentSide = (int)arg1.ReadByte();
			this.breed = (int)arg1.ReadByte();
			if ( this.breed < (int)Stump.DofusProtocol.Enums.BreedEnum.Feca || this.breed > (int)Stump.DofusProtocol.Enums.BreedEnum.Zobal )
			{
				throw new Exception("Forbidden value (" + this.breed + ") on element of FriendOnlineInformations.breed.");
			}
			this.sex = (Boolean)arg1.ReadBoolean();
			this.guildInfo = new BasicGuildInformations();
			this.guildInfo.deserialize(arg1);
			this.moodSmileyId = (int)arg1.ReadByte();
		}
		
	}
}
