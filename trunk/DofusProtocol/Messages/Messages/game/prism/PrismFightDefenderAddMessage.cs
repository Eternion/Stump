using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PrismFightDefenderAddMessage : Message
	{
		public const uint protocolId = 5895;
		internal Boolean _isInitialized = false;
		public double fightId = 0;
		public CharacterMinimalPlusLookAndGradeInformations fighterMovementInformations;
		public Boolean inMain = false;
		
		public PrismFightDefenderAddMessage()
		{
			this.fighterMovementInformations = new CharacterMinimalPlusLookAndGradeInformations();
		}
		
		public PrismFightDefenderAddMessage(double arg1, CharacterMinimalPlusLookAndGradeInformations arg2, Boolean arg3)
			: this()
		{
			initPrismFightDefenderAddMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5895;
		}
		
		public PrismFightDefenderAddMessage initPrismFightDefenderAddMessage(double arg1 = 0, CharacterMinimalPlusLookAndGradeInformations arg2 = null, Boolean arg3 = false)
		{
			this.fightId = arg1;
			this.fighterMovementInformations = arg2;
			this.inMain = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.fighterMovementInformations = new CharacterMinimalPlusLookAndGradeInformations();
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
			this.serializeAs_PrismFightDefenderAddMessage(arg1);
		}
		
		public void serializeAs_PrismFightDefenderAddMessage(BigEndianWriter arg1)
		{
			arg1.WriteDouble(this.fightId);
			this.fighterMovementInformations.serializeAs_CharacterMinimalPlusLookAndGradeInformations(arg1);
			arg1.WriteBoolean(this.inMain);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismFightDefenderAddMessage(arg1);
		}
		
		public void deserializeAs_PrismFightDefenderAddMessage(BigEndianReader arg1)
		{
			this.fightId = (double)arg1.ReadDouble();
			this.fighterMovementInformations = new CharacterMinimalPlusLookAndGradeInformations();
			this.fighterMovementInformations.deserialize(arg1);
			this.inMain = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
