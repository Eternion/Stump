using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MountEmoteIconUsedOkMessage : Message
	{
		public const uint protocolId = 5978;
		internal Boolean _isInitialized = false;
		public int mountId = 0;
		public uint reactionType = 0;
		
		public MountEmoteIconUsedOkMessage()
		{
		}
		
		public MountEmoteIconUsedOkMessage(int arg1, uint arg2)
			: this()
		{
			initMountEmoteIconUsedOkMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5978;
		}
		
		public MountEmoteIconUsedOkMessage initMountEmoteIconUsedOkMessage(int arg1 = 0, uint arg2 = 0)
		{
			this.mountId = arg1;
			this.reactionType = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.mountId = 0;
			this.reactionType = 0;
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
			this.serializeAs_MountEmoteIconUsedOkMessage(arg1);
		}
		
		public void serializeAs_MountEmoteIconUsedOkMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.mountId);
			if ( this.reactionType < 0 )
			{
				throw new Exception("Forbidden value (" + this.reactionType + ") on element reactionType.");
			}
			arg1.WriteByte((byte)this.reactionType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MountEmoteIconUsedOkMessage(arg1);
		}
		
		public void deserializeAs_MountEmoteIconUsedOkMessage(BigEndianReader arg1)
		{
			this.mountId = (int)arg1.ReadInt();
			this.reactionType = (uint)arg1.ReadByte();
			if ( this.reactionType < 0 )
			{
				throw new Exception("Forbidden value (" + this.reactionType + ") on element of MountEmoteIconUsedOkMessage.reactionType.");
			}
		}
		
	}
}
