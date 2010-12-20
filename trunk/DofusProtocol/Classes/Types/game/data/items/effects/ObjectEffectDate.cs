using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ObjectEffectDate : ObjectEffect
	{
		public const uint protocolId = 72;
		public uint year = 0;
		public uint month = 0;
		public uint day = 0;
		public uint hour = 0;
		public uint minute = 0;
		
		public ObjectEffectDate()
		{
		}
		
		public ObjectEffectDate(uint arg1, uint arg2, uint arg3, uint arg4, uint arg5, uint arg6)
			: this()
		{
			initObjectEffectDate(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getTypeId()
		{
			return 72;
		}
		
		public ObjectEffectDate initObjectEffectDate(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0, uint arg5 = 0, uint arg6 = 0)
		{
			base.initObjectEffect(arg1);
			this.year = arg2;
			this.month = arg3;
			this.day = arg4;
			this.hour = arg5;
			this.minute = arg6;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.year = 0;
			this.month = 0;
			this.day = 0;
			this.hour = 0;
			this.minute = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectEffectDate(arg1);
		}
		
		public void serializeAs_ObjectEffectDate(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectEffect(arg1);
			if ( this.year < 0 )
			{
				throw new Exception("Forbidden value (" + this.year + ") on element year.");
			}
			arg1.WriteShort((short)this.year);
			if ( this.month < 0 )
			{
				throw new Exception("Forbidden value (" + this.month + ") on element month.");
			}
			arg1.WriteShort((short)this.month);
			if ( this.day < 0 )
			{
				throw new Exception("Forbidden value (" + this.day + ") on element day.");
			}
			arg1.WriteShort((short)this.day);
			if ( this.hour < 0 )
			{
				throw new Exception("Forbidden value (" + this.hour + ") on element hour.");
			}
			arg1.WriteShort((short)this.hour);
			if ( this.minute < 0 )
			{
				throw new Exception("Forbidden value (" + this.minute + ") on element minute.");
			}
			arg1.WriteShort((short)this.minute);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectEffectDate(arg1);
		}
		
		public void deserializeAs_ObjectEffectDate(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.year = (uint)arg1.ReadShort();
			if ( this.year < 0 )
			{
				throw new Exception("Forbidden value (" + this.year + ") on element of ObjectEffectDate.year.");
			}
			this.month = (uint)arg1.ReadShort();
			if ( this.month < 0 )
			{
				throw new Exception("Forbidden value (" + this.month + ") on element of ObjectEffectDate.month.");
			}
			this.day = (uint)arg1.ReadShort();
			if ( this.day < 0 )
			{
				throw new Exception("Forbidden value (" + this.day + ") on element of ObjectEffectDate.day.");
			}
			this.hour = (uint)arg1.ReadShort();
			if ( this.hour < 0 )
			{
				throw new Exception("Forbidden value (" + this.hour + ") on element of ObjectEffectDate.hour.");
			}
			this.minute = (uint)arg1.ReadShort();
			if ( this.minute < 0 )
			{
				throw new Exception("Forbidden value (" + this.minute + ") on element of ObjectEffectDate.minute.");
			}
		}
		
	}
}
