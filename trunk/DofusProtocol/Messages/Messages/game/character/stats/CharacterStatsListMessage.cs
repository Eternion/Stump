using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CharacterStatsListMessage : Message
	{
		public const uint protocolId = 500;
		internal Boolean _isInitialized = false;
		public CharacterCharacteristicsInformations stats;
		
		public CharacterStatsListMessage()
		{
			this.stats = new CharacterCharacteristicsInformations();
		}
		
		public CharacterStatsListMessage(CharacterCharacteristicsInformations arg1)
			: this()
		{
			initCharacterStatsListMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 500;
		}
		
		public CharacterStatsListMessage initCharacterStatsListMessage(CharacterCharacteristicsInformations arg1 = null)
		{
			this.stats = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.stats = new CharacterCharacteristicsInformations();
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
			this.serializeAs_CharacterStatsListMessage(arg1);
		}
		
		public void serializeAs_CharacterStatsListMessage(BigEndianWriter arg1)
		{
			this.stats.serializeAs_CharacterCharacteristicsInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterStatsListMessage(arg1);
		}
		
		public void deserializeAs_CharacterStatsListMessage(BigEndianReader arg1)
		{
			this.stats = new CharacterCharacteristicsInformations();
			this.stats.deserialize(arg1);
		}
		
	}
}
