using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameRolePlayNamedActorInformations : GameRolePlayActorInformations
	{
		public const uint protocolId = 154;
		public String name = "";
		
		public GameRolePlayNamedActorInformations()
		{
		}
		
		public GameRolePlayNamedActorInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, String arg4)
			: this()
		{
			initGameRolePlayNamedActorInformations(arg1, arg2, arg3, arg4);
		}
		
		public override uint getTypeId()
		{
			return 154;
		}
		
		public GameRolePlayNamedActorInformations initGameRolePlayNamedActorInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, String arg4 = "")
		{
			base.initGameRolePlayActorInformations(arg1, arg2, arg3);
			this.name = arg4;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.name = "";
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameRolePlayNamedActorInformations(arg1);
		}
		
		public void serializeAs_GameRolePlayNamedActorInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameRolePlayActorInformations(arg1);
			arg1.WriteUTF((string)this.name);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayNamedActorInformations(arg1);
		}
		
		public void deserializeAs_GameRolePlayNamedActorInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.name = (String)arg1.ReadUTF();
		}
		
	}
}
