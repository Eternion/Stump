using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameFightMutantInformations : GameFightFighterNamedInformations
	{
		public const uint protocolId = 50;
		public uint powerLevel = 0;
		
		public GameFightMutantInformations()
		{
		}
		
		public GameFightMutantInformations(int arg1, EntityLook arg2, EntityDispositionInformations arg3, uint arg4, Boolean arg5, GameFightMinimalStats arg6, String arg7, uint arg8)
			: this()
		{
			initGameFightMutantInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}
		
		public override uint getTypeId()
		{
			return 50;
		}
		
		public GameFightMutantInformations initGameFightMutantInformations(int arg1 = 0, EntityLook arg2 = null, EntityDispositionInformations arg3 = null, uint arg4 = 2, Boolean arg5 = false, GameFightMinimalStats arg6 = null, String arg7 = "", uint arg8 = 0)
		{
			base.initGameFightFighterNamedInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			this.powerLevel = arg8;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.powerLevel = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightMutantInformations(arg1);
		}
		
		public void serializeAs_GameFightMutantInformations(BigEndianWriter arg1)
		{
			base.serializeAs_GameFightFighterNamedInformations(arg1);
			if ( this.powerLevel < 0 )
			{
				throw new Exception("Forbidden value (" + this.powerLevel + ") on element powerLevel.");
			}
			arg1.WriteByte((byte)this.powerLevel);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightMutantInformations(arg1);
		}
		
		public void deserializeAs_GameFightMutantInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.powerLevel = (uint)arg1.ReadByte();
			if ( this.powerLevel < 0 )
			{
				throw new Exception("Forbidden value (" + this.powerLevel + ") on element of GameFightMutantInformations.powerLevel.");
			}
		}
		
	}
}
