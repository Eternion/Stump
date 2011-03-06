using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class Item : Object
	{
		public const uint protocolId = 7;
		
		public Item()
		{
		}
		
		public virtual uint getTypeId()
		{
			return 7;
		}
		
		public Item initItem()
		{
			return this;
		}
		
		public virtual void reset()
		{
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
		}
		
		public void serializeAs_Item(BigEndianWriter arg1)
		{
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
		}
		
		public void deserializeAs_Item(BigEndianReader arg1)
		{
		}
		
	}
}
