using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MoodSmileyRequestMessage : Message
	{
		public const uint protocolId = 6192;
		internal Boolean _isInitialized = false;
		public int smileyId = 0;
		
		public MoodSmileyRequestMessage()
		{
		}
		
		public MoodSmileyRequestMessage(int arg1)
			: this()
		{
			initMoodSmileyRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6192;
		}
		
		public MoodSmileyRequestMessage initMoodSmileyRequestMessage(int arg1 = 0)
		{
			this.smileyId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.smileyId = 0;
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
			this.serializeAs_MoodSmileyRequestMessage(arg1);
		}
		
		public void serializeAs_MoodSmileyRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.smileyId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MoodSmileyRequestMessage(arg1);
		}
		
		public void deserializeAs_MoodSmileyRequestMessage(BigEndianReader arg1)
		{
			this.smileyId = (int)arg1.ReadByte();
		}
		
	}
}
