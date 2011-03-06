using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameRolePlayActorInformations : GameContextActorInformations
	{
		public const uint protocolId = 141;
		
		public GameRolePlayActorInformations()
		{
		}
		
		public GameRolePlayActorInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3)
			: this()
		{
			initGameRolePlayActorInformations(arg1, arg2, arg3);
		}
		
		public override uint getTypeId()
		{
			return 141;
		}
		
		public GameRolePlayActorInformations initGameRolePlayActorInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null)
		{
			base.initGameContextActorInformations(arg1, arg2, arg3);
			return this;
		}
		
		public override void reset()
		{
			base.reset();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameRolePlayActorInformations(arg1);
		}
		
		public void serializeAs_GameRolePlayActorInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameContextActorInformations(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayActorInformations(arg1);
		}
		
		public void deserializeAs_GameRolePlayActorInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
		}
		
	}
}
