using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameContextCreateMessage : Message
	{
		public const uint protocolId = 200;
		internal Boolean _isInitialized = false;
		public uint context = 1;
		
		public GameContextCreateMessage()
		{
		}
		
		public GameContextCreateMessage(uint arg1)
			: this()
		{
			initGameContextCreateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 200;
		}
		
		public GameContextCreateMessage initGameContextCreateMessage(uint arg1 = 1)
		{
			this.context = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.context = 1;
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
			this.serializeAs_GameContextCreateMessage(arg1);
		}
		
		public void serializeAs_GameContextCreateMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.context);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameContextCreateMessage(arg1);
		}
		
		public void deserializeAs_GameContextCreateMessage(BigEndianReader arg1)
		{
			this.context = (uint)arg1.ReadByte();
			if ( this.context < 0 )
			{
				throw new Exception("Forbidden value (" + this.context + ") on element of GameContextCreateMessage.context.");
			}
		}
		
	}
}
