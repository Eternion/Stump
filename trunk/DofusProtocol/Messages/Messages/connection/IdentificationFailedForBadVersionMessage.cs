using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class IdentificationFailedForBadVersionMessage : IdentificationFailedMessage
	{
		public const uint protocolId = 21;
		internal Boolean _isInitialized = false;
        public Stump.DofusProtocol.Classes.Version requiredVersion;
		
		public IdentificationFailedForBadVersionMessage()
		{
			this.requiredVersion = new Stump.DofusProtocol.Classes.Version();
		}
		
		public IdentificationFailedForBadVersionMessage(uint arg1, Stump.DofusProtocol.Classes.Version arg2)
			: this()
		{
			initIdentificationFailedForBadVersionMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 21;
		}
		
		public IdentificationFailedForBadVersionMessage initIdentificationFailedForBadVersionMessage(uint arg1 = 99, Stump.DofusProtocol.Classes.Version arg2 = null)
		{
			base.initIdentificationFailedMessage(arg1);
			this.requiredVersion = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.requiredVersion = new Stump.DofusProtocol.Classes.Version();
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_IdentificationFailedForBadVersionMessage(arg1);
		}
		
		public void serializeAs_IdentificationFailedForBadVersionMessage(BigEndianWriter arg1)
		{
			base.serializeAs_IdentificationFailedMessage(arg1);
			this.requiredVersion.serializeAs_Version(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IdentificationFailedForBadVersionMessage(arg1);
		}
		
		public void deserializeAs_IdentificationFailedForBadVersionMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.requiredVersion = new Stump.DofusProtocol.Classes.Version();
			this.requiredVersion.deserialize(arg1);
		}
		
	}
}
