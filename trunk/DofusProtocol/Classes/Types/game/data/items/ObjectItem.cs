using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ObjectItem : Item
	{
		public const uint protocolId = 37;
		public uint position = 63;
		public uint objectGID = 0;
		public int powerRate = 0;
		public Boolean overMax = false;
		public List<ObjectEffect> effects;
		public uint objectUID = 0;
		public uint quantity = 0;
		
		public ObjectItem()
		{
			this.effects = new List<ObjectEffect>();
		}
		
		public ObjectItem(uint arg1, uint arg2, int arg3, Boolean arg4, List<ObjectEffect> arg5, uint arg6, uint arg7)
			: this()
		{
			initObjectItem(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getTypeId()
		{
			return 37;
		}
		
		public ObjectItem initObjectItem(uint arg1 = 63, uint arg2 = 0, int arg3 = 0, Boolean arg4 = false, List<ObjectEffect> arg5 = null, uint arg6 = 0, uint arg7 = 0)
		{
			this.position = arg1;
			this.@objectGID = arg2;
			this.powerRate = arg3;
			this.overMax = arg4;
			this.effects = arg5;
			this.@objectUID = arg6;
			this.quantity = arg7;
			return this;
		}
		
		public override void reset()
		{
			this.position = 63;
			this.@objectGID = 0;
			this.powerRate = 0;
			this.overMax = false;
			this.effects = new List<ObjectEffect>();
			this.@objectUID = 0;
			this.quantity = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectItem(arg1);
		}
		
		public void serializeAs_ObjectItem(BigEndianWriter arg1)
		{
			base.serializeAs_Item(arg1);
			arg1.WriteByte((byte)this.position);
			if ( this.@objectGID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectGID + ") on element objectGID.");
			}
			arg1.WriteShort((short)this.@objectGID);
			arg1.WriteShort((short)this.powerRate);
			arg1.WriteBoolean(this.overMax);
			arg1.WriteShort((short)this.effects.Count);
			var loc1 = 0;
			while ( loc1 < this.effects.Count )
			{
				arg1.WriteShort((short)this.effects[loc1].getTypeId());
				this.effects[loc1].serialize(arg1);
				++loc1;
			}
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element objectUID.");
			}
			arg1.WriteInt((int)this.@objectUID);
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element quantity.");
			}
			arg1.WriteInt((int)this.quantity);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectItem(arg1);
		}
		
		public void deserializeAs_ObjectItem(BigEndianReader arg1)
		{
			var loc3 = 0;
			object loc4 = null;
			base.deserialize(arg1);
			this.position = (uint)arg1.ReadByte();
			if ( this.position < 0 || this.position > 255 )
			{
				throw new Exception("Forbidden value (" + this.position + ") on element of ObjectItem.position.");
			}
			this.@objectGID = (uint)arg1.ReadShort();
			if ( this.@objectGID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectGID + ") on element of ObjectItem.objectGID.");
			}
			this.powerRate = (int)arg1.ReadShort();
			this.overMax = (Boolean)arg1.ReadBoolean();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = (ushort)arg1.ReadUShort();
				(( loc4 = ProtocolTypeManager.GetInstance<ObjectEffect>((uint)loc3)) as ObjectEffect).deserialize(arg1);
				this.effects.Add((ObjectEffect)loc4);
				++loc2;
			}
			this.@objectUID = (uint)arg1.ReadInt();
			if ( this.@objectUID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectUID + ") on element of ObjectItem.objectUID.");
			}
			this.quantity = (uint)arg1.ReadInt();
			if ( this.quantity < 0 )
			{
				throw new Exception("Forbidden value (" + this.quantity + ") on element of ObjectItem.quantity.");
			}
		}
		
	}
}
