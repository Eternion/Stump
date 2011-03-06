using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FightCommonInformations : Object
	{
		public const uint protocolId = 43;
		public int fightId = 0;
		public uint fightType = 0;
		public List<FightTeamInformations> fightTeams;
		public List<uint> fightTeamsPositions;
		public List<FightOptionsInformations> fightTeamsOptions;
		
		public FightCommonInformations()
		{
			this.fightTeams = new List<FightTeamInformations>();
			this.fightTeamsPositions = new List<uint>();
			this.fightTeamsOptions = new List<FightOptionsInformations>();
		}
		
		public FightCommonInformations(int arg1, uint arg2, List<FightTeamInformations> arg3, List<uint> arg4, List<FightOptionsInformations> arg5)
			: this()
		{
			initFightCommonInformations(arg1, arg2, arg3, arg4, arg5);
		}
		
		public virtual uint getTypeId()
		{
			return 43;
		}
		
		public FightCommonInformations initFightCommonInformations(int arg1 = 0, uint arg2 = 0, List<FightTeamInformations> arg3 = null, List<uint> arg4 = null, List<FightOptionsInformations> arg5 = null)
		{
			this.fightId = arg1;
			this.fightType = arg2;
			this.fightTeams = arg3;
			this.fightTeamsPositions = arg4;
			this.fightTeamsOptions = arg5;
			return this;
		}
		
		public virtual void reset()
		{
			this.fightId = 0;
			this.fightType = 0;
			this.fightTeams = new List<FightTeamInformations>();
			this.fightTeamsPositions = new List<uint>();
			this.fightTeamsOptions = new List<FightOptionsInformations>();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_FightCommonInformations(arg1);
		}
		
		public void serializeAs_FightCommonInformations(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.fightId);
			arg1.WriteByte((byte)this.fightType);
			arg1.WriteShort((short)this.fightTeams.Count);
			var loc1 = 0;
			while ( loc1 < this.fightTeams.Count )
			{
				arg1.WriteShort((short)this.fightTeams[loc1].getTypeId());
				this.fightTeams[loc1].serialize(arg1);
				++loc1;
			}
			arg1.WriteShort((short)this.fightTeamsPositions.Count);
			var loc2 = 0;
			while ( loc2 < this.fightTeamsPositions.Count )
			{
				if ( this.fightTeamsPositions[loc2] < 0 || this.fightTeamsPositions[loc2] > 559 )
				{
					throw new Exception("Forbidden value (" + this.fightTeamsPositions[loc2] + ") on element 4 (starting at 1) of fightTeamsPositions.");
				}
				arg1.WriteShort((short)this.fightTeamsPositions[loc2]);
				++loc2;
			}
			arg1.WriteShort((short)this.fightTeamsOptions.Count);
			var loc3 = 0;
			while ( loc3 < this.fightTeamsOptions.Count )
			{
				this.fightTeamsOptions[loc3].serializeAs_FightOptionsInformations(arg1);
				++loc3;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_FightCommonInformations(arg1);
		}
		
		public void deserializeAs_FightCommonInformations(BigEndianReader arg1)
		{
			var loc7 = 0;
			object loc8 = null;
			var loc9 = 0;
			object loc10 = null;
			this.fightId = (int)arg1.ReadInt();
			this.fightType = (uint)arg1.ReadByte();
			if ( this.fightType < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightType + ") on element of FightCommonInformations.fightType.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc7 = (ushort)arg1.ReadUShort();
				(( loc8 = ProtocolTypeManager.GetInstance<FightTeamInformations>((uint)loc7)) as FightTeamInformations).deserialize(arg1);
				this.fightTeams.Add((FightTeamInformations)loc8);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				if ( (loc9 = arg1.ReadShort()) < 0 || loc9 > 559 )
				{
					throw new Exception("Forbidden value (" + loc9 + ") on elements of fightTeamsPositions.");
				}
				this.fightTeamsPositions.Add((uint)loc9);
				++loc4;
			}
			var loc5 = (ushort)arg1.ReadUShort();
			var loc6 = 0;
			while ( loc6 < loc5 )
			{
				((loc10 = new FightOptionsInformations()) as FightOptionsInformations).deserialize(arg1);
				this.fightTeamsOptions.Add((FightOptionsInformations)loc10);
				++loc6;
			}
		}
		
	}
}
