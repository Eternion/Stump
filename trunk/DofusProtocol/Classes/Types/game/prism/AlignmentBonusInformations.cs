using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class AlignmentBonusInformations : Object
	{
		public const uint protocolId = 135;
		public uint pctbonus = 0;
		public double grademult = 0;
		
		public AlignmentBonusInformations()
		{
		}
		
		public AlignmentBonusInformations(uint arg1, double arg2)
			: this()
		{
			initAlignmentBonusInformations(arg1, arg2);
		}
		
		public virtual uint getTypeId()
		{
			return 135;
		}
		
		public AlignmentBonusInformations initAlignmentBonusInformations(uint arg1 = 0, double arg2 = 0)
		{
			this.pctbonus = arg1;
			this.grademult = arg2;
			return this;
		}
		
		public virtual void reset()
		{
			this.pctbonus = 0;
			this.grademult = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_AlignmentBonusInformations(arg1);
		}
		
		public void serializeAs_AlignmentBonusInformations(BigEndianWriter arg1)
		{
			if ( this.pctbonus < 0 )
			{
				throw new Exception("Forbidden value (" + this.pctbonus + ") on element pctbonus.");
			}
			arg1.WriteInt((int)this.pctbonus);
			arg1.WriteDouble(this.grademult);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AlignmentBonusInformations(arg1);
		}
		
		public void deserializeAs_AlignmentBonusInformations(BigEndianReader arg1)
		{
			this.pctbonus = (uint)arg1.ReadInt();
			if ( this.pctbonus < 0 )
			{
				throw new Exception("Forbidden value (" + this.pctbonus + ") on element of AlignmentBonusInformations.pctbonus.");
			}
			this.grademult = (double)arg1.ReadDouble();
		}
		
	}
}
