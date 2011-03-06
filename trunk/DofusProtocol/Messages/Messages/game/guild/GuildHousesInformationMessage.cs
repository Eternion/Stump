using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildHousesInformationMessage : Message
	{
		public const uint protocolId = 5919;
		internal Boolean _isInitialized = false;
		public List<HouseInformationsForGuild> housesInformations;
		
		public GuildHousesInformationMessage()
		{
			this.housesInformations = new List<HouseInformationsForGuild>();
		}
		
		public GuildHousesInformationMessage(List<HouseInformationsForGuild> arg1)
			: this()
		{
			initGuildHousesInformationMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5919;
		}
		
		public GuildHousesInformationMessage initGuildHousesInformationMessage(List<HouseInformationsForGuild> arg1)
		{
			this.housesInformations = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.housesInformations = new List<HouseInformationsForGuild>();
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GuildHousesInformationMessage(arg1);
		}
		
		public void serializeAs_GuildHousesInformationMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.housesInformations.Count);
			var loc1 = 0;
			while ( loc1 < this.housesInformations.Count )
			{
				this.housesInformations[loc1].serializeAs_HouseInformationsForGuild(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildHousesInformationMessage(arg1);
		}
		
		public void deserializeAs_GuildHousesInformationMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new HouseInformationsForGuild()) as HouseInformationsForGuild).deserialize(arg1);
				this.housesInformations.Add((HouseInformationsForGuild)loc3);
				++loc2;
			}
		}
		
	}
}
