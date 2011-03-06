using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class StartupActionsObjetAttributionMessage : Message
	{
		public const uint protocolId = 1303;
		internal Boolean _isInitialized = false;
		public uint actionId = 0;
		public uint characterId = 0;
		
		public StartupActionsObjetAttributionMessage()
		{
		}
		
		public StartupActionsObjetAttributionMessage(uint arg1, uint arg2)
			: this()
		{
			initStartupActionsObjetAttributionMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 1303;
		}
		
		public StartupActionsObjetAttributionMessage initStartupActionsObjetAttributionMessage(uint arg1 = 0, uint arg2 = 0)
		{
			this.actionId = arg1;
			this.characterId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.actionId = 0;
			this.characterId = 0;
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
			this.serializeAs_StartupActionsObjetAttributionMessage(arg1);
		}
		
		public void serializeAs_StartupActionsObjetAttributionMessage(BigEndianWriter arg1)
		{
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element actionId.");
			}
			arg1.WriteInt((int)this.actionId);
			if ( this.characterId < 0 )
			{
				throw new Exception("Forbidden value (" + this.characterId + ") on element characterId.");
			}
			arg1.WriteInt((int)this.characterId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StartupActionsObjetAttributionMessage(arg1);
		}
		
		public void deserializeAs_StartupActionsObjetAttributionMessage(BigEndianReader arg1)
		{
			this.actionId = (uint)arg1.ReadInt();
			if ( this.actionId < 0 )
			{
				throw new Exception("Forbidden value (" + this.actionId + ") on element of StartupActionsObjetAttributionMessage.actionId.");
			}
			this.characterId = (uint)arg1.ReadInt();
			if ( this.characterId < 0 )
			{
				throw new Exception("Forbidden value (" + this.characterId + ") on element of StartupActionsObjetAttributionMessage.characterId.");
			}
		}
		
	}
}
