using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class JobCrafterDirectoryEntryPlayerInfo : Object
	{
		public const uint protocolId = 194;
		public uint playerId = 0;
		public String playerName = "";
		public int alignmentSide = 0;
		public int breed = 0;
		public Boolean sex = false;
		public Boolean isInWorkshop = false;
		public int worldX = 0;
		public int worldY = 0;
		public uint mapId = 0;
		public uint subareaId = 0;
		
		public JobCrafterDirectoryEntryPlayerInfo()
		{
		}
		
		public JobCrafterDirectoryEntryPlayerInfo(uint arg1, String arg2, int arg3, int arg4, Boolean arg5, Boolean arg6, int arg7, int arg8, uint arg9, uint arg10)
			: this()
		{
			initJobCrafterDirectoryEntryPlayerInfo(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
		}
		
		public virtual uint getTypeId()
		{
			return 194;
		}
		
		public JobCrafterDirectoryEntryPlayerInfo initJobCrafterDirectoryEntryPlayerInfo(uint arg1 = 0, String arg2 = "", int arg3 = 0, int arg4 = 0, Boolean arg5 = false, Boolean arg6 = false, int arg7 = 0, int arg8 = 0, uint arg9 = 0, uint arg10 = 0)
		{
			this.playerId = arg1;
			this.playerName = arg2;
			this.alignmentSide = arg3;
			this.breed = arg4;
			this.sex = arg5;
			this.isInWorkshop = arg6;
			this.worldX = arg7;
			this.worldY = arg8;
			this.mapId = arg9;
			this.subareaId = arg10;
			return this;
		}
		
		public virtual void reset()
		{
			this.playerId = 0;
			this.playerName = "";
			this.alignmentSide = 0;
			this.breed = 0;
			this.sex = false;
			this.isInWorkshop = false;
			this.worldX = 0;
			this.worldY = 0;
			this.mapId = 0;
			this.subareaId = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_JobCrafterDirectoryEntryPlayerInfo(arg1);
		}
		
		public void serializeAs_JobCrafterDirectoryEntryPlayerInfo(BigEndianWriter arg1)
		{
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element playerId.");
			}
			arg1.WriteInt((int)this.playerId);
			arg1.WriteUTF((string)this.playerName);
			arg1.WriteByte((byte)this.alignmentSide);
			arg1.WriteByte((byte)this.breed);
			arg1.WriteBoolean(this.sex);
			arg1.WriteBoolean(this.isInWorkshop);
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element worldX.");
			}
			arg1.WriteShort((short)this.worldX);
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element worldY.");
			}
			arg1.WriteShort((short)this.worldY);
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element mapId.");
			}
			arg1.WriteInt((int)this.mapId);
			if ( this.subareaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subareaId + ") on element subareaId.");
			}
			arg1.WriteShort((short)this.subareaId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobCrafterDirectoryEntryPlayerInfo(arg1);
		}
		
		public void deserializeAs_JobCrafterDirectoryEntryPlayerInfo(BigEndianReader arg1)
		{
			this.playerId = (uint)arg1.ReadInt();
			if ( this.playerId < 0 )
			{
				throw new Exception("Forbidden value (" + this.playerId + ") on element of JobCrafterDirectoryEntryPlayerInfo.playerId.");
			}
			this.playerName = (String)arg1.ReadUTF();
			this.alignmentSide = (int)arg1.ReadByte();
			this.breed = (int)arg1.ReadByte();
            if (this.breed < (int)Stump.DofusProtocol.Enums.BreedEnum.Feca || this.breed > (int)Stump.DofusProtocol.Enums.BreedEnum.Zobal)
			{
				throw new Exception("Forbidden value (" + this.breed + ") on element of JobCrafterDirectoryEntryPlayerInfo.breed.");
			}
			this.sex = (Boolean)arg1.ReadBoolean();
			this.isInWorkshop = (Boolean)arg1.ReadBoolean();
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of JobCrafterDirectoryEntryPlayerInfo.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of JobCrafterDirectoryEntryPlayerInfo.worldY.");
			}
			this.mapId = (uint)arg1.ReadInt();
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element of JobCrafterDirectoryEntryPlayerInfo.mapId.");
			}
			this.subareaId = (uint)arg1.ReadShort();
			if ( this.subareaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subareaId + ") on element of JobCrafterDirectoryEntryPlayerInfo.subareaId.");
			}
		}
		
	}
}
