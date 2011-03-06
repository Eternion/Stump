using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class IgnoredAddedMessage : Message
	{
		public const uint protocolId = 5678;
		internal Boolean _isInitialized = false;
		public IgnoredInformations ignoreAdded;
		public Boolean session = false;
		
		public IgnoredAddedMessage()
		{
			this.ignoreAdded = new IgnoredInformations();
		}
		
		public IgnoredAddedMessage(IgnoredInformations arg1, Boolean arg2)
			: this()
		{
			initIgnoredAddedMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5678;
		}
		
		public IgnoredAddedMessage initIgnoredAddedMessage(IgnoredInformations arg1 = null, Boolean arg2 = false)
		{
			this.ignoreAdded = arg1;
			this.session = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.ignoreAdded = new IgnoredInformations();
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
			this.serializeAs_IgnoredAddedMessage(arg1);
		}
		
		public void serializeAs_IgnoredAddedMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.ignoreAdded.getTypeId());
			this.ignoreAdded.serialize(arg1);
			arg1.WriteBoolean(this.session);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IgnoredAddedMessage(arg1);
		}
		
		public void deserializeAs_IgnoredAddedMessage(BigEndianReader arg1)
		{
			var loc1 = (ushort)arg1.ReadUShort();
			this.ignoreAdded = ProtocolTypeManager.GetInstance<IgnoredInformations>((uint)loc1);
			this.ignoreAdded.deserialize(arg1);
			this.session = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
