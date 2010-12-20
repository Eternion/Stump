using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PaddockToSellFilterMessage : Message
	{
		public const uint protocolId = 6161;
		internal Boolean _isInitialized = false;
		public int areaId = 0;
		public int atLeastNbMount = 0;
		public int atLeastNbMachine = 0;
		public uint maxPrice = 0;
		
		public PaddockToSellFilterMessage()
		{
		}
		
		public PaddockToSellFilterMessage(int arg1, int arg2, int arg3, uint arg4)
			: this()
		{
			initPaddockToSellFilterMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6161;
		}
		
		public PaddockToSellFilterMessage initPaddockToSellFilterMessage(int arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 0)
		{
			this.areaId = arg1;
			this.atLeastNbMount = arg2;
			this.atLeastNbMachine = arg3;
			this.maxPrice = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.areaId = 0;
			this.atLeastNbMount = 0;
			this.atLeastNbMachine = 0;
			this.maxPrice = 0;
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
			this.serializeAs_PaddockToSellFilterMessage(arg1);
		}
		
		public void serializeAs_PaddockToSellFilterMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.areaId);
			arg1.WriteByte((byte)this.atLeastNbMount);
			arg1.WriteByte((byte)this.atLeastNbMachine);
			if ( this.maxPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxPrice + ") on element maxPrice.");
			}
			arg1.WriteInt((int)this.maxPrice);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockToSellFilterMessage(arg1);
		}
		
		public void deserializeAs_PaddockToSellFilterMessage(BigEndianReader arg1)
		{
			this.areaId = (int)arg1.ReadInt();
			this.atLeastNbMount = (int)arg1.ReadByte();
			this.atLeastNbMachine = (int)arg1.ReadByte();
			this.maxPrice = (uint)arg1.ReadInt();
			if ( this.maxPrice < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxPrice + ") on element of PaddockToSellFilterMessage.maxPrice.");
			}
		}
		
	}
}
