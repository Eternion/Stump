using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobMultiCraftAvailableSkillsMessage : JobAllowMultiCraftRequestMessage
	{
		public const uint protocolId = 5747;
		internal Boolean _isInitialized = false;
		public uint playerId = 0;
		public List<uint> skills;
		
		public JobMultiCraftAvailableSkillsMessage()
		{
			this.skills = new List<uint>();
		}
		
		public JobMultiCraftAvailableSkillsMessage(Boolean arg1, uint arg2, List<uint> arg3)
			: this()
		{
			initJobMultiCraftAvailableSkillsMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5747;
		}
		
		public JobMultiCraftAvailableSkillsMessage initJobMultiCraftAvailableSkillsMessage(Boolean arg1 = false, uint arg2 = 0, List<uint> arg3 = null)
		{
			base.initJobAllowMultiCraftRequestMessage(arg1);
			this.playerId = arg2;
			this.skills = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.playerId = 0;
			this.skills = new List<uint>();
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_JobMultiCraftAvailableSkillsMessage(arg1);
		}
		
		public void serializeAs_JobMultiCraftAvailableSkillsMessage(BigEndianWriter arg1)
		{
			base.serializeAs_JobAllowMultiCraftRequestMessage(arg1);
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element playerId.");
			}
			arg1.WriteInt((int)this.playerId);
			arg1.WriteShort((short)this.skills.Count);
			var loc1 = 0;
			while ( loc1 < this.skills.Count )
			{
				if ( this.skills[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.skills[loc1] + ") on element 2 (starting at 1) of skills.");
				}
				arg1.WriteShort((short)this.skills[loc1]);
				++loc1;
			}
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobMultiCraftAvailableSkillsMessage(arg1);
		}
		
		public void deserializeAs_JobMultiCraftAvailableSkillsMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			base.deserialize(arg1);
			this.playerId = (uint)arg1.ReadInt();
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element of JobMultiCraftAvailableSkillsMessage.playerId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of skills.");
				}
				this.skills.Add((uint)loc3);
				++loc2;
			}
		}
		
	}
}
