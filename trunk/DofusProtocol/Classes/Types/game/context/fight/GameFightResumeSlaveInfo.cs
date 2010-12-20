using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameFightResumeSlaveInfo : Object
	{
		public const uint protocolId = 364;
		public int slaveId = 0;
		public List<GameFightSpellCooldown> spellCooldowns;
		public uint summonCount = 0;
		public uint bombCount = 0;
		
		public GameFightResumeSlaveInfo()
		{
			this.spellCooldowns = new List<GameFightSpellCooldown>();
		}
		
		public GameFightResumeSlaveInfo(int arg1, List<GameFightSpellCooldown> arg2, uint arg3, uint arg4)
			: this()
		{
			initGameFightResumeSlaveInfo(arg1, arg2, arg3, arg4);
		}
		
		public virtual uint getTypeId()
		{
			return 364;
		}
		
		public GameFightResumeSlaveInfo initGameFightResumeSlaveInfo(int arg1 = 0, List<GameFightSpellCooldown> arg2 = null, uint arg3 = 0, uint arg4 = 0)
		{
			this.slaveId = arg1;
			this.spellCooldowns = arg2;
			this.summonCount = arg3;
			this.bombCount = arg4;
			return this;
		}
		
		public virtual void reset()
		{
			this.slaveId = 0;
			this.spellCooldowns = new List<GameFightSpellCooldown>();
			this.summonCount = 0;
			this.bombCount = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightResumeSlaveInfo(arg1);
		}
		
		public void serializeAs_GameFightResumeSlaveInfo(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.slaveId);
			arg1.WriteShort((short)this.spellCooldowns.Count);
			var loc1 = 0;
			while ( loc1 < this.spellCooldowns.Count )
			{
				this.spellCooldowns[loc1].serializeAs_GameFightSpellCooldown(arg1);
				++loc1;
			}
			if ( this.summonCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.summonCount + ") on element summonCount.");
			}
			arg1.WriteByte((byte)this.summonCount);
			if ( this.bombCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.bombCount + ") on element bombCount.");
			}
			arg1.WriteByte((byte)this.bombCount);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightResumeSlaveInfo(arg1);
		}
		
		public void deserializeAs_GameFightResumeSlaveInfo(BigEndianReader arg1)
		{
			object loc3 = null;
			this.slaveId = (int)arg1.ReadInt();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new GameFightSpellCooldown()) as GameFightSpellCooldown).deserialize(arg1);
				this.spellCooldowns.Add((GameFightSpellCooldown)loc3);
				++loc2;
			}
			this.summonCount = (uint)arg1.ReadByte();
			if ( this.summonCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.summonCount + ") on element of GameFightResumeSlaveInfo.summonCount.");
			}
			this.bombCount = (uint)arg1.ReadByte();
			if ( this.bombCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.bombCount + ") on element of GameFightResumeSlaveInfo.bombCount.");
			}
		}
		
	}
}
