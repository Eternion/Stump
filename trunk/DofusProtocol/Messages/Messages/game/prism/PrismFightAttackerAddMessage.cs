using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PrismFightAttackerAddMessage : Message
	{
		public const uint protocolId = 5893;
		internal Boolean _isInitialized = false;
		public double fightId = 0;
		public List<CharacterMinimalPlusLookAndGradeInformations> charactersDescription;
		
		public PrismFightAttackerAddMessage()
		{
			this.charactersDescription = new List<CharacterMinimalPlusLookAndGradeInformations>();
		}
		
		public PrismFightAttackerAddMessage(double arg1, List<CharacterMinimalPlusLookAndGradeInformations> arg2)
			: this()
		{
			initPrismFightAttackerAddMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5893;
		}
		
		public PrismFightAttackerAddMessage initPrismFightAttackerAddMessage(double arg1 = 0, List<CharacterMinimalPlusLookAndGradeInformations> arg2 = null)
		{
			this.fightId = arg1;
			this.charactersDescription = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.charactersDescription = new List<CharacterMinimalPlusLookAndGradeInformations>();
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
			this.serializeAs_PrismFightAttackerAddMessage(arg1);
		}
		
		public void serializeAs_PrismFightAttackerAddMessage(BigEndianWriter arg1)
		{
			arg1.WriteDouble(this.fightId);
			arg1.WriteShort((short)this.charactersDescription.Count);
			var loc1 = 0;
			while ( loc1 < this.charactersDescription.Count )
			{
				this.charactersDescription[loc1].serializeAs_CharacterMinimalPlusLookAndGradeInformations(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismFightAttackerAddMessage(arg1);
		}
		
		public void deserializeAs_PrismFightAttackerAddMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.fightId = (double)arg1.ReadDouble();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new CharacterMinimalPlusLookAndGradeInformations()) as CharacterMinimalPlusLookAndGradeInformations).deserialize(arg1);
				this.charactersDescription.Add((CharacterMinimalPlusLookAndGradeInformations)loc3);
				++loc2;
			}
		}
		
	}
}
