using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameMapChangeOrientationRequestMessage : Message
	{
		public const uint protocolId = 945;
		internal Boolean _isInitialized = false;
		public uint direction = 1;
		
		public GameMapChangeOrientationRequestMessage()
		{
		}
		
		public GameMapChangeOrientationRequestMessage(uint arg1)
			: this()
		{
			initGameMapChangeOrientationRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 945;
		}
		
		public GameMapChangeOrientationRequestMessage initGameMapChangeOrientationRequestMessage(uint arg1 = 1)
		{
			this.direction = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.direction = 1;
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
			this.serializeAs_GameMapChangeOrientationRequestMessage(arg1);
		}
		
		public void serializeAs_GameMapChangeOrientationRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.direction);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameMapChangeOrientationRequestMessage(arg1);
		}
		
		public void deserializeAs_GameMapChangeOrientationRequestMessage(BigEndianReader arg1)
		{
			this.direction = (uint)arg1.ReadByte();
			if ( this.direction < 0 )
			{
				throw new Exception("Forbidden value (" + this.direction + ") on element of GameMapChangeOrientationRequestMessage.direction.");
			}
		}
		
	}
}
