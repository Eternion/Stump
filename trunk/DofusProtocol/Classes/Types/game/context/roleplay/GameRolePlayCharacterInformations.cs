using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameRolePlayCharacterInformations : GameRolePlayHumanoidInformations
	{
		public const uint protocolId = 36;
		public ActorAlignmentInformations alignmentInfos;
		
		public GameRolePlayCharacterInformations()
		{
			this.alignmentInfos = new ActorAlignmentInformations();
		}
		
		public GameRolePlayCharacterInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, String arg4, HumanInformations arg5, ActorAlignmentInformations arg6)
			: this()
		{
			initGameRolePlayCharacterInformations(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getTypeId()
		{
			return 36;
		}
		
		public GameRolePlayCharacterInformations initGameRolePlayCharacterInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, String arg4 = "", HumanInformations arg5 = null, ActorAlignmentInformations arg6 = null)
		{
			base.initGameRolePlayHumanoidInformations(arg1, arg2, arg3, arg4, arg5);
			this.alignmentInfos = arg6;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.alignmentInfos = new ActorAlignmentInformations();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameRolePlayCharacterInformations(arg1);
		}
		
		public void serializeAs_GameRolePlayCharacterInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameRolePlayHumanoidInformations(arg1);
			this.alignmentInfos.serializeAs_ActorAlignmentInformations(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayCharacterInformations(arg1);
		}
		
		public void deserializeAs_GameRolePlayCharacterInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.alignmentInfos = new ActorAlignmentInformations();
			this.alignmentInfos.deserialize(arg1);
		}
		
	}
}
