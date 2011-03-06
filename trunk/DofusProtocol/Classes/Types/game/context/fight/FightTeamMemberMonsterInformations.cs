using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FightTeamMemberMonsterInformations : FightTeamMemberInformations
	{
		public const uint protocolId = 6;
		public int monsterId = 0;
		public uint grade = 0;
		
		public FightTeamMemberMonsterInformations()
		{
		}
		
		public FightTeamMemberMonsterInformations(int arg1, int arg2, uint arg3)
			: this()
		{
			initFightTeamMemberMonsterInformations(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 6;
		}
		
		public FightTeamMemberMonsterInformations initFightTeamMemberMonsterInformations(int arg1 = 0, int arg2 = 0, uint arg3 = 0)
		{
			base.initFightTeamMemberInformations(arg1);
			this.monsterId = arg2;
			this.grade = arg3;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.monsterId = 0;
			this.grade = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightTeamMemberMonsterInformations(arg1);
		}
		
		public void serializeAs_FightTeamMemberMonsterInformations(BigEndianWriter arg1)
		{
			base.serializeAs_FightTeamMemberInformations(arg1);
			arg1.WriteInt((int)this.monsterId);
			if ( this.grade < 0 )
			{
				throw new Exception("Forbidden value (" + this.grade + ") on element grade.");
			}
			arg1.WriteByte((byte)this.grade);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightTeamMemberMonsterInformations(arg1);
		}
		
		public void deserializeAs_FightTeamMemberMonsterInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.monsterId = (int)arg1.ReadInt();
			this.grade = (uint)arg1.ReadByte();
			if ( this.grade < 0 )
			{
				throw new Exception("Forbidden value (" + this.grade + ") on element of FightTeamMemberMonsterInformations.grade.");
			}
		}
		
	}
}
