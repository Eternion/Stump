using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ObjectItemMinimalInformation : Item
	{
		public const uint protocolId = 124;
		public uint objectGID = 0;
		public int powerRate = 0;
		public Boolean overMax = false;
		public List<ObjectEffect> effects;
		
		public ObjectItemMinimalInformation()
		{
			this.effects = new List<ObjectEffect>();
		}
		
		public ObjectItemMinimalInformation(uint arg1, int arg2, Boolean arg3, List<ObjectEffect> arg4)
			: this()
		{
			initObjectItemMinimalInformation(arg1, arg2, arg3, arg4);
		}
		
		public override uint getTypeId()
		{
			return 124;
		}
		
		public ObjectItemMinimalInformation initObjectItemMinimalInformation(uint arg1 = 0, int arg2 = 0, Boolean arg3 = false, List<ObjectEffect> arg4 = null)
		{
			this.@objectGID = arg1;
			this.powerRate = arg2;
			this.overMax = arg3;
			this.effects = arg4;
			return this;
		}
		
		public override void reset()
		{
			this.@objectGID = 0;
			this.powerRate = 0;
			this.overMax = false;
			this.effects = new List<ObjectEffect>();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectItemMinimalInformation(arg1);
		}
		
		public void serializeAs_ObjectItemMinimalInformation(BigEndianWriter arg1)
		{
			base.serializeAs_Item(arg1);
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
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectItemMinimalInformation(arg1);
		}
		
		public void deserializeAs_ObjectItemMinimalInformation(BigEndianReader arg1)
		{
			var loc3 = 0;
			object loc4 = null;
			base.deserialize(arg1);
			this.@objectGID = (uint)arg1.ReadShort();
			if ( this.@objectGID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectGID + ") on element of ObjectItemMinimalInformation.objectGID.");
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
		}
		
	}
}
