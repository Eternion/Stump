using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class PrismConquestInformation : Object
	{
		public const uint protocolId = 152;
		public uint subId = 0;
		public uint alignment = 0;
		public Boolean isEntered = false;
		public Boolean isInRoom = false;
		
		public PrismConquestInformation()
		{
		}
		
		public PrismConquestInformation(uint arg1, uint arg2, Boolean arg3, Boolean arg4)
			: this()
		{
			initPrismConquestInformation(arg1, arg2, arg3, arg4);
		}
		
		public virtual uint getTypeId()
		{
			return 152;
		}
		
		public PrismConquestInformation initPrismConquestInformation(uint arg1 = 0, uint arg2 = 0, Boolean arg3 = false, Boolean arg4 = false)
		{
			this.subId = arg1;
			this.alignment = arg2;
			this.isEntered = arg3;
			this.isInRoom = arg4;
			return this;
		}
		
		public virtual void reset()
		{
			this.subId = 0;
			this.alignment = 0;
			this.isEntered = false;
			this.isInRoom = false;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PrismConquestInformation(arg1);
		}
		
		public void serializeAs_PrismConquestInformation(BigEndianWriter arg1)
		{
			var loc1 = 0;
			BooleanByteWrapper.SetFlag(loc1, 0, this.isEntered);
			BooleanByteWrapper.SetFlag(loc1, 1, this.isInRoom);
			arg1.WriteByte((byte)loc1);
			if ( this.subId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subId + ") on element subId.");
			}
			arg1.WriteInt((int)this.subId);
			if ( this.alignment < 0 )
			{
				throw new Exception("Forbidden value (" + this.alignment + ") on element alignment.");
			}
			arg1.WriteByte((byte)this.alignment);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismConquestInformation(arg1);
		}
		
		public void deserializeAs_PrismConquestInformation(BigEndianReader arg1)
		{
			var loc1 = arg1.ReadByte();
			this.isEntered = (Boolean)BooleanByteWrapper.GetFlag(loc1, 0);
			this.isInRoom = (Boolean)BooleanByteWrapper.GetFlag(loc1, 1);
			this.subId = (uint)arg1.ReadInt();
			if ( this.subId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subId + ") on element of PrismConquestInformation.subId.");
			}
			this.alignment = (uint)arg1.ReadByte();
			if ( this.alignment < 0 )
			{
				throw new Exception("Forbidden value (" + this.alignment + ") on element of PrismConquestInformation.alignment.");
			}
		}
		
	}
}
