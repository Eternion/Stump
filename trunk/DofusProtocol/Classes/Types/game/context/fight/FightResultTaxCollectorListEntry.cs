using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FightResultTaxCollectorListEntry : FightResultFighterListEntry
	{
		public const uint protocolId = 84;
		public uint level = 0;
		public BasicGuildInformations guildInfo;
		public int experienceForGuild = 0;
		
		public FightResultTaxCollectorListEntry()
		{
			this.guildInfo = new BasicGuildInformations();
		}
		
		public FightResultTaxCollectorListEntry(uint arg1, FightLoot arg2, int arg3, Boolean arg4, uint arg5, BasicGuildInformations arg6, int arg7)
			: this()
		{
			initFightResultTaxCollectorListEntry(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getTypeId()
		{
			return 84;
		}
		
		public FightResultTaxCollectorListEntry initFightResultTaxCollectorListEntry(uint arg1 = 0, FightLoot arg2 = null, int arg3 = 0, Boolean arg4 = false, uint arg5 = 0, BasicGuildInformations arg6 = null, int arg7 = 0)
		{
			base.initFightResultFighterListEntry(arg1, arg2, arg3, arg4);
			this.level = arg5;
			this.guildInfo = arg6;
			this.experienceForGuild = arg7;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.level = 0;
			this.guildInfo = new BasicGuildInformations();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightResultTaxCollectorListEntry(arg1);
		}
		
		public void serializeAs_FightResultTaxCollectorListEntry(BigEndianWriter arg1)
		{
			base.serializeAs_FightResultFighterListEntry(arg1);
			if ( this.level < 1 || this.level > 200 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element level.");
			}
			arg1.WriteByte((byte)this.level);
			this.guildInfo.serializeAs_BasicGuildInformations(arg1);
			arg1.WriteInt((int)this.experienceForGuild);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightResultTaxCollectorListEntry(arg1);
		}
		
		public void deserializeAs_FightResultTaxCollectorListEntry(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.level = (uint)arg1.ReadByte();
			if ( this.level < 1 || this.level > 200 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of FightResultTaxCollectorListEntry.level.");
			}
			this.guildInfo = new BasicGuildInformations();
			this.guildInfo.deserialize(arg1);
			this.experienceForGuild = (int)arg1.ReadInt();
		}
		
	}
}
