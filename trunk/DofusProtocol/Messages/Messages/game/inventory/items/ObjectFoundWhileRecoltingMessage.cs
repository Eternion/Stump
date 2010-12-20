using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectFoundWhileRecoltingMessage : Message
	{
		public const uint protocolId = 6017;
		internal Boolean _isInitialized = false;
		public uint genericId = 0;
		public uint quantity = 0;
		public uint ressourceGenericId = 0;
		
		public ObjectFoundWhileRecoltingMessage()
		{
		}
		
		public ObjectFoundWhileRecoltingMessage(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initObjectFoundWhileRecoltingMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6017;
		}
		
		public ObjectFoundWhileRecoltingMessage initObjectFoundWhileRecoltingMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			this.genericId = arg1;
			this.quantity = arg2;
			this.ressourceGenericId = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.genericId = 0;
			this.quantity = 0;
			this.ressourceGenericId = 0;
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
			this.serializeAs_ObjectFoundWhileRecoltingMessage(arg1);
		}
		
		public void serializeAs_ObjectFoundWhileRecoltingMessage(BigEndianWriter arg1)
		{
			if ( this.genericId < 0 )
			{
				throw new Exception("Forbidden value (" + this.genericId + ") on element genericId.");
			}
			arg1.WriteInt((int)this.genericId);
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element quantity.");
			}
			arg1.WriteInt((int)this.quantity);
			if ( this.ressourceGenericId < 0 )
			{
				throw new Exception("Forbidden value (" + this.ressourceGenericId + ") on element ressourceGenericId.");
			}
			arg1.WriteInt((int)this.ressourceGenericId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectFoundWhileRecoltingMessage(arg1);
		}
		
		public void deserializeAs_ObjectFoundWhileRecoltingMessage(BigEndianReader arg1)
		{
			this.genericId = (uint)arg1.ReadInt();
			if ( this.genericId < 0 )
			{
				throw new Exception("Forbidden value (" + this.genericId + ") on element of ObjectFoundWhileRecoltingMessage.genericId.");
			}
			this.quantity = (uint)arg1.ReadInt();
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element of ObjectFoundWhileRecoltingMessage.quantity.");
			}
			this.ressourceGenericId = (uint)arg1.ReadInt();
			if ( this.ressourceGenericId < 0 )
			{
				throw new Exception("Forbidden value (" + this.ressourceGenericId + ") on element of ObjectFoundWhileRecoltingMessage.ressourceGenericId.");
			}
		}
		
	}
}
