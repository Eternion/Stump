using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class AchievementListMessage : Message
	{
		public const uint protocolId = 6205;
		internal Boolean _isInitialized = false;
		public List<Achievement> startedAchievements;
		public List<uint> finishedAchievementsIds;
		
		public AchievementListMessage()
		{
			this.startedAchievements = new List<Achievement>();
			this.finishedAchievementsIds = new List<uint>();
		}
		
		public AchievementListMessage(List<Achievement> arg1, List<uint> arg2)
			: this()
		{
			initAchievementListMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6205;
		}
		
		public AchievementListMessage initAchievementListMessage(List<Achievement> arg1, List<uint> arg2)
		{
			this.startedAchievements = arg1;
			this.finishedAchievementsIds = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.startedAchievements = new List<Achievement>();
			this.finishedAchievementsIds = new List<uint>();
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
			this.serializeAs_AchievementListMessage(arg1);
		}
		
		public void serializeAs_AchievementListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.startedAchievements.Count);
			var loc1 = 0;
			while ( loc1 < this.startedAchievements.Count )
			{
				arg1.WriteShort((short)this.startedAchievements[loc1].getTypeId());
				this.startedAchievements[loc1].serialize(arg1);
				++loc1;
			}
			arg1.WriteShort((short)this.finishedAchievementsIds.Count);
			var loc2 = 0;
			while ( loc2 < this.finishedAchievementsIds.Count )
			{
				if ( this.finishedAchievementsIds[loc2] < 0 )
				{
					throw new Exception("Forbidden value (" + this.finishedAchievementsIds[loc2] + ") on element 2 (starting at 1) of finishedAchievementsIds.");
				}
				arg1.WriteShort((short)this.finishedAchievementsIds[loc2]);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AchievementListMessage(arg1);
		}
		
		public void deserializeAs_AchievementListMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			object loc6 = null;
			var loc7 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc5 = (ushort)arg1.ReadUShort();
				(( loc6 = ProtocolTypeManager.GetInstance<Achievement>((uint)loc5)) as Achievement).deserialize(arg1);
				this.startedAchievements.Add((Achievement)loc6);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				if ( (loc7 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc7 + ") on elements of finishedAchievementsIds.");
				}
				this.finishedAchievementsIds.Add((uint)loc7);
				++loc4;
			}
		}
		
	}
}
