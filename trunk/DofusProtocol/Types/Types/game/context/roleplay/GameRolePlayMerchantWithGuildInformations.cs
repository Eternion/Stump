// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameRolePlayMerchantWithGuildInformations.xml' the '30/06/2011 11:40:23'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Types
{
	public class GameRolePlayMerchantWithGuildInformations : GameRolePlayMerchantInformations
	{
		public const uint Id = 146;
		public override short TypeId
		{
			get
			{
				return 146;
			}
		}
		
		public Types.GuildInformations guildInformations;
		
		public GameRolePlayMerchantWithGuildInformations()
		{
		}
		
		public GameRolePlayMerchantWithGuildInformations(int contextualId, Types.EntityLook look, Types.EntityDispositionInformations disposition, string name, int sellType, Types.GuildInformations guildInformations)
			 : base(contextualId, look, disposition, name, sellType)
		{
			this.guildInformations = guildInformations;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			guildInformations.Serialize(writer);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			guildInformations = new Types.GuildInformations();
			guildInformations.Deserialize(reader);
		}
	}
}