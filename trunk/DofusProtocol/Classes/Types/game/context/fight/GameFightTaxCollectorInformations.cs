using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameFightTaxCollectorInformations : GameFightAIInformations
	{
		public const uint protocolId = 48;
		public uint firstNameId = 0;
		public uint lastNameId = 0;
		public uint level = 0;
		
		public GameFightTaxCollectorInformations()
		{
		}
		
		public GameFightTaxCollectorInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, uint arg4, Boolean arg5, GameFightMinimalStats arg6, uint arg7, uint arg8, uint arg9)
			: this()
		{
			initGameFightTaxCollectorInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
		}
		
		public override uint getTypeId()
		{
			return 48;
		}
		
		public GameFightTaxCollectorInformations initGameFightTaxCollectorInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, uint arg4 = 2, Boolean arg5 = false, GameFightMinimalStats arg6 = null, uint arg7 = 0, uint arg8 = 0, uint arg9 = 0)
		{
			base.initGameFightAIInformations(arg1, arg2, arg3, arg4, arg5, arg6);
			this.firstNameId = arg7;
			this.lastNameId = arg8;
			this.level = arg9;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.firstNameId = 0;
			this.lastNameId = 0;
			this.level = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightTaxCollectorInformations(arg1);
		}
		
		public void serializeAs_GameFightTaxCollectorInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameFightAIInformations(arg1);
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
			if ( this.level < 0 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element level.");
			}
			arg1.WriteShort((short)this.level);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightTaxCollectorInformations(arg1);
		}
		
		public void deserializeAs_GameFightTaxCollectorInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.firstNameId = (uint)arg1.ReadShort();
			if ( this.firstNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.firstNameId + ") on element of GameFightTaxCollectorInformations.firstNameId.");
			}
			this.lastNameId = (uint)arg1.ReadShort();
			if ( this.lastNameId < 0 )
			{
				throw new Exception("Forbidden value (" + this.lastNameId + ") on element of GameFightTaxCollectorInformations.lastNameId.");
			}
			this.level = (uint)arg1.ReadShort();
			if ( this.level < 0 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of GameFightTaxCollectorInformations.level.");
			}
		}
		
	}
}
