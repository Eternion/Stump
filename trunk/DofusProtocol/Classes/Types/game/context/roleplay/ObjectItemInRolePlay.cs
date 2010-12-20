using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ObjectItemInRolePlay : Object
	{
		public const uint protocolId = 198;
		public uint cellId = 0;
		public uint objectGID = 0;
		
		public ObjectItemInRolePlay()
		{
		}
		
		public ObjectItemInRolePlay(uint arg1, uint arg2)
			: this()
		{
			initObjectItemInRolePlay(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 198;
		}
		
		public ObjectItemInRolePlay initObjectItemInRolePlay(uint arg1 = 0, uint arg2 = 0)
		{
			this.cellId = arg1;
			this.@objectGID = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.cellId = 0;
			this.@objectGID = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectItemInRolePlay(arg1);
		}
		
		public void serializeAs_ObjectItemInRolePlay(BigEndianWriter arg1)
		{
			if ( this.cellId < 0 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element cellId.");
			}
			arg1.WriteShort((short)this.cellId);
			if ( this.@objectGID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectGID + ") on element objectGID.");
			}
			arg1.WriteShort((short)this.@objectGID);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectItemInRolePlay(arg1);
		}
		
		public void deserializeAs_ObjectItemInRolePlay(BigEndianReader arg1)
		{
			this.cellId = (uint)arg1.ReadShort();
			if ( this.cellId < 0 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element of ObjectItemInRolePlay.cellId.");
			}
			this.@objectGID = (uint)arg1.ReadShort();
			if ( this.@objectGID < 0 )
			{
				throw new Exception("Forbidden value (" + this.@objectGID + ") on element of ObjectItemInRolePlay.objectGID.");
			}
		}
		
	}
}
