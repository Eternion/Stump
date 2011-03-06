using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class Shortcut : Object
	{
		public const uint protocolId = 369;
		public uint slot = 0;
		
		public Shortcut()
		{
		}
		
		public Shortcut(uint arg1)
			: this()
		{
			initShortcut(arg1);
		}
		
		public virtual uint getTypeId()
		{
			return 369;
		}
		
		public Shortcut initShortcut(uint arg1 = 0)
		{
			this.slot = arg1;
			return this;
		}
		
		public virtual void reset()
		{
			this.slot = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_Shortcut(arg1);
		}
		
		public void serializeAs_Shortcut(BigEndianWriter arg1)
		{
			if ( this.slot < 0 || this.slot > 99 )
			{
				throw new Exception("Forbidden value (" + this.slot + ") on element slot.");
			}
			arg1.WriteInt((int)this.slot);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_Shortcut(arg1);
		}
		
		public void deserializeAs_Shortcut(BigEndianReader arg1)
		{
			this.slot = (uint)arg1.ReadInt();
			if ( this.slot < 0 || this.slot > 99 )
			{
				throw new Exception("Forbidden value (" + this.slot + ") on element of Shortcut.slot.");
			}
		}
		
	}
}
