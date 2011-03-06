using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class GameFightMinimalStatsPreparation : GameFightMinimalStats
	{
		public const uint protocolId = 360;
		public uint initiative = 0;
		
		public GameFightMinimalStatsPreparation()
		{
		}
		
		public GameFightMinimalStatsPreparation(uint arg1, uint arg2, int arg3, int arg4, int arg5, int arg6, int arg7, int arg8, int arg9, int arg10, uint arg11, uint arg12, uint arg13, int arg14, uint arg15)
			: this()
		{
			initGameFightMinimalStatsPreparation(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
		}
		
		public override uint getTypeId()
		{
			return 360;
		}
		
		public GameFightMinimalStatsPreparation initGameFightMinimalStatsPreparation(uint arg1 = 0, uint arg2 = 0, int arg3 = 0, int arg4 = 0, int arg5 = 0, int arg6 = 0, int arg7 = 0, int arg8 = 0, int arg9 = 0, int arg10 = 0, uint arg11 = 0, uint arg12 = 0, uint arg13 = 0, int arg14 = 0, uint arg15 = 0)
		{
			base.initGameFightMinimalStats(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
			this.initiative = arg15;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.initiative = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameFightMinimalStatsPreparation(arg1);
		}
		
		public void serializeAs_GameFightMinimalStatsPreparation(BigEndianWriter arg1)
		{
			base.serializeAs_GameFightMinimalStats(arg1);
			if ( this.initiative < 0 )
			{
				throw new Exception("Forbidden value (" + this.initiative + ") on element initiative.");
			}
			arg1.WriteInt((int)this.initiative);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightMinimalStatsPreparation(arg1);
		}
		
		public void deserializeAs_GameFightMinimalStatsPreparation(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.initiative = (uint)arg1.ReadInt();
			if ( this.initiative < 0 )
			{
				throw new Exception("Forbidden value (" + this.initiative + ") on element of GameFightMinimalStatsPreparation.initiative.");
			}
		}
		
	}
}
