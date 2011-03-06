using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class PaddockBuyableInformations : PaddockInformations
	{
		public const uint protocolId = 130;
		public uint price = 0;
		
		public PaddockBuyableInformations()
		{
		}
		
		public PaddockBuyableInformations(uint arg1, uint arg2, uint arg3)
			: this()
		{
			initPaddockBuyableInformations(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 130;
		}
		
		public PaddockBuyableInformations initPaddockBuyableInformations(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0)
		{
			base.initPaddockInformations(arg1, arg2);
			this.price = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.price = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PaddockBuyableInformations(arg1);
		}
		
		public void serializeAs_PaddockBuyableInformations(BigEndianWriter arg1)
		{
			base.serializeAs_PaddockInformations(arg1);
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element price.");
			}
			arg1.WriteInt((int)this.price);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockBuyableInformations(arg1);
		}
		
		public void deserializeAs_PaddockBuyableInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.price = (uint)arg1.ReadInt();
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element of PaddockBuyableInformations.price.");
			}
		}
		
	}
}
