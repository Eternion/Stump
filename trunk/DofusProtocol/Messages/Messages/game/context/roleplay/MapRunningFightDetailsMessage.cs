using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class MapRunningFightDetailsMessage : Message
	{
		public const uint protocolId = 5751;
		internal Boolean _isInitialized = false;
		public uint fightId = 0;
		public List<String> names;
		public List<uint> levels;
		public uint teamSwap = 0;
		public List<Boolean> alives;
		
		public MapRunningFightDetailsMessage()
		{
			this.names = new List<String>();
			this.levels = new List<uint>();
			this.alives = new List<Boolean>();
		}
		
		public MapRunningFightDetailsMessage(uint arg1, List<String> arg2, List<uint> arg3, uint arg4, List<Boolean> arg5)
			: this()
		{
			initMapRunningFightDetailsMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 5751;
		}
		
		public MapRunningFightDetailsMessage initMapRunningFightDetailsMessage(uint arg1 = 0, List<String> arg2 = null, List<uint> arg3 = null, uint arg4 = 0, List<Boolean> arg5 = null)
		{
			this.fightId = arg1;
			this.names = arg2;
			this.levels = arg3;
			this.teamSwap = arg4;
			this.alives = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.fightId = 0;
			this.names = new List<String>();
			this.levels = new List<uint>();
			this.teamSwap = 0;
			this.alives = new List<Boolean>();
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
			this.serializeAs_MapRunningFightDetailsMessage(arg1);
		}
		
		public void serializeAs_MapRunningFightDetailsMessage(BigEndianWriter arg1)
		{
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element fightId.");
			}
			arg1.WriteInt((int)this.fightId);
			arg1.WriteShort((short)this.names.Count);
			var loc1 = 0;
			while ( loc1 < this.names.Count )
			{
				arg1.WriteUTF((string)this.names[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.levels.Count);
			var loc2 = 0;
			while ( loc2 < this.levels.Count )
			{
				if ( this.levels[loc2] < 0 )
				{
					throw new Exception("Forbidden value (" + this.levels[loc2] + ") on element 3 (starting at 1) of levels.");
				}
				arg1.WriteShort((short)this.levels[loc2]);
				++loc2;
			}
			if ( this.teamSwap < 0 )
			{
				throw new Exception("Forbidden value (" + this.teamSwap + ") on element teamSwap.");
			}
			arg1.WriteByte((byte)this.teamSwap);
			arg1.WriteShort((short)this.alives.Count);
			var loc3 = 0;
			while ( loc3 < this.alives.Count )
			{
				arg1.WriteBoolean(this.alives[loc3]);
				++loc3;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_MapRunningFightDetailsMessage(arg1);
		}
		
		public void deserializeAs_MapRunningFightDetailsMessage(BigEndianReader arg1)
		{
			object loc7 = null;
			var loc8 = 0;
			var loc9 = false;
			this.fightId = (uint)arg1.ReadInt();
			if ( this.fightId < 0 )
			{
				throw new Exception("Forbidden value (" + this.fightId + ") on element of MapRunningFightDetailsMessage.fightId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc7 = arg1.ReadUTF();
				this.names.Add((String)loc7);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				if ( (loc8 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc8 + ") on elements of levels.");
				}
				this.levels.Add((uint)loc8);
				++loc4;
			}
			this.teamSwap = (uint)arg1.ReadByte();
			if ( this.teamSwap < 0 )
			{
				throw new Exception("Forbidden value (" + this.teamSwap + ") on element of MapRunningFightDetailsMessage.teamSwap.");
			}
			var loc5 = (ushort)arg1.ReadUShort();
			var loc6 = 0;
			while ( loc6 < loc5 )
			{
				loc9 = arg1.ReadBoolean();
				this.alives.Add((Boolean)loc9);
				++loc6;
			}
		}
		
	}
}
