using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class TaxCollectorAttackedMessage : Message
	{
		public const uint protocolId = 5918;
		internal Boolean _isInitialized = false;
		public uint firstNameId = 0;
		public uint lastNameId = 0;
		public int worldX = 0;
		public int worldY = 0;
		public uint mapId = 0;
		
		public TaxCollectorAttackedMessage()
		{
		}
		
		public TaxCollectorAttackedMessage(uint arg1, uint arg2, int arg3, int arg4, uint arg5)
			: this()
		{
			initTaxCollectorAttackedMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 5918;
		}
		
		public TaxCollectorAttackedMessage initTaxCollectorAttackedMessage(uint arg1 = 0, uint arg2 = 0, int arg3 = 0, int arg4 = 0, uint arg5 = 0)
		{
			this.firstNameId = arg1;
			this.lastNameId = arg2;
			this.worldX = arg3;
			this.worldY = arg4;
			this.mapId = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.firstNameId = 0;
			this.lastNameId = 0;
			this.worldX = 0;
			this.worldY = 0;
			this.mapId = 0;
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
			this.serializeAs_TaxCollectorAttackedMessage(arg1);
		}
		
		public void serializeAs_TaxCollectorAttackedMessage(BigEndianWriter arg1)
		{
			if ( this.firstNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstNameId + ") on element firstNameId.");
			}
			arg1.WriteShort((short)this.firstNameId);
			if ( this.lastNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastNameId + ") on element lastNameId.");
			}
			arg1.WriteShort((short)this.lastNameId);
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element worldX.");
			}
			arg1.WriteShort((short)this.worldX);
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element worldY.");
			}
			arg1.WriteShort((short)this.worldY);
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element mapId.");
			}
			arg1.WriteInt((int)this.mapId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorAttackedMessage(arg1);
		}
		
		public void deserializeAs_TaxCollectorAttackedMessage(BigEndianReader arg1)
		{
			this.firstNameId = (uint)arg1.ReadShort();
			if ( this.firstNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstNameId + ") on element of TaxCollectorAttackedMessage.firstNameId.");
			}
			this.lastNameId = (uint)arg1.ReadShort();
			if ( this.lastNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastNameId + ") on element of TaxCollectorAttackedMessage.lastNameId.");
			}
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of TaxCollectorAttackedMessage.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of TaxCollectorAttackedMessage.worldY.");
			}
			this.mapId = (uint)arg1.ReadInt();
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element of TaxCollectorAttackedMessage.mapId.");
			}
		}
		
	}
}
