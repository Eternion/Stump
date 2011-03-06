using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ActorAlignmentInformations : Object
	{
		public const uint protocolId = 201;
		public int alignmentSide = 0;
		public uint alignmentValue = 0;
		public uint alignmentGrade = 0;
		public uint dishonor = 0;
		public uint characterPower = 0;
		
		public ActorAlignmentInformations()
		{
		}
		
		public ActorAlignmentInformations(int arg1, uint arg2, uint arg3, uint arg4, uint arg5)
			: this()
		{
			initActorAlignmentInformations(arg1, arg2, arg3, arg4, arg5);
		}
		
		public virtual uint getTypeId()
		{
			return 201;
		}
		
		public ActorAlignmentInformations initActorAlignmentInformations(int arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0, uint arg5 = 0)
		{
			this.alignmentSide = arg1;
			this.alignmentValue = arg2;
			this.alignmentGrade = arg3;
			this.dishonor = arg4;
			this.characterPower = arg5;
			return this;
		}
		
		public virtual void reset()
		{
			this.alignmentSide = 0;
			this.alignmentValue = 0;
			this.alignmentGrade = 0;
			this.dishonor = 0;
			this.characterPower = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ActorAlignmentInformations(arg1);
		}
		
		public void serializeAs_ActorAlignmentInformations(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.alignmentSide);
			if ( this.alignmentValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.alignmentValue + ") on element alignmentValue.");
			}
			arg1.WriteByte((byte)this.alignmentValue);
			if ( this.alignmentGrade < 0 )
			{
				throw new Exception("Forbidden value (" + this.alignmentGrade + ") on element alignmentGrade.");
			}
			arg1.WriteByte((byte)this.alignmentGrade);
			if ( this.dishonor < 0 || this.dishonor > 500 )
			{
				throw new Exception("Forbidden value (" + this.dishonor + ") on element dishonor.");
			}
			arg1.WriteShort((short)this.dishonor);
			if ( this.characterPower < 0 )
			{
				throw new Exception("Forbidden value (" + this.characterPower + ") on element characterPower.");
			}
			arg1.WriteInt((int)this.characterPower);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ActorAlignmentInformations(arg1);
		}
		
		public void deserializeAs_ActorAlignmentInformations(BigEndianReader arg1)
		{
			this.alignmentSide = (int)arg1.ReadByte();
			this.alignmentValue = (uint)arg1.ReadByte();
			if ( this.alignmentValue < 0 )
			{
				throw new Exception("Forbidden value (" + this.alignmentValue + ") on element of ActorAlignmentInformations.alignmentValue.");
			}
			this.alignmentGrade = (uint)arg1.ReadByte();
			if ( this.alignmentGrade < 0 )
			{
				throw new Exception("Forbidden value (" + this.alignmentGrade + ") on element of ActorAlignmentInformations.alignmentGrade.");
			}
			this.dishonor = (uint)arg1.ReadUShort();
			if ( this.dishonor < 0 || this.dishonor > 500 )
			{
				throw new Exception("Forbidden value (" + this.dishonor + ") on element of ActorAlignmentInformations.dishonor.");
			}
			this.characterPower = (uint)arg1.ReadInt();
			if ( this.characterPower < 0 )
			{
				throw new Exception("Forbidden value (" + this.characterPower + ") on element of ActorAlignmentInformations.characterPower.");
			}
		}
		
	}
}
