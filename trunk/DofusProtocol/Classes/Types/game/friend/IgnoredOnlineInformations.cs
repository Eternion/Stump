using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class IgnoredOnlineInformations : IgnoredInformations
	{
		public const uint protocolId = 105;
		public String playerName = "";
		public int breed = 0;
		public Boolean sex = false;
		
		public IgnoredOnlineInformations()
		{
		}
		
		public IgnoredOnlineInformations(String arg1, uint arg2, String arg3, int arg4, Boolean arg5)
			: this()
		{
			initIgnoredOnlineInformations(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getTypeId()
		{
			return 105;
		}
		
		public IgnoredOnlineInformations initIgnoredOnlineInformations(String arg1 = "", uint arg2 = 0, String arg3 = "", int arg4 = 0, Boolean arg5 = false)
		{
			base.initIgnoredInformations(arg1, arg2);
			this.playerName = arg3;
			this.breed = arg4;
			this.sex = arg5;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.playerName = "";
			this.breed = 0;
			this.sex = false;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_IgnoredOnlineInformations(arg1);
		}
		
		public void serializeAs_IgnoredOnlineInformations(BigEndianWriter arg1)
		{
			base.serializeAs_IgnoredInformations(arg1);
			arg1.WriteUTF((string)this.playerName);
			arg1.WriteByte((byte)this.breed);
			arg1.WriteBoolean(this.sex);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_IgnoredOnlineInformations(arg1);
		}
		
		public void deserializeAs_IgnoredOnlineInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.playerName = (String)arg1.ReadUTF();
			this.breed = (int)arg1.ReadByte();
            if (this.breed < (int)Stump.DofusProtocol.Enums.BreedEnum.Feca || this.breed > (int)Stump.DofusProtocol.Enums.BreedEnum.Zobal)
			{
				throw new Exception("Forbidden value (" + this.breed + ") on element of IgnoredOnlineInformations.breed.");
			}
			this.sex = (Boolean)arg1.ReadBoolean();
		}
		
	}
}
