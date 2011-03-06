using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameRolePlayMerchantWithGuildInformations : GameRolePlayMerchantInformations
	{
		public const uint protocolId = 146;
		public GuildInformations guildInformations;
		
		public GameRolePlayMerchantWithGuildInformations()
		{
			this.guildInformations = new GuildInformations();
		}
		
		public GameRolePlayMerchantWithGuildInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, String arg4, uint arg5, GuildInformations arg6)
			: this()
		{
			initGameRolePlayMerchantWithGuildInformations(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getTypeId()
		{
			return 146;
		}
		
		public GameRolePlayMerchantWithGuildInformations initGameRolePlayMerchantWithGuildInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, String arg4 = "", uint arg5 = 0, GuildInformations arg6 = null)
		{
			base.initGameRolePlayMerchantInformations(arg1, arg2, arg3, arg4, arg5);
			this.guildInformations = arg6;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.guildInformations = new GuildInformations();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameRolePlayMerchantWithGuildInformations(arg1);
		}
		
		public void serializeAs_GameRolePlayMerchantWithGuildInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameRolePlayMerchantInformations(arg1);
			this.guildInformations.serializeAs_GuildInformations(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayMerchantWithGuildInformations(arg1);
		}
		
		public void deserializeAs_GameRolePlayMerchantWithGuildInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.guildInformations = new GuildInformations();
			this.guildInformations.deserialize(arg1);
		}
		
	}
}
