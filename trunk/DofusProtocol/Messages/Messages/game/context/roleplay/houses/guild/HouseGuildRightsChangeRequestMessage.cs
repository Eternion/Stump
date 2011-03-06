using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class HouseGuildRightsChangeRequestMessage : Message
	{
		public const uint protocolId = 5702;
		internal Boolean _isInitialized = false;
		public uint rights = 0;
		
		public HouseGuildRightsChangeRequestMessage()
		{
		}
		
		public HouseGuildRightsChangeRequestMessage(uint arg1)
			: this()
		{
			initHouseGuildRightsChangeRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5702;
		}
		
		public HouseGuildRightsChangeRequestMessage initHouseGuildRightsChangeRequestMessage(uint arg1 = 0)
		{
			this.rights = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.rights = 0;
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
			this.serializeAs_HouseGuildRightsChangeRequestMessage(arg1);
		}
		
		public void serializeAs_HouseGuildRightsChangeRequestMessage(BigEndianWriter arg1)
		{
			if ( this.rights < 0 || this.rights > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.rights + ") on element rights.");
			}
			arg1.WriteUInt((uint)this.rights);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseGuildRightsChangeRequestMessage(arg1);
		}
		
		public void deserializeAs_HouseGuildRightsChangeRequestMessage(BigEndianReader arg1)
		{
			this.rights = (uint)arg1.ReadUInt();
			if ( this.rights < 0 || this.rights > 4294967295 )
			{
				throw new Exception("Forbidden value (" + this.rights + ") on element of HouseGuildRightsChangeRequestMessage.rights.");
			}
		}
		
	}
}
