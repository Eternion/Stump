using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class KamasUpdateMessage : Message
	{
		public const uint protocolId = 5537;
		internal Boolean _isInitialized = false;
		public int kamasTotal = 0;
		
		public KamasUpdateMessage()
		{
		}
		
		public KamasUpdateMessage(int arg1)
			: this()
		{
			initKamasUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5537;
		}
		
		public KamasUpdateMessage initKamasUpdateMessage(int arg1 = 0)
		{
			this.kamasTotal = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.kamasTotal = 0;
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
			this.serializeAs_KamasUpdateMessage(arg1);
		}
		
		public void serializeAs_KamasUpdateMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.kamasTotal);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_KamasUpdateMessage(arg1);
		}
		
		public void deserializeAs_KamasUpdateMessage(BigEndianReader arg1)
		{
			this.kamasTotal = (int)arg1.ReadInt();
		}
		
	}
}
