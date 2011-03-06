using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ActorExtendedAlignmentInformations : ActorAlignmentInformations
	{
		public const uint protocolId = 202;
		public uint honor = 0;
		public uint honorGradeFloor = 0;
		public uint honorNextGradeFloor = 0;
		public Boolean pvpEnabled = false;
		
		public ActorExtendedAlignmentInformations()
		{
		}
		
		public ActorExtendedAlignmentInformations(int arg1, uint arg2, uint arg3, uint arg4, uint arg5, uint arg6, uint arg7, uint arg8, Boolean arg9)
			: this()
		{
			initActorExtendedAlignmentInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
		}
		
		public override uint getTypeId()
		{
			return 202;
		}
		
		public ActorExtendedAlignmentInformations initActorExtendedAlignmentInformations(int arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0, uint arg5 = 0, uint arg6 = 0, uint arg7 = 0, uint arg8 = 0, Boolean arg9 = false)
		{
			base.initActorAlignmentInformations(arg1, arg2, arg3, arg4, arg5);
			this.honor = arg6;
			this.honorGradeFloor = arg7;
			this.honorNextGradeFloor = arg8;
			this.pvpEnabled = arg9;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.honor = 0;
			this.honorGradeFloor = 0;
			this.honorNextGradeFloor = 0;
			this.pvpEnabled = false;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ActorExtendedAlignmentInformations(arg1);
		}
		
		public void serializeAs_ActorExtendedAlignmentInformations(BigEndianWriter arg1)
		{
			base.serializeAs_ActorAlignmentInformations(arg1);
			if ( this.honor < 0 || this.honor > 20000 )
			{
				throw new Exception("Forbidden value (" + this.honor + ") on element honor.");
			}
			arg1.WriteShort((short)this.honor);
			if ( this.honorGradeFloor < 0 || this.honorGradeFloor > 20000 )
			{
				throw new Exception("Forbidden value (" + this.honorGradeFloor + ") on element honorGradeFloor.");
			}
			arg1.WriteShort((short)this.honorGradeFloor);
			if ( this.honorNextGradeFloor < 0 || this.honorNextGradeFloor > 20000 )
			{
				throw new Exception("Forbidden value (" + this.honorNextGradeFloor + ") on element honorNextGradeFloor.");
			}
			arg1.WriteShort((short)this.honorNextGradeFloor);
			arg1.WriteBoolean(this.pvpEnabled);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ActorExtendedAlignmentInformations(arg1);
		}
		
		public void deserializeAs_ActorExtendedAlignmentInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.honor = (uint)arg1.ReadUShort();
			if ( this.honor < 0 || this.honor > 20000 )
			{
				throw new Exception("Forbidden value (" + this.honor + ") on element of ActorExtendedAlignmentInformations.honor.");
			}
			this.honorGradeFloor = (uint)arg1.ReadUShort();
			if ( this.honorGradeFloor < 0 || this.honorGradeFloor > 20000 )
			{
				throw new Exception("Forbidden value (" + this.honorGradeFloor + ") on element of ActorExtendedAlignmentInformations.honorGradeFloor.");
			}
			this.honorNextGradeFloor = (uint)arg1.ReadUShort();
			if ( this.honorNextGradeFloor < 0 || this.honorNextGradeFloor > 20000 )
			{
				throw new Exception("Forbidden value (" + this.honorNextGradeFloor + ") on element of ActorExtendedAlignmentInformations.honorNextGradeFloor.");
			}
			this.pvpEnabled = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
