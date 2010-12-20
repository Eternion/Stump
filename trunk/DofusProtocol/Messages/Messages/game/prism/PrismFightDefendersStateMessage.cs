using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PrismFightDefendersStateMessage : Message
	{
		public const uint protocolId = 5899;
		internal Boolean _isInitialized = false;
		public double fightId = 0;
		public List<CharacterMinimalPlusLookAndGradeInformations> mainFighters;
		public List<CharacterMinimalPlusLookAndGradeInformations> reserveFighters;
		
		public PrismFightDefendersStateMessage()
		{
			this.mainFighters = new List<CharacterMinimalPlusLookAndGradeInformations>();
			this.reserveFighters = new List<CharacterMinimalPlusLookAndGradeInformations>();
		}
		
		public PrismFightDefendersStateMessage(double arg1, List<CharacterMinimalPlusLookAndGradeInformations> arg2, List<CharacterMinimalPlusLookAndGradeInformations> arg3)
			: this()
		{
			initPrismFightDefendersStateMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5899;
		}
		
		public PrismFightDefendersStateMessage initPrismFightDefendersStateMessage(double arg1 = 0, List<CharacterMinimalPlusLookAndGradeInformations> arg2 = null, List<CharacterMinimalPlusLookAndGradeInformations> arg3 = null)
		{
			this.fightId = arg1;
			this.mainFighters = arg2;
			this.reserveFighters = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.mainFighters = new List<CharacterMinimalPlusLookAndGradeInformations>();
			this.reserveFighters = new List<CharacterMinimalPlusLookAndGradeInformations>();
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
			this.serializeAs_PrismFightDefendersStateMessage(arg1);
		}
		
		public void serializeAs_PrismFightDefendersStateMessage(BigEndianWriter arg1)
		{
			arg1.WriteDouble(this.fightId);
			arg1.WriteShort((short)this.mainFighters.Count);
			var loc1 = 0;
			while ( loc1 < this.mainFighters.Count )
			{
				this.mainFighters[loc1].serializeAs_CharacterMinimalPlusLookAndGradeInformations(arg1);
				++loc1;
			}
			arg1.WriteShort((short)this.reserveFighters.Count);
			var loc2 = 0;
			while ( loc2 < this.reserveFighters.Count )
			{
				this.reserveFighters[loc2].serializeAs_CharacterMinimalPlusLookAndGradeInformations(arg1);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PrismFightDefendersStateMessage(arg1);
		}
		
		public void deserializeAs_PrismFightDefendersStateMessage(BigEndianReader arg1)
		{
			object loc5 = null;
			object loc6 = null;
			this.fightId = (double)arg1.ReadDouble();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc5 = new CharacterMinimalPlusLookAndGradeInformations()) as CharacterMinimalPlusLookAndGradeInformations).deserialize(arg1);
				this.mainFighters.Add((CharacterMinimalPlusLookAndGradeInformations)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				((loc6 = new CharacterMinimalPlusLookAndGradeInformations()) as CharacterMinimalPlusLookAndGradeInformations).deserialize(arg1);
				this.reserveFighters.Add((CharacterMinimalPlusLookAndGradeInformations)loc6);
				++loc4;
			}
		}
		
	}
}
