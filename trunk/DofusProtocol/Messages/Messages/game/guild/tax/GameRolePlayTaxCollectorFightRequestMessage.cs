using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameRolePlayTaxCollectorFightRequestMessage : Message
	{
		public const uint protocolId = 5954;
		internal Boolean _isInitialized = false;
		public int taxCollectorId = 0;
		
		public GameRolePlayTaxCollectorFightRequestMessage()
		{
		}
		
		public GameRolePlayTaxCollectorFightRequestMessage(int arg1)
			: this()
		{
			initGameRolePlayTaxCollectorFightRequestMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5954;
		}
		
		public GameRolePlayTaxCollectorFightRequestMessage initGameRolePlayTaxCollectorFightRequestMessage(int arg1 = 0)
		{
			this.taxCollectorId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.taxCollectorId = 0;
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
			this.serializeAs_GameRolePlayTaxCollectorFightRequestMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayTaxCollectorFightRequestMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.taxCollectorId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayTaxCollectorFightRequestMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayTaxCollectorFightRequestMessage(BigEndianReader arg1)
		{
			this.taxCollectorId = (int)arg1.ReadInt();
		}
		
	}
}
