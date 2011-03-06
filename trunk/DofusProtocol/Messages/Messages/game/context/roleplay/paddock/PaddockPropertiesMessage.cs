using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PaddockPropertiesMessage : Message
	{
		public const uint protocolId = 5824;
		internal Boolean _isInitialized = false;
		public PaddockInformations properties;
		
		public PaddockPropertiesMessage()
		{
			this.properties = new PaddockInformations();
		}
		
		public PaddockPropertiesMessage(PaddockInformations arg1)
			: this()
		{
			initPaddockPropertiesMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5824;
		}
		
		public PaddockPropertiesMessage initPaddockPropertiesMessage(PaddockInformations arg1 = null)
		{
			this.properties = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.properties = new PaddockInformations();
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
			this.serializeAs_PaddockPropertiesMessage(arg1);
		}
		
		public void serializeAs_PaddockPropertiesMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.properties.getTypeId());
			this.properties.serialize(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockPropertiesMessage(arg1);
		}
		
		public void deserializeAs_PaddockPropertiesMessage(BigEndianReader arg1)
		{
			var loc1 = (ushort)arg1.ReadUShort();
			this.properties = ProtocolTypeManager.GetInstance<PaddockInformations>((uint)loc1);
			this.properties.deserialize(arg1);
		}
		
	}
}
