using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class DisplayNumericalValueMessage : Message
	{
		public const uint protocolId = 5808;
		internal Boolean _isInitialized = false;
		public int entityId = 0;
		public int value = 0;
		public uint type = 0;
		
		public DisplayNumericalValueMessage()
		{
		}
		
		public DisplayNumericalValueMessage(int arg1, int arg2, uint arg3)
			: this()
		{
			initDisplayNumericalValueMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5808;
		}
		
		public DisplayNumericalValueMessage initDisplayNumericalValueMessage(int arg1 = 0, int arg2 = 0, uint arg3 = 0)
		{
			this.entityId = arg1;
			this.value = arg2;
			this.type = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.entityId = 0;
			this.value = 0;
			this.type = 0;
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
			this.serializeAs_DisplayNumericalValueMessage(arg1);
		}
		
		public void serializeAs_DisplayNumericalValueMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.entityId);
			arg1.WriteInt((int)this.value);
			arg1.WriteByte((byte)this.type);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_DisplayNumericalValueMessage(arg1);
		}
		
		public void deserializeAs_DisplayNumericalValueMessage(BigEndianReader arg1)
		{
			this.entityId = (int)arg1.ReadInt();
			this.value = (int)arg1.ReadInt();
			this.type = (uint)arg1.ReadByte();
			if ( this.type < 0 )
			{
				throw new Exception("Forbidden value (" + this.type + ") on element of DisplayNumericalValueMessage.type.");
			}
		}
		
	}
}
