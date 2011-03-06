using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PVPActivationCostMessage : Message
	{
		public const uint protocolId = 1801;
		internal Boolean _isInitialized = false;
		public uint cost = 0;
		
		public PVPActivationCostMessage()
		{
		}
		
		public PVPActivationCostMessage(uint arg1)
			: this()
		{
			initPVPActivationCostMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 1801;
		}
		
		public PVPActivationCostMessage initPVPActivationCostMessage(uint arg1 = 0)
		{
			this.cost = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.cost = 0;
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
			this.serializeAs_PVPActivationCostMessage(arg1);
		}
		
		public void serializeAs_PVPActivationCostMessage(BigEndianWriter arg1)
		{
			if ( this.cost < 0 )
			{
				throw new Exception("Forbidden value (" + this.cost + ") on element cost.");
			}
			arg1.WriteShort((short)this.cost);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PVPActivationCostMessage(arg1);
		}
		
		public void deserializeAs_PVPActivationCostMessage(BigEndianReader arg1)
		{
			this.cost = (uint)arg1.ReadShort();
			if ( this.cost < 0 )
			{
				throw new Exception("Forbidden value (" + this.cost + ") on element of PVPActivationCostMessage.cost.");
			}
		}
		
	}
}
