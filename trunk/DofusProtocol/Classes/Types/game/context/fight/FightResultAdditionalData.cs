using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class FightResultAdditionalData : Object
	{
		public const uint protocolId = 191;
		
		public FightResultAdditionalData()
		{
		}
		
		public virtual uint getTypeId()
		{
			return 191;
		}
		
		public FightResultAdditionalData initFightResultAdditionalData()
		{
			return this;
		}
		
		public virtual void reset()
		{
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
		}
		
		public void serializeAs_FightResultAdditionalData(BigEndianWriter arg1)
		{
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
		}
		
		public void deserializeAs_FightResultAdditionalData(BigEndianReader arg1)
		{
		}
		
	}
}
