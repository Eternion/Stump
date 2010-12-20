using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameActionMark : Object
	{
		public const uint protocolId = 351;
		public int markAuthorId = 0;
		public uint markSpellId = 0;
		public int markId = 0;
		public int markType = 0;
		public List<GameActionMarkedCell> cells;
		
		public GameActionMark()
		{
			this.cells = new List<GameActionMarkedCell>();
		}
		
		public GameActionMark(int arg1, uint arg2, int arg3, int arg4, List<GameActionMarkedCell> arg5)
			: this()
		{
			initGameActionMark(arg1, arg2, arg3, arg4, arg5);
		}
		
		public virtual uint getTypeId()
		{
			return 351;
		}
		
		public GameActionMark initGameActionMark(int arg1 = 0, uint arg2 = 0, int arg3 = 0, int arg4 = 0, List<GameActionMarkedCell> arg5 = null)
		{
			this.markAuthorId = arg1;
			this.markSpellId = arg2;
			this.markId = arg3;
			this.markType = arg4;
			this.cells = arg5;
			return this;
		}
		
		public virtual void reset()
		{
			this.markAuthorId = 0;
			this.markSpellId = 0;
			this.markId = 0;
			this.markType = 0;
			this.cells = new List<GameActionMarkedCell>();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameActionMark(arg1);
		}
		
		public void serializeAs_GameActionMark(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.markAuthorId);
			if ( this.markSpellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.markSpellId + ") on element markSpellId.");
			}
			arg1.WriteInt((int)this.markSpellId);
			arg1.WriteShort((short)this.markId);
			arg1.WriteByte((byte)this.markType);
			arg1.WriteShort((short)this.cells.Count);
			var loc1 = 0;
			while ( loc1 < this.cells.Count )
			{
				this.cells[loc1].serializeAs_GameActionMarkedCell(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionMark(arg1);
		}
		
		public void deserializeAs_GameActionMark(BigEndianReader arg1)
		{
			object loc3 = null;
			this.markAuthorId = (int)arg1.ReadInt();
			this.markSpellId = (uint)arg1.ReadInt();
			if ( this.markSpellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.markSpellId + ") on element of GameActionMark.markSpellId.");
			}
			this.markId = (int)arg1.ReadShort();
			this.markType = (int)arg1.ReadByte();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new GameActionMarkedCell()) as GameActionMarkedCell).deserialize(arg1);
				this.cells.Add((GameActionMarkedCell)loc3);
				++loc2;
			}
		}
		
	}
}
