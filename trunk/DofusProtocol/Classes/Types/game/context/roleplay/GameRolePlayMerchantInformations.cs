using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameRolePlayMerchantInformations : GameRolePlayNamedActorInformations
	{
		public const uint protocolId = 129;
		public uint sellType = 0;
		
		public GameRolePlayMerchantInformations()
		{
		}
		
		public GameRolePlayMerchantInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, String arg4, uint arg5)
			: this()
		{
			initGameRolePlayMerchantInformations(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getTypeId()
		{
			return 129;
		}
		
		public GameRolePlayMerchantInformations initGameRolePlayMerchantInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, String arg4 = "", uint arg5 = 0)
		{
			base.initGameRolePlayNamedActorInformations(arg1, arg2, arg3, arg4);
			this.sellType = arg5;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.sellType = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameRolePlayMerchantInformations(arg1);
		}
		
		public void serializeAs_GameRolePlayMerchantInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameRolePlayNamedActorInformations(arg1);
			if ( this.sellType < 0 )
			{
				throw new Exception("Forbidden value (" + this.sellType + ") on element sellType.");
			}
			arg1.WriteInt((int)this.sellType);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayMerchantInformations(arg1);
		}
		
		public void deserializeAs_GameRolePlayMerchantInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.sellType = (uint)arg1.ReadInt();
			if ( this.sellType < 0 )
			{
				throw new Exception("Forbidden value (" + this.sellType + ") on element of GameRolePlayMerchantInformations.sellType.");
			}
		}
		
	}
}
