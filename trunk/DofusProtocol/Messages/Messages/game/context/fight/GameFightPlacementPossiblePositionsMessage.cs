using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightPlacementPossiblePositionsMessage : Message
	{
		public const uint protocolId = 703;
		internal Boolean _isInitialized = false;
		public List<uint> positionsForChallengers;
		public List<uint> positionsForDefenders;
		public uint teamNumber = 2;
		
		public GameFightPlacementPossiblePositionsMessage()
		{
			this.positionsForChallengers = new List<uint>();
			this.positionsForDefenders = new List<uint>();
		}
		
		public GameFightPlacementPossiblePositionsMessage(List<uint> arg1, List<uint> arg2, uint arg3)
			: this()
		{
			initGameFightPlacementPossiblePositionsMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 703;
		}
		
		public GameFightPlacementPossiblePositionsMessage initGameFightPlacementPossiblePositionsMessage(List<uint> arg1, List<uint> arg2, uint arg3 = 2)
		{
			this.positionsForChallengers = arg1;
			this.positionsForDefenders = arg2;
			this.teamNumber = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.positionsForChallengers = new List<uint>();
			this.positionsForDefenders = new List<uint>();
			this.teamNumber = 2;
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
			this.serializeAs_GameFightPlacementPossiblePositionsMessage(arg1);
		}
		
		public void serializeAs_GameFightPlacementPossiblePositionsMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.positionsForChallengers.Count);
			var loc1 = 0;
			while ( loc1 < this.positionsForChallengers.Count )
			{
				if ( this.positionsForChallengers[loc1] < 0 || this.positionsForChallengers[loc1] > 559 )
				{
					throw new Exception("Forbidden value (" + this.positionsForChallengers[loc1] + ") on element 1 (starting at 1) of positionsForChallengers.");
				}
				arg1.WriteShort((short)this.positionsForChallengers[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.positionsForDefenders.Count);
			var loc2 = 0;
			while ( loc2 < this.positionsForDefenders.Count )
			{
				if ( this.positionsForDefenders[loc2] < 0 || this.positionsForDefenders[loc2] > 559 )
				{
					throw new Exception("Forbidden value (" + this.positionsForDefenders[loc2] + ") on element 2 (starting at 1) of positionsForDefenders.");
				}
				arg1.WriteShort((short)this.positionsForDefenders[loc2]);
				++loc2;
			}
			arg1.WriteByte((byte)this.teamNumber);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightPlacementPossiblePositionsMessage(arg1);
		}
		
		public void deserializeAs_GameFightPlacementPossiblePositionsMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			var loc6 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc5 = arg1.ReadShort()) < 0 || loc5 > 559 )
				{
					throw new Exception("Forbidden value (" + loc5 + ") on elements of positionsForChallengers.");
				}
				this.positionsForChallengers.Add((uint)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				if ( (loc6 = arg1.ReadShort()) < 0 || loc6 > 559 )
				{
					throw new Exception("Forbidden value (" + loc6 + ") on elements of positionsForDefenders.");
				}
				this.positionsForDefenders.Add((uint)loc6);
				++loc4;
			}
			this.teamNumber = (uint)arg1.ReadByte();
			if ( this.teamNumber < 0 )
			{
				throw new Exception("Forbidden value (" + this.teamNumber + ") on element of GameFightPlacementPossiblePositionsMessage.teamNumber.");
			}
		}
		
	}
}
