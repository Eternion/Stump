using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FriendInformations : Object
	{
		public const uint protocolId = 78;
		public String name = "";
		public uint playerState = 99;
		public uint lastConnection = 0;
		
		public FriendInformations()
		{
		}
		
		public FriendInformations(String arg1, uint arg2, uint arg3)
			: this()
		{
			initFriendInformations(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 78;
		}
		
		public FriendInformations initFriendInformations(String arg1 = "", uint arg2 = 99, uint arg3 = 0)
		{
			this.name = arg1;
			this.playerState = arg2;
			this.lastConnection = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.name = "";
			this.playerState = 99;
			this.lastConnection = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FriendInformations(arg1);
		}
		
		public void serializeAs_FriendInformations(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.name);
			arg1.WriteByte((byte)this.playerState);
			if ( this.lastConnection < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastConnection + ") on element lastConnection.");
			}
			arg1.WriteInt((int)this.lastConnection);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FriendInformations(arg1);
		}
		
		public void deserializeAs_FriendInformations(BigEndianReader arg1)
		{
			this.name = (String)arg1.ReadUTF();
			this.playerState = (uint)arg1.ReadByte();
			if ( this.playerState < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerState + ") on element of FriendInformations.playerState.");
			}
			this.lastConnection = (uint)arg1.ReadInt();
			if ( this.lastConnection < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastConnection + ") on element of FriendInformations.lastConnection.");
			}
		}
		
	}
}
