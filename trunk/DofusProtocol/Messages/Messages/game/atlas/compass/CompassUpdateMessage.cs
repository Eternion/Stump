using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CompassUpdateMessage : Message
	{
		public const uint protocolId = 5591;
		internal Boolean _isInitialized = false;
		public uint type = 0;
		public int worldX = 0;
		public int worldY = 0;
		
		public CompassUpdateMessage()
		{
		}
		
		public CompassUpdateMessage(uint arg1, int arg2, int arg3)
			: this()
		{
			initCompassUpdateMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5591;
		}
		
		public CompassUpdateMessage initCompassUpdateMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0)
		{
			this.type = arg1;
			this.worldX = arg2;
			this.worldY = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.type = 0;
			this.worldX = 0;
			this.worldY = 0;
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
			this.serializeAs_CompassUpdateMessage(arg1);
		}
		
		public void serializeAs_CompassUpdateMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.type);
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
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CompassUpdateMessage(arg1);
		}
		
		public void deserializeAs_CompassUpdateMessage(BigEndianReader arg1)
		{
			this.type = (uint)arg1.ReadByte();
			if ( this.type < 0 )
			{
				throw new Exception("Forbidden value (" + this.type + ") on element of CompassUpdateMessage.type.");
			}
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of CompassUpdateMessage.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of CompassUpdateMessage.worldY.");
			}
		}
		
	}
}
