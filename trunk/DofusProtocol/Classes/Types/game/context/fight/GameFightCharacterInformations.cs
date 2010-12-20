using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameFightCharacterInformations : GameFightFighterNamedInformations
	{
		public const uint protocolId = 46;
		public uint level = 0;
		public ActorAlignmentInformations alignmentInfos;
		
		public GameFightCharacterInformations()
		{
			this.alignmentInfos = new ActorAlignmentInformations();
		}
		
		public GameFightCharacterInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, uint arg4, Boolean arg5, GameFightMinimalStats arg6, String arg7, uint arg8, ActorAlignmentInformations arg9)
			: this()
		{
			initGameFightCharacterInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
		}
		
		public override uint getTypeId()
		{
			return 46;
		}
		
		public GameFightCharacterInformations initGameFightCharacterInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, uint arg4 = 2, Boolean arg5 = false, GameFightMinimalStats arg6 = null, String arg7 = "", uint arg8 = 0, ActorAlignmentInformations arg9 = null)
		{
			base.initGameFightFighterNamedInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			this.level = arg8;
			this.alignmentInfos = arg9;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.level = 0;
			this.alignmentInfos = new ActorAlignmentInformations();
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightCharacterInformations(arg1);
		}
		
		public void serializeAs_GameFightCharacterInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameFightFighterNamedInformations(arg1);
			if ( this.level < 0 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element level.");
			}
			arg1.WriteShort((short)this.level);
			this.alignmentInfos.serializeAs_ActorAlignmentInformations(arg1);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightCharacterInformations(arg1);
		}
		
		public void deserializeAs_GameFightCharacterInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.level = (uint)arg1.ReadShort();
			if ( this.level < 0 )
			{
				throw new Exception("Forbidden value (" + this.level + ") on element of GameFightCharacterInformations.level.");
			}
			this.alignmentInfos = new ActorAlignmentInformations();
			this.alignmentInfos.deserialize(arg1);
		}
		
	}
}
