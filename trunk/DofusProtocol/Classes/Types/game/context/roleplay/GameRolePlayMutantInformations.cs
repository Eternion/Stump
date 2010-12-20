using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameRolePlayMutantInformations : GameRolePlayHumanoidInformations
	{
		public const uint protocolId = 3;
		public int monsterId = 0;
		public int powerLevel = 0;
		
		public GameRolePlayMutantInformations()
		{
		}
		
		public GameRolePlayMutantInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, String arg4, HumanInformations arg5, int arg6, int arg7)
			: this()
		{
			initGameRolePlayMutantInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getTypeId()
		{
			return 3;
		}
		
		public GameRolePlayMutantInformations initGameRolePlayMutantInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, String arg4 = "", HumanInformations arg5 = null, int arg6 = 0, int arg7 = 0)
		{
			base.initGameRolePlayHumanoidInformations(arg1, arg2, arg3, arg4, arg5);
			this.monsterId = arg6;
			this.powerLevel = arg7;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.monsterId = 0;
			this.powerLevel = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameRolePlayMutantInformations(arg1);
		}
		
		public void serializeAs_GameRolePlayMutantInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameRolePlayHumanoidInformations(arg1);
			arg1.WriteInt((int)this.monsterId);
			arg1.WriteByte((byte)this.powerLevel);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayMutantInformations(arg1);
		}
		
		public void deserializeAs_GameRolePlayMutantInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.monsterId = (int)arg1.ReadInt();
			this.powerLevel = (int)arg1.ReadByte();
		}
		
	}
}
