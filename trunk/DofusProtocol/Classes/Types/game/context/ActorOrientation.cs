using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ActorOrientation : Object
	{
		public const uint protocolId = 353;
		public int id = 0;
		public uint direction = 1;
		
		public ActorOrientation()
		{
		}
		
		public ActorOrientation(int arg1, uint arg2)
			: this()
		{
			initActorOrientation(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 353;
		}
		
		public ActorOrientation initActorOrientation(int arg1 = 0, uint arg2 = 1)
		{
			this.id = arg1;
			this.direction = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.id = 0;
			this.direction = 1;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ActorOrientation(arg1);
		}
		
		public void serializeAs_ActorOrientation(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.id);
			arg1.WriteByte((byte)this.direction);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ActorOrientation(arg1);
		}
		
		public void deserializeAs_ActorOrientation(BigEndianReader arg1)
		{
			this.id = (int)arg1.ReadInt();
			this.direction = (uint)arg1.ReadByte();
			if ( this.direction < 0 )
			{
				throw new Exception("Forbidden value (" + this.direction + ") on element of ActorOrientation.direction.");
			}
		}
		
	}
}
