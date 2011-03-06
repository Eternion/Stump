using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class CharacterHardcoreInformations : CharacterBaseInformations
	{
		public const uint protocolId = 86;
		public uint deathState = 0;
		public uint deathCount = 0;
		public uint deathMaxLevel = 0;
		
		public CharacterHardcoreInformations()
		{
		}
		
		public CharacterHardcoreInformations(uint arg1, uint arg2, String arg3, EntityLook arg4, int arg5, Boolean arg6, uint arg7, uint arg8, uint arg9)
			: this()
		{
			initCharacterHardcoreInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
		}
		
		public override uint getTypeId()
		{
			return 86;
		}
		
		public CharacterHardcoreInformations initCharacterHardcoreInformations(uint arg1 = 0, uint arg2 = 0, String arg3 = "", EntityLook arg4 = null, int arg5 = 0, Boolean arg6 = false, uint arg7 = 0, uint arg8 = 0, uint arg9 = 0)
		{
			base.initCharacterBaseInformations(arg1, arg2, arg3, arg4, arg5, arg6);
			this.deathState = arg7;
			this.deathCount = arg8;
			this.deathMaxLevel = arg9;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.deathState = 0;
			this.deathCount = 0;
			this.deathMaxLevel = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_CharacterHardcoreInformations(arg1);
		}
		
		public void serializeAs_CharacterHardcoreInformations(BigEndianWriter arg1)
		{
			base.serializeAs_CharacterBaseInformations(arg1);
			arg1.WriteByte((byte)this.deathState);
			if ( this.deathCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.deathCount + ") on element deathCount.");
			}
			arg1.WriteShort((short)this.deathCount);
			if ( this.deathMaxLevel < 1 || this.deathMaxLevel > 200 )
			{
				throw new Exception("Forbidden value (" + this.deathMaxLevel + ") on element deathMaxLevel.");
			}
			arg1.WriteByte((byte)this.deathMaxLevel);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterHardcoreInformations(arg1);
		}
		
		public void deserializeAs_CharacterHardcoreInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.deathState = (uint)arg1.ReadByte();
			if ( this.deathState < 0 )
			{
				throw new Exception("Forbidden value (" + this.deathState + ") on element of CharacterHardcoreInformations.deathState.");
			}
			this.deathCount = (uint)arg1.ReadShort();
			if ( this.deathCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.deathCount + ") on element of CharacterHardcoreInformations.deathCount.");
			}
			this.deathMaxLevel = (uint)arg1.ReadByte();
			if ( this.deathMaxLevel < 1 || this.deathMaxLevel > 200 )
			{
				throw new Exception("Forbidden value (" + this.deathMaxLevel + ") on element of CharacterHardcoreInformations.deathMaxLevel.");
			}
		}
		
	}
}
