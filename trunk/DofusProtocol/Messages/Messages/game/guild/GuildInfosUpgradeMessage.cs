using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildInfosUpgradeMessage : Message
	{
		public const uint protocolId = 5636;
		internal Boolean _isInitialized = false;
		public uint maxTaxCollectorsCount = 0;
		public uint taxCollectorsCount = 0;
		public uint taxCollectorLifePoints = 0;
		public uint taxCollectorDamagesBonuses = 0;
		public uint taxCollectorPods = 0;
		public uint taxCollectorProspecting = 0;
		public uint taxCollectorWisdom = 0;
		public uint boostPoints = 0;
		public List<uint> spellId;
		public List<uint> spellLevel;
		
		public GuildInfosUpgradeMessage()
		{
			this.spellId = new List<uint>();
			this.spellLevel = new List<uint>();
		}
		
		public GuildInfosUpgradeMessage(uint arg1, uint arg2, uint arg3, uint arg4, uint arg5, uint arg6, uint arg7, uint arg8, List<uint> arg9, List<uint> arg10)
			: this()
		{
			initGuildInfosUpgradeMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
		}
		
		public override uint getMessageId()
		{
			return 5636;
		}
		
		public GuildInfosUpgradeMessage initGuildInfosUpgradeMessage(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0, uint arg5 = 0, uint arg6 = 0, uint arg7 = 0, uint arg8 = 0, List<uint> arg9 = null, List<uint> arg10 = null)
		{
			this.maxTaxCollectorsCount = arg1;
			this.taxCollectorsCount = arg2;
			this.taxCollectorLifePoints = arg3;
			this.taxCollectorDamagesBonuses = arg4;
			this.taxCollectorPods = arg5;
			this.taxCollectorProspecting = arg6;
			this.taxCollectorWisdom = arg7;
			this.boostPoints = arg8;
			this.spellId = arg9;
			this.spellLevel = arg10;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.maxTaxCollectorsCount = 0;
			this.taxCollectorsCount = 0;
			this.taxCollectorLifePoints = 0;
			this.taxCollectorDamagesBonuses = 0;
			this.taxCollectorPods = 0;
			this.taxCollectorProspecting = 0;
			this.taxCollectorWisdom = 0;
			this.boostPoints = 0;
			this.spellId = new List<uint>();
			this.spellLevel = new List<uint>();
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
			this.serializeAs_GuildInfosUpgradeMessage(arg1);
		}
		
		public void serializeAs_GuildInfosUpgradeMessage(BigEndianWriter arg1)
		{
			if ( this.maxTaxCollectorsCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxTaxCollectorsCount + ") on element maxTaxCollectorsCount.");
			}
			arg1.WriteByte((byte)this.maxTaxCollectorsCount);
			if ( this.taxCollectorsCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorsCount + ") on element taxCollectorsCount.");
			}
			arg1.WriteByte((byte)this.taxCollectorsCount);
			if ( this.taxCollectorLifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorLifePoints + ") on element taxCollectorLifePoints.");
			}
			arg1.WriteShort((short)this.taxCollectorLifePoints);
			if ( this.taxCollectorDamagesBonuses < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorDamagesBonuses + ") on element taxCollectorDamagesBonuses.");
			}
			arg1.WriteShort((short)this.taxCollectorDamagesBonuses);
			if ( this.taxCollectorPods < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorPods + ") on element taxCollectorPods.");
			}
			arg1.WriteShort((short)this.taxCollectorPods);
			if ( this.taxCollectorProspecting < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorProspecting + ") on element taxCollectorProspecting.");
			}
			arg1.WriteShort((short)this.taxCollectorProspecting);
			if ( this.taxCollectorWisdom < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorWisdom + ") on element taxCollectorWisdom.");
			}
			arg1.WriteShort((short)this.taxCollectorWisdom);
			if ( this.boostPoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.boostPoints + ") on element boostPoints.");
			}
			arg1.WriteShort((short)this.boostPoints);
			arg1.WriteShort((short)this.spellId.Count);
			var loc1 = 0;
			while ( loc1 < this.spellId.Count )
			{
				if ( this.spellId[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.spellId[loc1] + ") on element 9 (starting at 1) of spellId.");
				}
				arg1.WriteShort((short)this.spellId[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.spellLevel.Count);
			var loc2 = 0;
			while ( loc2 < this.spellLevel.Count )
			{
				if ( this.spellLevel[loc2] < 0 )
				{
					throw new Exception("Forbidden value (" + this.spellLevel[loc2] + ") on element 10 (starting at 1) of spellLevel.");
				}
				arg1.WriteByte((byte)this.spellLevel[loc2]);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildInfosUpgradeMessage(arg1);
		}
		
		public void deserializeAs_GuildInfosUpgradeMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			var loc6 = 0;
			this.maxTaxCollectorsCount = (uint)arg1.ReadByte();
			if ( this.maxTaxCollectorsCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxTaxCollectorsCount + ") on element of GuildInfosUpgradeMessage.maxTaxCollectorsCount.");
			}
			this.taxCollectorsCount = (uint)arg1.ReadByte();
			if ( this.taxCollectorsCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorsCount + ") on element of GuildInfosUpgradeMessage.taxCollectorsCount.");
			}
			this.taxCollectorLifePoints = (uint)arg1.ReadShort();
			if ( this.taxCollectorLifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorLifePoints + ") on element of GuildInfosUpgradeMessage.taxCollectorLifePoints.");
			}
			this.taxCollectorDamagesBonuses = (uint)arg1.ReadShort();
			if ( this.taxCollectorDamagesBonuses < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorDamagesBonuses + ") on element of GuildInfosUpgradeMessage.taxCollectorDamagesBonuses.");
			}
			this.taxCollectorPods = (uint)arg1.ReadShort();
			if ( this.taxCollectorPods < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorPods + ") on element of GuildInfosUpgradeMessage.taxCollectorPods.");
			}
			this.taxCollectorProspecting = (uint)arg1.ReadShort();
			if ( this.taxCollectorProspecting < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorProspecting + ") on element of GuildInfosUpgradeMessage.taxCollectorProspecting.");
			}
			this.taxCollectorWisdom = (uint)arg1.ReadShort();
			if ( this.taxCollectorWisdom < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorWisdom + ") on element of GuildInfosUpgradeMessage.taxCollectorWisdom.");
			}
			this.boostPoints = (uint)arg1.ReadShort();
			if ( this.boostPoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.boostPoints + ") on element of GuildInfosUpgradeMessage.boostPoints.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc5 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc5 + ") on elements of spellId.");
				}
				this.spellId.Add((uint)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				if ( (loc6 = arg1.ReadByte()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc6 + ") on elements of spellLevel.");
				}
				this.spellLevel.Add((uint)loc6);
				++loc4;
			}
		}
		
	}
}
