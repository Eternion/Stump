using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ContentPart : Object
	{
		public const uint protocolId = 350;
		public String id = "";
		public uint state = 0;
		
		public ContentPart()
		{
		}
		
		public ContentPart(String arg1, uint arg2)
			: this()
		{
			initContentPart(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 350;
		}
		
		public ContentPart initContentPart(String arg1 = "", uint arg2 = 0)
		{
			this.id = arg1;
			this.state = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.id = "";
			this.state = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ContentPart(arg1);
		}
		
		public void serializeAs_ContentPart(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.id);
			arg1.WriteByte((byte)this.state);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ContentPart(arg1);
		}
		
		public void deserializeAs_ContentPart(BigEndianReader arg1)
		{
			this.id = (String)arg1.ReadUTF();
			this.state = (uint)arg1.ReadByte();
			if ( this.state < 0 )
			{
				throw new Exception("Forbidden value (" + this.state + ") on element of ContentPart.state.");
			}
		}
		
	}
}
