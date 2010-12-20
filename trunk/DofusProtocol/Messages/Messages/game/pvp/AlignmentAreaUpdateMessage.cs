using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class AlignmentAreaUpdateMessage : Message
	{
		public const uint protocolId = 6060;
		internal Boolean _isInitialized = false;
		public uint areaId = 0;
		public int side = 0;
		
		public AlignmentAreaUpdateMessage()
		{
		}
		
		public AlignmentAreaUpdateMessage(uint arg1, int arg2)
			: this()
		{
			initAlignmentAreaUpdateMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6060;
		}
		
		public AlignmentAreaUpdateMessage initAlignmentAreaUpdateMessage(uint arg1 = 0, int arg2 = 0)
		{
			this.areaId = arg1;
			this.side = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.areaId = 0;
			this.side = 0;
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
			this.serializeAs_AlignmentAreaUpdateMessage(arg1);
		}
		
		public void serializeAs_AlignmentAreaUpdateMessage(BigEndianWriter arg1)
		{
			if ( this.areaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.areaId + ") on element areaId.");
			}
			arg1.WriteShort((short)this.areaId);
			arg1.WriteByte((byte)this.side);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AlignmentAreaUpdateMessage(arg1);
		}
		
		public void deserializeAs_AlignmentAreaUpdateMessage(BigEndianReader arg1)
		{
			this.areaId = (uint)arg1.ReadShort();
			if ( this.areaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.areaId + ") on element of AlignmentAreaUpdateMessage.areaId.");
			}
			this.side = (int)arg1.ReadByte();
		}
		
	}
}
