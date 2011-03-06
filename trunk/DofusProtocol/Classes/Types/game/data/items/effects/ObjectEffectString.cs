using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ObjectEffectString : ObjectEffect
	{
		public const uint protocolId = 74;
		public String value = "";
		
		public ObjectEffectString()
		{
		}
		
		public ObjectEffectString(uint arg1, String arg2)
			: this()
		{
			initObjectEffectString(arg1, arg2);
		}
		
		public override uint getTypeId()
		{
			return 74;
		}
		
		public ObjectEffectString initObjectEffectString(uint arg1 = 0, String arg2 = "")
		{
			base.initObjectEffect(arg1);
			this.value = arg2;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.value = "";
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ObjectEffectString(arg1);
		}
		
		public void serializeAs_ObjectEffectString(BigEndianWriter arg1)
		{
			base.serializeAs_ObjectEffect(arg1);
			arg1.WriteUTF((string)this.value);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectEffectString(arg1);
		}
		
		public void deserializeAs_ObjectEffectString(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.value = (String)arg1.ReadUTF();
		}
		
	}
}
