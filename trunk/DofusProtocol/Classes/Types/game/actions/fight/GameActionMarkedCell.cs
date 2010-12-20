using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameActionMarkedCell : Object
	{
		public const uint protocolId = 85;
		public uint cellId = 0;
		public int zoneSize = 0;
		public int cellColor = 0;
		
		public GameActionMarkedCell()
		{
		}
		
		public GameActionMarkedCell(uint arg1, int arg2, int arg3)
			: this()
		{
			initGameActionMarkedCell(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 85;
		}
		
		public GameActionMarkedCell initGameActionMarkedCell(uint arg1 = 0, int arg2 = 0, int arg3 = 0)
		{
			this.cellId = arg1;
			this.zoneSize = arg2;
			this.cellColor = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.cellId = 0;
			this.zoneSize = 0;
			this.cellColor = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameActionMarkedCell(arg1);
		}
		
		public void serializeAs_GameActionMarkedCell(BigEndianWriter arg1)
		{
			if ( this.cellId < 0 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element cellId.");
			}
			arg1.WriteShort((short)this.cellId);
			arg1.WriteByte((byte)this.zoneSize);
			arg1.WriteInt((int)this.cellColor);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameActionMarkedCell(arg1);
		}
		
		public void deserializeAs_GameActionMarkedCell(BigEndianReader arg1)
		{
			this.cellId = (uint)arg1.ReadShort();
			if ( this.cellId < 0 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element of GameActionMarkedCell.cellId.");
			}
			this.zoneSize = (int)arg1.ReadByte();
			this.cellColor = (int)arg1.ReadInt();
		}
		
	}
}
