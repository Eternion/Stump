using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameMapMovementRequestMessage : Message
	{
		public const uint protocolId = 950;
		internal Boolean _isInitialized = false;
		public List<uint> keyMovements;
		public uint mapId = 0;
		
		public GameMapMovementRequestMessage()
		{
			this.keyMovements = new List<uint>();
		}
		
		public GameMapMovementRequestMessage(List<uint> arg1, uint arg2)
			: this()
		{
			initGameMapMovementRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 950;
		}
		
		public GameMapMovementRequestMessage initGameMapMovementRequestMessage(List<uint> arg1, uint arg2 = 0)
		{
			this.keyMovements = arg1;
			this.mapId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.keyMovements = new List<uint>();
			this.mapId = 0;
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
			this.serializeAs_GameMapMovementRequestMessage(arg1);
		}
		
		public void serializeAs_GameMapMovementRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.keyMovements.Count);
			var loc1 = 0;
			while ( loc1 < this.keyMovements.Count )
			{
				if ( this.keyMovements[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.keyMovements[loc1] + ") on element 1 (starting at 1) of keyMovements.");
				}
				arg1.WriteShort((short)this.keyMovements[loc1]);
				++loc1;
			}
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element mapId.");
			}
			arg1.WriteInt((int)this.mapId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameMapMovementRequestMessage(arg1);
		}
		
		public void deserializeAs_GameMapMovementRequestMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of keyMovements.");
				}
				this.keyMovements.Add((uint)loc3);
				++loc2;
			}
			this.mapId = (uint)arg1.ReadInt();
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element of GameMapMovementRequestMessage.mapId.");
			}
		}
		
	}
}
