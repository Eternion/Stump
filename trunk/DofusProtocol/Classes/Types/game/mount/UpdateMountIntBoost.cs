using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class UpdateMountIntBoost : UpdateMountBoost
	{
		public const uint protocolId = 357;
		public int value = 0;
		
		public UpdateMountIntBoost()
		{
		}
		
		public UpdateMountIntBoost(int arg1, int arg2)
			: this()
		{
			initUpdateMountIntBoost(arg1, arg2);
		}
		
		public override uint getTypeId()
		{
			return 357;
		}
		
		public UpdateMountIntBoost initUpdateMountIntBoost(int arg1 = 0, int arg2 = 0)
		{
			base.initUpdateMountBoost(arg1);
			this.value = arg2;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.value = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_UpdateMountIntBoost(arg1);
		}
		
		public void serializeAs_UpdateMountIntBoost(BigEndianWriter arg1)
		{
			base.serializeAs_UpdateMountBoost(arg1);
			arg1.WriteInt((int)this.value);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_UpdateMountIntBoost(arg1);
		}
		
		public void deserializeAs_UpdateMountIntBoost(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.value = (int)arg1.ReadInt();
		}
		
	}
}
