using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameRolePlaySpellAnimMessage : Message
	{
		public const uint protocolId = 6114;
		internal Boolean _isInitialized = false;
		public int casterId = 0;
		public uint targetCellId = 0;
		public uint spellId = 0;
		public uint spellLevel = 0;
		
		public GameRolePlaySpellAnimMessage()
		{
		}
		
		public GameRolePlaySpellAnimMessage(int arg1, uint arg2, uint arg3, uint arg4)
			: this()
		{
			initGameRolePlaySpellAnimMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6114;
		}
		
		public GameRolePlaySpellAnimMessage initGameRolePlaySpellAnimMessage(int arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0)
		{
			this.casterId = arg1;
			this.targetCellId = arg2;
			this.spellId = arg3;
			this.spellLevel = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.casterId = 0;
			this.targetCellId = 0;
			this.spellId = 0;
			this.spellLevel = 0;
			this._isInitialized = false;
		}
		
		public override void pack(BigEndianWriter arg1)
		{
			this.serialize(arg1);
			WritePacket(arg1, this.getMessageId());
		}
		
		public override void unpack(BigEndianReader arg1, uint arg2)
		{
			this.deserialize(arg1);
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_GameRolePlaySpellAnimMessage(arg1);
		}
		
		public void serializeAs_GameRolePlaySpellAnimMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.casterId);
			if ( this.targetCellId < 0 || this.targetCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.targetCellId + ") on element targetCellId.");
			}
			arg1.WriteShort((short)this.targetCellId);
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element spellId.");
			}
			arg1.WriteShort((short)this.spellId);
			if ( this.spellLevel < 1 || this.spellLevel > 6 )
			{
				throw new Exception("Forbidden value (" + this.spellLevel + ") on element spellLevel.");
			}
			arg1.WriteByte((byte)this.spellLevel);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlaySpellAnimMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlaySpellAnimMessage(BigEndianReader arg1)
		{
			this.casterId = (int)arg1.ReadInt();
			this.targetCellId = (uint)arg1.ReadShort();
			if ( this.targetCellId < 0 || this.targetCellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.targetCellId + ") on element of GameRolePlaySpellAnimMessage.targetCellId.");
			}
			this.spellId = (uint)arg1.ReadShort();
			if ( this.spellId < 0 )
			{
				throw new Exception("Forbidden value (" + this.spellId + ") on element of GameRolePlaySpellAnimMessage.spellId.");
			}
			this.spellLevel = (uint)arg1.ReadByte();
			if ( this.spellLevel < 1 || this.spellLevel > 6 )
			{
				throw new Exception("Forbidden value (" + this.spellLevel + ") on element of GameRolePlaySpellAnimMessage.spellLevel.");
			}
		}
		
	}
}
