using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class PrismSubAreaInformation : Object
	{
		public const uint protocolId = 142;
		public uint subId = 0;
		public uint alignment = 0;
		public uint mapId = 0;
		public Boolean isInFight = false;
		public Boolean isFightable = false;
		
		public PrismSubAreaInformation()
		{
		}
		
		public PrismSubAreaInformation(uint arg1, uint arg2, uint arg3, Boolean arg4, Boolean arg5)
			: this()
		{
			initPrismSubAreaInformation(arg1, arg2, arg3, arg4, arg5);
		}
		
		public virtual uint getTypeId()
		{
			return 142;
		}
		
		public PrismSubAreaInformation initPrismSubAreaInformation(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, Boolean arg4 = false, Boolean arg5 = false)
		{
			this.subId = arg1;
			this.alignment = arg2;
			this.mapId = arg3;
			this.isInFight = arg4;
			this.isFightable = arg5;
			return this;
		}
		
		public virtual void reset()
		{
			this.subId = 0;
			this.alignment = 0;
			this.mapId = 0;
			this.isInFight = false;
			this.isFightable = false;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PrismSubAreaInformation(arg1);
		}
		
		public void serializeAs_PrismSubAreaInformation(BigEndianWriter arg1)
		{
			var loc1 = 0;
			BooleanByteWrapper.SetFlag(loc1, 0, this.isInFight);
			BooleanByteWrapper.SetFlag(loc1, 1, this.isFightable);
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
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element mapId.");
			}
			arg1.WriteInt((int)this.mapId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismSubAreaInformation(arg1);
		}
		
		public void deserializeAs_PrismSubAreaInformation(BigEndianReader arg1)
		{
			var loc1 = arg1.ReadByte();
			this.isInFight = (Boolean)BooleanByteWrapper.GetFlag(loc1, 0);
			this.isFightable = (Boolean)BooleanByteWrapper.GetFlag(loc1, 1);
			this.subId = (uint)arg1.ReadInt();
			if ( this.subId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subId + ") on element of PrismSubAreaInformation.subId.");
			}
			this.alignment = (uint)arg1.ReadByte();
			if ( this.alignment < 0 )
			{
				throw new Exception("Forbidden value (" + this.alignment + ") on element of PrismSubAreaInformation.alignment.");
			}
			this.mapId = (uint)arg1.ReadInt();
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element of PrismSubAreaInformation.mapId.");
			}
		}
		
	}
}
