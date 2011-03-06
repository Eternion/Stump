using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameFightAIInformations : GameFightFighterInformations
	{
		public const uint protocolId = 151;
		
		public GameFightAIInformations()
		{
		}
		
		public GameFightAIInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, uint arg4, Boolean arg5, GameFightMinimalStats arg6)
			: this()
		{
			initGameFightAIInformations(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public override uint getTypeId()
		{
			return 151;
		}
		
		public GameFightAIInformations initGameFightAIInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, uint arg4 = 2, Boolean arg5 = false, GameFightMinimalStats arg6 = null)
		{
			base.initGameFightFighterInformations(arg1, arg2, arg3, arg4, arg5, arg6);
			return this;
		}
		
		public override void reset()
		{
			base.reset();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightAIInformations(arg1);
		}
		
		public void serializeAs_GameFightAIInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameFightFighterInformations(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightAIInformations(arg1);
		}
		
		public void deserializeAs_GameFightAIInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
		}
		
	}
}
