using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChallengeTargetsListMessage : Message
	{
		public const uint protocolId = 5613;
		internal Boolean _isInitialized = false;
		public List<int> targetIds;
		public List<int> targetCells;
		
		public ChallengeTargetsListMessage()
		{
			this.targetIds = new List<int>();
			this.targetCells = new List<int>();
		}
		
		public ChallengeTargetsListMessage(List<int> arg1, List<int> arg2)
			: this()
		{
			initChallengeTargetsListMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5613;
		}
		
		public ChallengeTargetsListMessage initChallengeTargetsListMessage(List<int> arg1, List<int> arg2)
		{
			this.targetIds = arg1;
			this.targetCells = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.targetIds = new List<int>();
			this.targetCells = new List<int>();
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
			this.serializeAs_ChallengeTargetsListMessage(arg1);
		}
		
		public void serializeAs_ChallengeTargetsListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.targetIds.Count);
			var loc1 = 0;
			while ( loc1 < this.targetIds.Count )
			{
				arg1.WriteInt((int)this.targetIds[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.targetCells.Count);
			var loc2 = 0;
			while ( loc2 < this.targetCells.Count )
			{
				if ( this.targetCells[loc2] < -1 || this.targetCells[loc2] > 559 )
				{
					throw new Exception("Forbidden value (" + this.targetCells[loc2] + ") on element 2 (starting at 1) of targetCells.");
				}
				arg1.WriteShort((short)this.targetCells[loc2]);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChallengeTargetsListMessage(arg1);
		}
		
		public void deserializeAs_ChallengeTargetsListMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			var loc6 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc5 = arg1.ReadInt();
				this.targetIds.Add((int)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				if ( (loc6 = arg1.ReadShort()) < -1 || loc6 > 559 )
				{
					throw new Exception("Forbidden value (" + loc6 + ") on elements of targetCells.");
				}
				this.targetCells.Add((int)loc6);
				++loc4;
			}
		}
		
	}
}
