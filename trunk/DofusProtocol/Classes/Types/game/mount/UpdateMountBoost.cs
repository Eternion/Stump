using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class UpdateMountBoost : Object
	{
		public const uint protocolId = 356;
		public int type = 0;
		
		public UpdateMountBoost()
		{
		}
		
		public UpdateMountBoost(int arg1)
			: this()
		{
			initUpdateMountBoost(arg1);
		}
		
		public virtual uint getTypeId()
		{
			return 356;
		}
		
		public UpdateMountBoost initUpdateMountBoost(int arg1 = 0)
		{
			this.type = arg1;
			return this;
		}
		
		public virtual void reset()
		{
			this.type = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_UpdateMountBoost(arg1);
		}
		
		public void serializeAs_UpdateMountBoost(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.type);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_UpdateMountBoost(arg1);
		}
		
		public void deserializeAs_UpdateMountBoost(BigEndianReader arg1)
		{
			this.type = (int)arg1.ReadByte();
		}
		
	}
}
