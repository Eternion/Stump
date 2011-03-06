using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class Achievement : Object
	{
		public const uint protocolId = 363;
		public uint id = 0;
		
		public Achievement()
		{
		}
		
		public Achievement(uint arg1)
			: this()
		{
			initAchievement(arg1);
		}
		
		public virtual uint getTypeId()
		{
			return 363;
		}
		
		public Achievement initAchievement(uint arg1 = 0)
		{
			this.id = arg1;
			return this;
		}
		
		public virtual void reset()
		{
			this.id = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_Achievement(arg1);
		}
		
		public void serializeAs_Achievement(BigEndianWriter arg1)
		{
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element id.");
			}
			arg1.WriteShort((short)this.id);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_Achievement(arg1);
		}
		
		public void deserializeAs_Achievement(BigEndianReader arg1)
		{
			this.id = (uint)arg1.ReadShort();
			if ( this.id < 0 )
			{
				throw new Exception("Forbidden value (" + this.id + ") on element of Achievement.id.");
			}
		}
		
	}
}
