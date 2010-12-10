// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class PaddockInformationsForSell : Object
	{
		public const uint protocolId = 222;
		public String guildOwner = "";
		public int worldX = 0;
		public int worldY = 0;
		public uint subAreaId = 0;
		public int nbMount = 0;
		public int nbObject = 0;
		public uint price = 0;
		
		public PaddockInformationsForSell()
		{
		}
		
		public PaddockInformationsForSell(String arg1, int arg2, int arg3, uint arg4, int arg5, int arg6, uint arg7)
			: this()
		{
			initPaddockInformationsForSell(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public virtual uint getTypeId()
		{
			return 222;
		}
		
		public PaddockInformationsForSell initPaddockInformationsForSell(String arg1 = "", int arg2 = 0, int arg3 = 0, uint arg4 = 0, int arg5 = 0, int arg6 = 0, uint arg7 = 0)
		{
			this.guildOwner = arg1;
			this.worldX = arg2;
			this.worldY = arg3;
			this.subAreaId = arg4;
			this.nbMount = arg5;
			this.nbObject = arg6;
			this.price = arg7;
			return this;
		}
		
		public virtual void reset()
		{
			this.guildOwner = "";
			this.worldX = 0;
			this.worldY = 0;
			this.subAreaId = 0;
			this.nbMount = 0;
			this.nbObject = 0;
			this.price = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PaddockInformationsForSell(arg1);
		}
		
		public void serializeAs_PaddockInformationsForSell(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.guildOwner);
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
			if ( this.subAreaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subAreaId + ") on element subAreaId.");
			}
			arg1.WriteShort((short)this.subAreaId);
			arg1.WriteByte((byte)this.nbMount);
			arg1.WriteByte((byte)this.nbObject);
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element price.");
			}
			arg1.WriteInt((int)this.price);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PaddockInformationsForSell(arg1);
		}
		
		public void deserializeAs_PaddockInformationsForSell(BigEndianReader arg1)
		{
			this.guildOwner = (String)arg1.ReadUTF();
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of PaddockInformationsForSell.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of PaddockInformationsForSell.worldY.");
			}
			this.subAreaId = (uint)arg1.ReadShort();
			if ( this.subAreaId < 0 )
			{
				throw new Exception("Forbidden value (" + this.subAreaId + ") on element of PaddockInformationsForSell.subAreaId.");
			}
			this.nbMount = (int)arg1.ReadByte();
			this.nbObject = (int)arg1.ReadByte();
			this.price = (uint)arg1.ReadInt();
			if ( this.price < 0 )
			{
				throw new Exception("Forbidden value (" + this.price + ") on element of PaddockInformationsForSell.price.");
			}
		}
		
	}
}
