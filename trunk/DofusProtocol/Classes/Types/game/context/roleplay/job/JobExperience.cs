// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class JobExperience : Object
	{
		public const uint protocolId = 98;
		public uint jobId = 0;
		public uint jobLevel = 0;
		public double jobXP = 0;
		public double jobXpLevelFloor = 0;
		public double jobXpNextLevelFloor = 0;
		
		public JobExperience()
		{
		}
		
		public JobExperience(uint arg1, uint arg2, double arg3, double arg4, double arg5)
			: this()
		{
			initJobExperience(arg1, arg2, arg3, arg4, arg5);
		}
		
		public virtual uint getTypeId()
		{
			return 98;
		}
		
		public JobExperience initJobExperience(uint arg1 = 0, uint arg2 = 0, double arg3 = 0, double arg4 = 0, double arg5 = 0)
		{
			this.jobId = arg1;
			this.jobLevel = arg2;
			this.jobXP = arg3;
			this.jobXpLevelFloor = arg4;
			this.jobXpNextLevelFloor = arg5;
			return this;
		}
		
		public virtual void reset()
		{
			this.jobId = 0;
			this.jobLevel = 0;
			this.jobXP = 0;
			this.jobXpLevelFloor = 0;
			this.jobXpNextLevelFloor = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_JobExperience(arg1);
		}
		
		public void serializeAs_JobExperience(BigEndianWriter arg1)
		{
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element jobId.");
			}
			arg1.WriteByte((byte)this.jobId);
			if ( this.jobLevel < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobLevel + ") on element jobLevel.");
			}
			arg1.WriteByte((byte)this.jobLevel);
			if ( this.jobXP < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobXP + ") on element jobXP.");
			}
			arg1.WriteDouble(this.jobXP);
			if ( this.jobXpLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobXpLevelFloor + ") on element jobXpLevelFloor.");
			}
			arg1.WriteDouble(this.jobXpLevelFloor);
			if ( this.jobXpNextLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobXpNextLevelFloor + ") on element jobXpNextLevelFloor.");
			}
			arg1.WriteDouble(this.jobXpNextLevelFloor);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobExperience(arg1);
		}
		
		public void deserializeAs_JobExperience(BigEndianReader arg1)
		{
			this.jobId = (uint)arg1.ReadByte();
			if ( this.jobId < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobId + ") on element of JobExperience.jobId.");
			}
			this.jobLevel = (uint)arg1.ReadByte();
			if ( this.jobLevel < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobLevel + ") on element of JobExperience.jobLevel.");
			}
			this.jobXP = (double)arg1.ReadDouble();
			if ( this.jobXP < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobXP + ") on element of JobExperience.jobXP.");
			}
			this.jobXpLevelFloor = (double)arg1.ReadDouble();
			if ( this.jobXpLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobXpLevelFloor + ") on element of JobExperience.jobXpLevelFloor.");
			}
			this.jobXpNextLevelFloor = (double)arg1.ReadDouble();
			if ( this.jobXpNextLevelFloor < 0 )
			{
				throw new Exception("Forbidden value (" + this.jobXpNextLevelFloor + ") on element of JobExperience.jobXpNextLevelFloor.");
			}
		}
		
	}
}
