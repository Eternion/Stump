using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FriendSpouseInformations : Object
	{
		public const uint protocolId = 77;
		public uint spouseId = 0;
		public String spouseName = "";
		public uint spouseLevel = 0;
		public int breed = 0;
		public int sex = 0;
		public EntityLook spouseEntityLook;
		public BasicGuildInformations guildInfo;
		public int alignmentSide = 0;
		
		public FriendSpouseInformations()
		{
			this.spouseEntityLook = new EntityLook();
			this.guildInfo = new BasicGuildInformations();
		}
		
		public FriendSpouseInformations(uint arg1, String arg2, uint arg3, int arg4, int arg5, EntityLook arg6, BasicGuildInformations arg7, int arg8)
			: this()
		{
			initFriendSpouseInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}
		
		public virtual uint getTypeId()
		{
			return 77;
		}
		
		public FriendSpouseInformations initFriendSpouseInformations(uint arg1 = 0, String arg2 = "", uint arg3 = 0, int arg4 = 0, int arg5 = 0, EntityLook arg6 = null, BasicGuildInformations arg7 = null, int arg8 = 0)
		{
			this.spouseId = arg1;
			this.spouseName = arg2;
			this.spouseLevel = arg3;
			this.breed = arg4;
			this.sex = arg5;
			this.spouseEntityLook = arg6;
			this.guildInfo = arg7;
			this.alignmentSide = arg8;
			return this;
		}
		
		public virtual void reset()
		{
			this.spouseId = 0;
			this.spouseName = "";
			this.spouseLevel = 0;
			this.breed = 0;
			this.sex = 0;
			this.spouseEntityLook = new EntityLook();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FriendSpouseInformations(arg1);
		}
		
		public void serializeAs_FriendSpouseInformations(BigEndianWriter arg1)
		{
			if ( this.spouseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spouseId + ") on element spouseId.");
			}
			arg1.WriteInt((int)this.spouseId);
			arg1.WriteUTF((string)this.spouseName);
			if ( this.spouseLevel < 1 || this.spouseLevel > 200 )
			{
				throw new Exception("Forbidden value (" + this.spouseLevel + ") on element spouseLevel.");
			}
			arg1.WriteByte((byte)this.spouseLevel);
			arg1.WriteByte((byte)this.breed);
			arg1.WriteByte((byte)this.sex);
			this.spouseEntityLook.serializeAs_EntityLook(arg1);
			this.guildInfo.serializeAs_BasicGuildInformations(arg1);
			arg1.WriteByte((byte)this.alignmentSide);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FriendSpouseInformations(arg1);
		}
		
		public void deserializeAs_FriendSpouseInformations(BigEndianReader arg1)
		{
			this.spouseId = (uint)arg1.ReadInt();
			if ( this.spouseId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spouseId + ") on element of FriendSpouseInformations.spouseId.");
			}
			this.spouseName = (String)arg1.ReadUTF();
			this.spouseLevel = (uint)arg1.ReadByte();
			if ( this.spouseLevel < 1 || this.spouseLevel > 200 )
			{
				throw new Exception("Forbidden value (" + this.spouseLevel + ") on element of FriendSpouseInformations.spouseLevel.");
			}
			this.breed = (int)arg1.ReadByte();
			this.sex = (int)arg1.ReadByte();
			this.spouseEntityLook = new EntityLook();
			this.spouseEntityLook.deserialize(arg1);
			this.guildInfo = new BasicGuildInformations();
			this.guildInfo.deserialize(arg1);
			this.alignmentSide = (int)arg1.ReadByte();
		}
		
	}
}
