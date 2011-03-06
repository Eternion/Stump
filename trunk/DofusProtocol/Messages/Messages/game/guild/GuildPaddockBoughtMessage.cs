using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildPaddockBoughtMessage : Message
	{
		public const uint protocolId = 5952;
		internal Boolean _isInitialized = false;
		public int worldX = 0;
		public int worldY = 0;
		public uint nbMountMax = 0;
		public uint nbItemMax = 0;
		
		public GuildPaddockBoughtMessage()
		{
		}
		
		public GuildPaddockBoughtMessage(int arg1, int arg2, uint arg3, uint arg4)
			: this()
		{
			initGuildPaddockBoughtMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5952;
		}
		
		public GuildPaddockBoughtMessage initGuildPaddockBoughtMessage(int arg1 = 0, int arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			this.worldX = arg1;
			this.worldY = arg2;
			this.nbMountMax = arg3;
			this.nbItemMax = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.worldX = 0;
			this.worldY = 0;
			this.nbMountMax = 0;
			this.nbItemMax = 0;
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
			this.serializeAs_GuildPaddockBoughtMessage(arg1);
		}
		
		public void serializeAs_GuildPaddockBoughtMessage(BigEndianWriter arg1)
		{
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element worldX.");
			}
			arg1.WriteShort((short)this.worldX);
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element worldY.");
			}
			arg1.WriteShort((short)this.worldY);
			if ( this.nbMountMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbMountMax + ") on element nbMountMax.");
			}
			arg1.WriteByte((byte)this.nbMountMax);
			if ( this.nbItemMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbItemMax + ") on element nbItemMax.");
			}
			arg1.WriteByte((byte)this.nbItemMax);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildPaddockBoughtMessage(arg1);
		}
		
		public void deserializeAs_GuildPaddockBoughtMessage(BigEndianReader arg1)
		{
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of GuildPaddockBoughtMessage.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of GuildPaddockBoughtMessage.worldY.");
			}
			this.nbMountMax = (uint)arg1.ReadByte();
			if ( this.nbMountMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbMountMax + ") on element of GuildPaddockBoughtMessage.nbMountMax.");
			}
			this.nbItemMax = (uint)arg1.ReadByte();
			if ( this.nbItemMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbItemMax + ") on element of GuildPaddockBoughtMessage.nbItemMax.");
			}
		}
		
	}
}
