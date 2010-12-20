using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class HouseInformationsExtended : HouseInformations
	{
		public const uint protocolId = 112;
		public String guildName = "";
		public GuildEmblem guildEmblem;
		
		public HouseInformationsExtended()
		{
			this.guildEmblem = new GuildEmblem();
		}
		
		public HouseInformationsExtended(uint arg1, List<uint> arg2, String arg3, Boolean arg4, uint arg5, String arg6, GuildEmblem arg7)
			: this()
		{
			initHouseInformationsExtended(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getTypeId()
		{
			return 112;
		}
		
		public HouseInformationsExtended initHouseInformationsExtended(uint arg1 = 0, List<uint> arg2 = null, String arg3 = "", Boolean arg4 = false, uint arg5 = 0, String arg6 = "", GuildEmblem arg7 = null)
		{
			base.initHouseInformations(arg1, arg2, arg3, arg4, arg5);
			this.guildName = arg6;
			this.guildEmblem = arg7;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.guildName = "";
			this.guildEmblem = new GuildEmblem();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_HouseInformationsExtended(arg1);
		}
		
		public void serializeAs_HouseInformationsExtended(BigEndianWriter arg1)
		{
			base.serializeAs_HouseInformations(arg1);
			arg1.WriteUTF((string)this.guildName);
			this.guildEmblem.serializeAs_GuildEmblem(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseInformationsExtended(arg1);
		}
		
		public void deserializeAs_HouseInformationsExtended(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.guildName = (String)arg1.ReadUTF();
			this.guildEmblem = new GuildEmblem();
			this.guildEmblem.deserialize(arg1);
		}
		
	}
}
