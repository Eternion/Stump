using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PrismFightAttackedMessage : Message
	{
		public const uint protocolId = 5894;
		internal Boolean _isInitialized = false;
		public int worldX = 0;
		public int worldY = 0;
		public uint mapId = 0;
		public uint subareaId = 0;
		
		public PrismFightAttackedMessage()
		{
		}
		
		public PrismFightAttackedMessage(int arg1, int arg2, uint arg3, uint arg4)
			: this()
		{
			initPrismFightAttackedMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 5894;
		}
		
		public PrismFightAttackedMessage initPrismFightAttackedMessage(int arg1 = 0, int arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			this.worldX = arg1;
			this.worldY = arg2;
			this.mapId = arg3;
			this.subareaId = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.worldX = 0;
			this.worldY = 0;
			this.mapId = 0;
			this.subareaId = 0;
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
			this.serializeAs_PrismFightAttackedMessage(arg1);
		}
		
		public void serializeAs_PrismFightAttackedMessage(BigEndianWriter arg1)
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
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element mapId.");
			}
			arg1.WriteInt((int)this.mapId);
			if ( this.subareaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subareaId + ") on element subareaId.");
			}
			arg1.WriteShort((short)this.subareaId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismFightAttackedMessage(arg1);
		}
		
		public void deserializeAs_PrismFightAttackedMessage(BigEndianReader arg1)
		{
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of PrismFightAttackedMessage.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of PrismFightAttackedMessage.worldY.");
			}
			this.mapId = (uint)arg1.ReadInt();
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element of PrismFightAttackedMessage.mapId.");
			}
			this.subareaId = (uint)arg1.ReadShort();
			if ( this.subareaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subareaId + ") on element of PrismFightAttackedMessage.subareaId.");
			}
		}
		
	}
}
