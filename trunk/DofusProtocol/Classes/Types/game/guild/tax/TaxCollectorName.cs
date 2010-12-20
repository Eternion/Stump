using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class TaxCollectorName : Object
	{
		public const uint protocolId = 187;
		public uint firstNameId = 0;
		public uint lastNameId = 0;
		
		public TaxCollectorName()
		{
		}
		
		public TaxCollectorName(uint arg1, uint arg2)
			: this()
		{
			initTaxCollectorName(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 187;
		}
		
		public TaxCollectorName initTaxCollectorName(uint arg1 = 0, uint arg2 = 0)
		{
			this.firstNameId = arg1;
			this.lastNameId = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.firstNameId = 0;
			this.lastNameId = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_TaxCollectorName(arg1);
		}
		
		public void serializeAs_TaxCollectorName(BigEndianWriter arg1)
		{
			if ( this.firstNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstNameId + ") on element firstNameId.");
			}
			arg1.WriteShort((short)this.firstNameId);
			if ( this.lastNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastNameId + ") on element lastNameId.");
			}
			arg1.WriteShort((short)this.lastNameId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorName(arg1);
		}
		
		public void deserializeAs_TaxCollectorName(BigEndianReader arg1)
		{
			this.firstNameId = (uint)arg1.ReadShort();
			if ( this.firstNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstNameId + ") on element of TaxCollectorName.firstNameId.");
			}
			this.lastNameId = (uint)arg1.ReadShort();
			if ( this.lastNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastNameId + ") on element of TaxCollectorName.lastNameId.");
			}
		}
		
	}
}
