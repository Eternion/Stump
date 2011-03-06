using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightOptionToggleMessage : Message
	{
		public const uint protocolId = 707;
		internal Boolean _isInitialized = false;
		public uint option = 3;
		
		public GameFightOptionToggleMessage()
		{
		}
		
		public GameFightOptionToggleMessage(uint arg1)
			: this()
		{
			initGameFightOptionToggleMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 707;
		}
		
		public GameFightOptionToggleMessage initGameFightOptionToggleMessage(uint arg1 = 3)
		{
			this.option = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.option = 3;
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
			this.serializeAs_GameFightOptionToggleMessage(arg1);
		}
		
		public void serializeAs_GameFightOptionToggleMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.option);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightOptionToggleMessage(arg1);
		}
		
		public void deserializeAs_GameFightOptionToggleMessage(BigEndianReader arg1)
		{
			this.option = (uint)arg1.ReadByte();
			if ( this.option < 0 )
			{
				throw new Exception("Forbidden value (" + this.option + ") on element of GameFightOptionToggleMessage.option.");
			}
		}
		
	}
}
