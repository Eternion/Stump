using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class PartyMemberInformations : CharacterMinimalPlusLookInformations
	{
		public const uint protocolId = 90;
		public uint lifePoints = 0;
		public uint maxLifePoints = 0;
		public uint prospecting = 0;
		public uint regenRate = 0;
		public uint initiative = 0;
		public Boolean pvpEnabled = false;
		public int alignmentSide = 0;
		
		public PartyMemberInformations()
		{
		}
		
		public PartyMemberInformations(uint arg1, uint arg2, String arg3, EntityLook arg4, uint arg5, uint arg6, uint arg7, uint arg8, uint arg9, Boolean arg10, int arg11)
			: this()
		{
			initPartyMemberInformations(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
		}
		
		public override uint getTypeId()
		{
			return 90;
		}
		
		public PartyMemberInformations initPartyMemberInformations(uint arg1 = 0, uint arg2 = 0, String arg3 = "", EntityLook arg4 = null, uint arg5 = 0, uint arg6 = 0, uint arg7 = 0, uint arg8 = 0, uint arg9 = 0, Boolean arg10 = false, int arg11 = 0)
		{
			base.initCharacterMinimalPlusLookInformations(arg1, arg2, arg3, arg4);
			this.lifePoints = arg5;
			this.maxLifePoints = arg6;
			this.prospecting = arg7;
			this.regenRate = arg8;
			this.initiative = arg9;
			this.pvpEnabled = arg10;
			this.alignmentSide = arg11;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.lifePoints = 0;
			this.maxLifePoints = 0;
			this.prospecting = 0;
			this.regenRate = 0;
			this.initiative = 0;
			this.pvpEnabled = false;
			this.alignmentSide = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_PartyMemberInformations(arg1);
		}
		
		public void serializeAs_PartyMemberInformations(BigEndianWriter arg1)
		{
			base.serializeAs_CharacterMinimalPlusLookInformations(arg1);
			if ( this.lifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.lifePoints + ") on element lifePoints.");
			}
			arg1.WriteInt((int)this.lifePoints);
			if ( this.maxLifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxLifePoints + ") on element maxLifePoints.");
			}
			arg1.WriteInt((int)this.maxLifePoints);
			if ( this.prospecting < 0 )
			{
				throw new Exception("Forbidden value (" + this.prospecting + ") on element prospecting.");
			}
			arg1.WriteShort((short)this.prospecting);
			if ( this.regenRate < 0 || this.regenRate > 255 )
			{
				throw new Exception("Forbidden value (" + this.regenRate + ") on element regenRate.");
			}
			arg1.WriteByte((byte)this.regenRate);
			if ( this.initiative < 0 )
			{
				throw new Exception("Forbidden value (" + this.initiative + ") on element initiative.");
			}
			arg1.WriteShort((short)this.initiative);
			arg1.WriteBoolean(this.pvpEnabled);
			arg1.WriteByte((byte)this.alignmentSide);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartyMemberInformations(arg1);
		}
		
		public void deserializeAs_PartyMemberInformations(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.lifePoints = (uint)arg1.ReadInt();
			if ( this.lifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.lifePoints + ") on element of PartyMemberInformations.lifePoints.");
			}
			this.maxLifePoints = (uint)arg1.ReadInt();
			if ( this.maxLifePoints < 0 )
			{
				throw new Exception("Forbidden value (" + this.maxLifePoints + ") on element of PartyMemberInformations.maxLifePoints.");
			}
			this.prospecting = (uint)arg1.ReadShort();
			if ( this.prospecting < 0 )
			{
				throw new Exception("Forbidden value (" + this.prospecting + ") on element of PartyMemberInformations.prospecting.");
			}
			this.regenRate = (uint)arg1.ReadByte();
			if ( this.regenRate < 0 || this.regenRate > 255 )
			{
				throw new Exception("Forbidden value (" + this.regenRate + ") on element of PartyMemberInformations.regenRate.");
			}
			this.initiative = (uint)arg1.ReadShort();
			if ( this.initiative < 0 )
			{
				throw new Exception("Forbidden value (" + this.initiative + ") on element of PartyMemberInformations.initiative.");
			}
			this.pvpEnabled = (Boolean)arg1.ReadBoolean();
			this.alignmentSide = (int)arg1.ReadByte();
		}
		
	}
}
