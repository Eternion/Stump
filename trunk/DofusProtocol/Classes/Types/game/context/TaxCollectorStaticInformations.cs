using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class TaxCollectorStaticInformations : Object
	{
		public const uint protocolId = 147;
		public uint firstNameId = 0;
		public uint lastNameId = 0;
		public GuildInformations guildIdentity;
		
		public TaxCollectorStaticInformations()
		{
			this.guildIdentity = new GuildInformations();
		}
		
		public TaxCollectorStaticInformations(uint arg1, uint arg2, GuildInformations arg3)
			: this()
		{
			initTaxCollectorStaticInformations(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 147;
		}
		
		public TaxCollectorStaticInformations initTaxCollectorStaticInformations(uint arg1 = 0, uint arg2 = 0, GuildInformations arg3 = null)
		{
			this.firstNameId = arg1;
			this.lastNameId = arg2;
			this.guildIdentity = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.firstNameId = 0;
			this.lastNameId = 0;
			this.guildIdentity = new GuildInformations();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_TaxCollectorStaticInformations(arg1);
		}
		
		public void serializeAs_TaxCollectorStaticInformations(BigEndianWriter arg1)
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
			this.guildIdentity.serializeAs_GuildInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorStaticInformations(arg1);
		}
		
		public void deserializeAs_TaxCollectorStaticInformations(BigEndianReader arg1)
		{
			this.firstNameId = (uint)arg1.ReadShort();
			if ( this.firstNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstNameId + ") on element of TaxCollectorStaticInformations.firstNameId.");
			}
			this.lastNameId = (uint)arg1.ReadShort();
			if ( this.lastNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastNameId + ") on element of TaxCollectorStaticInformations.lastNameId.");
			}
			this.guildIdentity = new GuildInformations();
			this.guildIdentity.deserialize(arg1);
		}
		
	}
}
