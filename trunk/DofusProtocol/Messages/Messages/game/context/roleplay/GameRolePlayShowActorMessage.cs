using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameRolePlayShowActorMessage : Message
	{
		public const uint protocolId = 5632;
		internal Boolean _isInitialized = false;
		public GameRolePlayActorInformations informations;
		
		public GameRolePlayShowActorMessage()
		{
			this.informations = new GameRolePlayActorInformations();
		}
		
		public GameRolePlayShowActorMessage(GameRolePlayActorInformations arg1)
			: this()
		{
			initGameRolePlayShowActorMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5632;
		}
		
		public GameRolePlayShowActorMessage initGameRolePlayShowActorMessage(GameRolePlayActorInformations arg1 = null)
		{
			this.informations = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.informations = new GameRolePlayActorInformations();
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
			this.serializeAs_GameRolePlayShowActorMessage(arg1);
		}
		
		public void serializeAs_GameRolePlayShowActorMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.informations.getTypeId());
			this.informations.serialize(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameRolePlayShowActorMessage(arg1);
		}
		
		public void deserializeAs_GameRolePlayShowActorMessage(BigEndianReader arg1)
		{
			var loc1 = (ushort)arg1.ReadUShort();
			this.informations = ProtocolTypeManager.GetInstance<GameRolePlayActorInformations>((uint)loc1);
			this.informations.deserialize(arg1);
		}
		
	}
}
