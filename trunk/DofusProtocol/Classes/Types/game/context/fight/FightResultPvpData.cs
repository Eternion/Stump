using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FightResultPvpData : FightResultAdditionalData
	{
		public const uint protocolId = 190;
		public uint grade = 0;
		public uint minHonorForGrade = 0;
		public uint maxHonorForGrade = 0;
		public uint honor = 0;
		public int honorDelta = 0;
		public uint dishonor = 0;
		public int dishonorDelta = 0;
		
		public FightResultPvpData()
		{
		}
		
		public FightResultPvpData(uint arg1, uint arg2, uint arg3, uint arg4, int arg5, uint arg6, int arg7)
			: this()
		{
			initFightResultPvpData(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getTypeId()
		{
			return 190;
		}
		
		public FightResultPvpData initFightResultPvpData(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0, int arg5 = 0, uint arg6 = 0, int arg7 = 0)
		{
			this.grade = arg1;
			this.minHonorForGrade = arg2;
			this.maxHonorForGrade = arg3;
			this.honor = arg4;
			this.honorDelta = arg5;
			this.dishonor = arg6;
			this.dishonorDelta = arg7;
			return this;
		}
		
		public override void reset()
		{
			this.grade = 0;
			this.minHonorForGrade = 0;
			this.maxHonorForGrade = 0;
			this.honor = 0;
			this.honorDelta = 0;
			this.dishonor = 0;
			this.dishonorDelta = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightResultPvpData(arg1);
		}
		
		public void serializeAs_FightResultPvpData(BigEndianWriter arg1)
		{
			base.serializeAs_FightResultAdditionalData(arg1);
			if ( this.grade < 0 || this.grade > 255 )
			{
				throw new Exception("Forbidden value (" + this.grade + ") on element grade.");
			}
			arg1.WriteByte((byte)this.grade);
			if ( this.minHonorForGrade < 0 || this.minHonorForGrade > 20000 )
			{
				throw new Exception("Forbidden value (" + this.minHonorForGrade + ") on element minHonorForGrade.");
			}
			arg1.WriteShort((short)this.minHonorForGrade);
			if ( this.maxHonorForGrade < 0 || this.maxHonorForGrade > 20000 )
			{
				throw new Exception("Forbidden value (" + this.maxHonorForGrade + ") on element maxHonorForGrade.");
			}
			arg1.WriteShort((short)this.maxHonorForGrade);
			if ( this.honor < 0 || this.honor > 20000 )
			{
				throw new Exception("Forbidden value (" + this.honor + ") on element honor.");
			}
			arg1.WriteShort((short)this.honor);
			arg1.WriteShort((short)this.honorDelta);
			if ( this.dishonor < 0 || this.dishonor > 500 )
			{
				throw new Exception("Forbidden value (" + this.dishonor + ") on element dishonor.");
			}
			arg1.WriteShort((short)this.dishonor);
			arg1.WriteShort((short)this.dishonorDelta);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightResultPvpData(arg1);
		}
		
		public void deserializeAs_FightResultPvpData(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.grade = (uint)arg1.ReadByte();
			if ( this.grade < 0 || this.grade > 255 )
			{
				throw new Exception("Forbidden value (" + this.grade + ") on element of FightResultPvpData.grade.");
			}
			this.minHonorForGrade = (uint)arg1.ReadUShort();
			if ( this.minHonorForGrade < 0 || this.minHonorForGrade > 20000 )
			{
				throw new Exception("Forbidden value (" + this.minHonorForGrade + ") on element of FightResultPvpData.minHonorForGrade.");
			}
			this.maxHonorForGrade = (uint)arg1.ReadUShort();
			if ( this.maxHonorForGrade < 0 || this.maxHonorForGrade > 20000 )
			{
				throw new Exception("Forbidden value (" + this.maxHonorForGrade + ") on element of FightResultPvpData.maxHonorForGrade.");
			}
			this.honor = (uint)arg1.ReadUShort();
			if ( this.honor < 0 || this.honor > 20000 )
			{
				throw new Exception("Forbidden value (" + this.honor + ") on element of FightResultPvpData.honor.");
			}
			this.honorDelta = (int)arg1.ReadShort();
			this.dishonor = (uint)arg1.ReadUShort();
			if ( this.dishonor < 0 || this.dishonor > 500 )
			{
				throw new Exception("Forbidden value (" + this.dishonor + ") on element of FightResultPvpData.dishonor.");
			}
			this.dishonorDelta = (int)arg1.ReadShort();
		}
		
	}
}
