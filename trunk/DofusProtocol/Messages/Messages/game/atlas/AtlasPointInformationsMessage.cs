using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class AtlasPointInformationsMessage : Message
	{
		public const uint protocolId = 5956;
		internal Boolean _isInitialized = false;
		public AtlasPointsInformations type;
		
		public AtlasPointInformationsMessage()
		{
			this.type = new AtlasPointsInformations();
		}
		
		public AtlasPointInformationsMessage(AtlasPointsInformations arg1)
			: this()
		{
			initAtlasPointInformationsMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5956;
		}
		
		public AtlasPointInformationsMessage initAtlasPointInformationsMessage(AtlasPointsInformations arg1 = null)
		{
			this.type = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.type = new AtlasPointsInformations();
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
			this.serializeAs_AtlasPointInformationsMessage(arg1);
		}
		
		public void serializeAs_AtlasPointInformationsMessage(BigEndianWriter arg1)
		{
			this.type.serializeAs_AtlasPointsInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AtlasPointInformationsMessage(arg1);
		}
		
		public void deserializeAs_AtlasPointInformationsMessage(BigEndianReader arg1)
		{
			this.type = new AtlasPointsInformations();
			this.type.deserialize(arg1);
		}
		
	}
}
