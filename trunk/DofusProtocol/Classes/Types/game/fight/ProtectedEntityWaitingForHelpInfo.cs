using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ProtectedEntityWaitingForHelpInfo : Object
	{
		public const uint protocolId = 186;
		public int timeLeftBeforeFight = 0;
		public int waitTimeForPlacement = 0;
		public uint nbPositionForDefensors = 0;
		
		public ProtectedEntityWaitingForHelpInfo()
		{
		}
		
		public ProtectedEntityWaitingForHelpInfo(int arg1, int arg2, uint arg3)
			: this()
		{
			initProtectedEntityWaitingForHelpInfo(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 186;
		}
		
		public ProtectedEntityWaitingForHelpInfo initProtectedEntityWaitingForHelpInfo(int arg1 = 0, int arg2 = 0, uint arg3 = 0)
		{
			this.timeLeftBeforeFight = arg1;
			this.waitTimeForPlacement = arg2;
			this.nbPositionForDefensors = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.timeLeftBeforeFight = 0;
			this.waitTimeForPlacement = 0;
			this.nbPositionForDefensors = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ProtectedEntityWaitingForHelpInfo(arg1);
		}
		
		public void serializeAs_ProtectedEntityWaitingForHelpInfo(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.timeLeftBeforeFight);
			arg1.WriteInt((int)this.waitTimeForPlacement);
			if ( this.nbPositionForDefensors < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbPositionForDefensors + ") on element nbPositionForDefensors.");
			}
			arg1.WriteByte((byte)this.nbPositionForDefensors);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ProtectedEntityWaitingForHelpInfo(arg1);
		}
		
		public void deserializeAs_ProtectedEntityWaitingForHelpInfo(BigEndianReader arg1)
		{
			this.timeLeftBeforeFight = (int)arg1.ReadInt();
			this.waitTimeForPlacement = (int)arg1.ReadInt();
			this.nbPositionForDefensors = (uint)arg1.ReadByte();
			if ( this.nbPositionForDefensors < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbPositionForDefensors + ") on element of ProtectedEntityWaitingForHelpInfo.nbPositionForDefensors.");
			}
		}
		
	}
}
