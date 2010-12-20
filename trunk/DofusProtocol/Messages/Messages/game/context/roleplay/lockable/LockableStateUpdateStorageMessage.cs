using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class LockableStateUpdateStorageMessage : LockableStateUpdateAbstractMessage
	{
		public const uint protocolId = 5669;
		internal Boolean _isInitialized = false;
		public int mapId = 0;
		public uint elementId = 0;
		
		public LockableStateUpdateStorageMessage()
		{
		}
		
		public LockableStateUpdateStorageMessage(Boolean arg1, int arg2, uint arg3)
			: this()
		{
			initLockableStateUpdateStorageMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5669;
		}
		
		public LockableStateUpdateStorageMessage initLockableStateUpdateStorageMessage(Boolean arg1 = false, int arg2 = 0, uint arg3 = 0)
		{
			base.initLockableStateUpdateAbstractMessage(arg1);
			this.mapId = arg2;
			this.elementId = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.mapId = 0;
			this.elementId = 0;
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
			this.serializeAs_LockableStateUpdateStorageMessage(arg1);
		}
		
		public void serializeAs_LockableStateUpdateStorageMessage(BigEndianWriter arg1)
		{
			base.serializeAs_LockableStateUpdateAbstractMessage(arg1);
			arg1.WriteInt((int)this.mapId);
			if ( this.elementId < 0 )
			{
				throw new Exception("Forbidden value (" + this.elementId + ") on element elementId.");
			}
			arg1.WriteInt((int)this.elementId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_LockableStateUpdateStorageMessage(arg1);
		}
		
		public void deserializeAs_LockableStateUpdateStorageMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.mapId = (int)arg1.ReadInt();
			this.elementId = (uint)arg1.ReadInt();
			if ( this.elementId < 0 )
			{
				throw new Exception("Forbidden value (" + this.elementId + ") on element of LockableStateUpdateStorageMessage.elementId.");
			}
		}
		
	}
}
