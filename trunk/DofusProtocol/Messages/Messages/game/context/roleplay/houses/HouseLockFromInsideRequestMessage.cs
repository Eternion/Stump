using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class HouseLockFromInsideRequestMessage : LockableChangeCodeMessage
	{
		public const uint protocolId = 5885;
		internal Boolean _isInitialized = false;
		
		public HouseLockFromInsideRequestMessage()
		{
		}
		
		public HouseLockFromInsideRequestMessage(String arg1)
			: this()
		{
			initHouseLockFromInsideRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5885;
		}
		
		public HouseLockFromInsideRequestMessage initHouseLockFromInsideRequestMessage(String arg1 = "")
		{
			base.initLockableChangeCodeMessage(arg1);
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
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
			this.serializeAs_HouseLockFromInsideRequestMessage(arg1);
		}
		
		public void serializeAs_HouseLockFromInsideRequestMessage(BigEndianWriter arg1)
		{
			base.serializeAs_LockableChangeCodeMessage(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseLockFromInsideRequestMessage(arg1);
		}
		
		public void deserializeAs_HouseLockFromInsideRequestMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
		}
		
	}
}
