using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class LivingObjectChangeSkinRequestMessage : Message
	{
		public const uint protocolId = 5725;
		internal Boolean _isInitialized = false;
		public uint livingUID = 0;
		public uint livingPosition = 0;
		public uint skinId = 0;
		
		public LivingObjectChangeSkinRequestMessage()
		{
		}
		
		public LivingObjectChangeSkinRequestMessage(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initLivingObjectChangeSkinRequestMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5725;
		}
		
		public LivingObjectChangeSkinRequestMessage initLivingObjectChangeSkinRequestMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.livingUID = arg1;
			this.livingPosition = arg2;
			this.skinId = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.livingUID = 0;
			this.livingPosition = 0;
			this.skinId = 0;
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
			this.serializeAs_LivingObjectChangeSkinRequestMessage(arg1);
		}
		
		public void serializeAs_LivingObjectChangeSkinRequestMessage(BigEndianWriter arg1)
		{
			if ( this.livingUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.livingUID + ") on element livingUID.");
			}
			arg1.WriteInt((int)this.livingUID);
			if ( this.livingPosition < 0 || this.livingPosition > 255 )
			{
				throw new Exception("Forbidden value (" + this.livingPosition + ") on element livingPosition.");
			}
			arg1.WriteByte((byte)this.livingPosition);
			if ( this.skinId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skinId + ") on element skinId.");
			}
			arg1.WriteInt((int)this.skinId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_LivingObjectChangeSkinRequestMessage(arg1);
		}
		
		public void deserializeAs_LivingObjectChangeSkinRequestMessage(BigEndianReader arg1)
		{
			this.livingUID = (uint)arg1.ReadInt();
			if ( this.livingUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.livingUID + ") on element of LivingObjectChangeSkinRequestMessage.livingUID.");
			}
			this.livingPosition = (uint)arg1.ReadByte();
			if ( this.livingPosition < 0 || this.livingPosition > 255 )
			{
				throw new Exception("Forbidden value (" + this.livingPosition + ") on element of LivingObjectChangeSkinRequestMessage.livingPosition.");
			}
			this.skinId = (uint)arg1.ReadInt();
			if ( this.skinId < 0 )
			{
				throw new Exception("Forbidden value (" + this.skinId + ") on element of LivingObjectChangeSkinRequestMessage.skinId.");
			}
		}
		
	}
}
