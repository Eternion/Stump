using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class SlaveSwitchContextMessage : Message
	{
		public const uint protocolId = 6214;
		internal Boolean _isInitialized = false;
		public int summonerId = 0;
		public int slaveId = 0;
		public List<SpellItem> slaveSpells;
		public CharacterCharacteristicsInformations slaveStats;
		
		public SlaveSwitchContextMessage()
		{
			this.slaveSpells = new List<SpellItem>();
			this.slaveStats = new CharacterCharacteristicsInformations();
		}
		
		public SlaveSwitchContextMessage(int arg1, int arg2, List<SpellItem> arg3, CharacterCharacteristicsInformations arg4)
			: this()
		{
			initSlaveSwitchContextMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6214;
		}
		
		public SlaveSwitchContextMessage initSlaveSwitchContextMessage(int arg1 = 0, int arg2 = 0, List<SpellItem> arg3 = null, CharacterCharacteristicsInformations arg4 = null)
		{
			this.summonerId = arg1;
			this.slaveId = arg2;
			this.slaveSpells = arg3;
			this.slaveStats = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.summonerId = 0;
			this.slaveId = 0;
			this.slaveSpells = new List<SpellItem>();
			this.slaveStats = new CharacterCharacteristicsInformations();
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
			this.serializeAs_SlaveSwitchContextMessage(arg1);
		}
		
		public void serializeAs_SlaveSwitchContextMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.summonerId);
			arg1.WriteInt((int)this.slaveId);
			arg1.WriteShort((short)this.slaveSpells.Count);
			var loc1 = 0;
			while ( loc1 < this.slaveSpells.Count )
			{
				this.slaveSpells[loc1].serializeAs_SpellItem(arg1);
				++loc1;
			}
			this.slaveStats.serializeAs_CharacterCharacteristicsInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SlaveSwitchContextMessage(arg1);
		}
		
		public void deserializeAs_SlaveSwitchContextMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.summonerId = (int)arg1.ReadInt();
			this.slaveId = (int)arg1.ReadInt();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new SpellItem()) as SpellItem).deserialize(arg1);
				this.slaveSpells.Add((SpellItem)loc3);
				++loc2;
			}
			this.slaveStats = new CharacterCharacteristicsInformations();
			this.slaveStats.deserialize(arg1);
		}
		
	}
}
