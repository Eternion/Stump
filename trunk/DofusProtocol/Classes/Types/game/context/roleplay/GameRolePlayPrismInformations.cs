using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameRolePlayPrismInformations : GameRolePlayActorInformations
	{
		public const uint protocolId = 161;
		public ActorAlignmentInformations alignInfos;
		
		public GameRolePlayPrismInformations()
		{
			this.alignInfos = new ActorAlignmentInformations();
		}
		
		public GameRolePlayPrismInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, ActorAlignmentInformations arg4)
			: this()
		{
			initGameRolePlayPrismInformations(arg1, arg2, arg3, arg4);
		}
		
		public override uint getTypeId()
		{
			return 161;
		}
		
		public GameRolePlayPrismInformations initGameRolePlayPrismInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, ActorAlignmentInformations arg4 = null)
		{
			base.initGameRolePlayActorInformations(arg1, arg2, arg3);
			this.alignInfos = arg4;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.alignInfos = new ActorAlignmentInformations();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameRolePlayPrismInformations(arg1);
		}
		
		public void serializeAs_GameRolePlayPrismInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameRolePlayActorInformations(arg1);
			this.alignInfos.serializeAs_ActorAlignmentInformations(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayPrismInformations(arg1);
		}
		
		public void deserializeAs_GameRolePlayPrismInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.alignInfos = new ActorAlignmentInformations();
			this.alignInfos.deserialize(arg1);
		}
		
	}
}
