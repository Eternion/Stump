using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class NpcStaticInformations : Object
	{
		public const uint protocolId = 155;
		public uint npcId = 0;
		public Boolean sex = false;
		public uint specialArtworkId = 0;
		
		public NpcStaticInformations()
		{
		}
		
		public NpcStaticInformations(uint arg1, Boolean arg2, uint arg3)
			: this()
		{
			initNpcStaticInformations(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 155;
		}
		
		public NpcStaticInformations initNpcStaticInformations(uint arg1 = 0, Boolean arg2 = false, uint arg3 = 0)
		{
			this.npcId = arg1;
			this.sex = arg2;
			this.specialArtworkId = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.npcId = 0;
			this.sex = false;
			this.specialArtworkId = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_NpcStaticInformations(arg1);
		}
		
		public void serializeAs_NpcStaticInformations(BigEndianWriter arg1)
		{
			if ( this.npcId < 0 )
			{
				throw new Exception("Forbidden value (" + this.npcId + ") on element npcId.");
			}
			arg1.WriteShort((short)this.npcId);
			arg1.WriteBoolean(this.sex);
			if ( this.specialArtworkId < 0 )
			{
				throw new Exception("Forbidden value (" + this.specialArtworkId + ") on element specialArtworkId.");
			}
			arg1.WriteShort((short)this.specialArtworkId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_NpcStaticInformations(arg1);
		}
		
		public void deserializeAs_NpcStaticInformations(BigEndianReader arg1)
		{
			this.npcId = (uint)arg1.ReadShort();
			if ( this.npcId < 0 )
			{
				throw new Exception("Forbidden value (" + this.npcId + ") on element of NpcStaticInformations.npcId.");
			}
			this.sex = (Boolean)arg1.ReadBoolean();
			this.specialArtworkId = (uint)arg1.ReadShort();
			if ( this.specialArtworkId < 0 )
			{
				throw new Exception("Forbidden value (" + this.specialArtworkId + ") on element of NpcStaticInformations.specialArtworkId.");
			}
		}
		
	}
}
