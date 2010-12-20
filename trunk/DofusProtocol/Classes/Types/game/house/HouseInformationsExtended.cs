using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class HouseInformationsExtended : HouseInformations
	{
		public const uint protocolId = 112;
		public GuildInformations guildInfo;
		
		public HouseInformationsExtended()
		{
			this.guildInfo = new GuildInformations();
		}
		
		public HouseInformationsExtended(uint arg1, List<uint> arg2, String arg3, Boolean arg4, uint arg5, GuildInformations arg6)
			: this()
		{
			initHouseInformationsExtended(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getTypeId()
		{
			return 112;
		}
		
		public HouseInformationsExtended initHouseInformationsExtended(uint arg1 = 0, List<uint> arg2 = null, String arg3 = "", Boolean arg4 = false, uint arg5 = 0, GuildInformations arg6 = null)
		{
			base.initHouseInformations(arg1, arg2, arg3, arg4, arg5);
			this.guildInfo = arg6;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.guildInfo = new GuildInformations();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_HouseInformationsExtended(arg1);
		}
		
		public void serializeAs_HouseInformationsExtended(BigEndianWriter arg1)
		{
			base.serializeAs_HouseInformations(arg1);
			this.guildInfo.serializeAs_GuildInformations(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_HouseInformationsExtended(arg1);
		}
		
		public void deserializeAs_HouseInformationsExtended(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.guildInfo = new GuildInformations();
			this.guildInfo.deserialize(arg1);
		}
		
	}
}
