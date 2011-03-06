using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PrismWorldInformationMessage : Message
	{
		public const uint protocolId = 5854;
		internal Boolean _isInitialized = false;
		public uint nbSubOwned = 0;
		public uint subTotal = 0;
		public uint maxSub = 0;
		public List<PrismSubAreaInformation> subAreasInformation;
		public uint nbConqsOwned = 0;
		public uint conqsTotal = 0;
		public List<PrismConquestInformation> conquetesInformation;
		
		public PrismWorldInformationMessage()
		{
			this.subAreasInformation = new List<PrismSubAreaInformation>();
			this.conquetesInformation = new List<PrismConquestInformation>();
		}
		
		public PrismWorldInformationMessage(uint arg1, uint arg2, uint arg3, List<PrismSubAreaInformation> arg4, uint arg5, uint arg6, List<PrismConquestInformation> arg7)
			: this()
		{
			initPrismWorldInformationMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getMessageId()
		{
			return 5854;
		}
		
		public PrismWorldInformationMessage initPrismWorldInformationMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, List<PrismSubAreaInformation> arg4 = null, uint arg5 = 0, uint arg6 = 0, List<PrismConquestInformation> arg7 = null)
		{
			this.nbSubOwned = arg1;
			this.subTotal = arg2;
			this.maxSub = arg3;
			this.subAreasInformation = arg4;
			this.nbConqsOwned = arg5;
			this.conqsTotal = arg6;
			this.conquetesInformation = arg7;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.nbSubOwned = 0;
			this.subTotal = 0;
			this.maxSub = 0;
			this.subAreasInformation = new List<PrismSubAreaInformation>();
			this.nbConqsOwned = 0;
			this.conqsTotal = 0;
			this.conquetesInformation = new List<PrismConquestInformation>();
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
			this.serializeAs_PrismWorldInformationMessage(arg1);
		}
		
		public void serializeAs_PrismWorldInformationMessage(BigEndianWriter arg1)
		{
			if ( this.nbSubOwned < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbSubOwned + ") on element nbSubOwned.");
			}
			arg1.WriteInt((int)this.nbSubOwned);
			if ( this.subTotal < 0 )
			{
				throw new Exception("Forbidden value (" + this.subTotal + ") on element subTotal.");
			}
			arg1.WriteInt((int)this.subTotal);
			if ( this.maxSub < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxSub + ") on element maxSub.");
			}
			arg1.WriteInt((int)this.maxSub);
			arg1.WriteShort((short)this.subAreasInformation.Count);
			var loc1 = 0;
			while ( loc1 < this.subAreasInformation.Count )
			{
				this.subAreasInformation[loc1].serializeAs_PrismSubAreaInformation(arg1);
				++loc1;
			}
			if ( this.nbConqsOwned < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbConqsOwned + ") on element nbConqsOwned.");
			}
			arg1.WriteInt((int)this.nbConqsOwned);
			if ( this.conqsTotal < 0 )
			{
				throw new Exception("Forbidden value (" + this.conqsTotal + ") on element conqsTotal.");
			}
			arg1.WriteInt((int)this.conqsTotal);
			arg1.WriteShort((short)this.conquetesInformation.Count);
			var loc2 = 0;
			while ( loc2 < this.conquetesInformation.Count )
			{
				this.conquetesInformation[loc2].serializeAs_PrismConquestInformation(arg1);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismWorldInformationMessage(arg1);
		}
		
		public void deserializeAs_PrismWorldInformationMessage(BigEndianReader arg1)
		{
			object loc5 = null;
			object loc6 = null;
			this.nbSubOwned = (uint)arg1.ReadInt();
			if ( this.nbSubOwned < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbSubOwned + ") on element of PrismWorldInformationMessage.nbSubOwned.");
			}
			this.subTotal = (uint)arg1.ReadInt();
			if ( this.subTotal < 0 )
			{
				throw new Exception("Forbidden value (" + this.subTotal + ") on element of PrismWorldInformationMessage.subTotal.");
			}
			this.maxSub = (uint)arg1.ReadInt();
			if ( this.maxSub < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxSub + ") on element of PrismWorldInformationMessage.maxSub.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc5 = new PrismSubAreaInformation()) as PrismSubAreaInformation).deserialize(arg1);
				this.subAreasInformation.Add((PrismSubAreaInformation)loc5);
				++loc2;
			}
			this.nbConqsOwned = (uint)arg1.ReadInt();
			if ( this.nbConqsOwned < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbConqsOwned + ") on element of PrismWorldInformationMessage.nbConqsOwned.");
			}
			this.conqsTotal = (uint)arg1.ReadInt();
			if ( this.conqsTotal < 0 )
			{
				throw new Exception("Forbidden value (" + this.conqsTotal + ") on element of PrismWorldInformationMessage.conqsTotal.");
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				((loc6 = new PrismConquestInformation()) as PrismConquestInformation).deserialize(arg1);
				this.conquetesInformation.Add((PrismConquestInformation)loc6);
				++loc4;
			}
		}
		
	}
}
