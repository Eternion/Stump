using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameFightShowFighterMessage : Message
	{
		public const uint protocolId = 5864;
		internal Boolean _isInitialized = false;
		public GameFightFighterInformations informations;
		
		public GameFightShowFighterMessage()
		{
			this.informations = new GameFightFighterInformations();
		}
		
		public GameFightShowFighterMessage(GameFightFighterInformations arg1)
			: this()
		{
			initGameFightShowFighterMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5864;
		}
		
		public GameFightShowFighterMessage initGameFightShowFighterMessage(GameFightFighterInformations arg1 = null)
		{
			this.informations = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.informations = new GameFightFighterInformations();
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
			this.serializeAs_GameFightShowFighterMessage(arg1);
		}
		
		public void serializeAs_GameFightShowFighterMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.informations.getTypeId());
			this.informations.serialize(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameFightShowFighterMessage(arg1);
		}
		
		public void deserializeAs_GameFightShowFighterMessage(BigEndianReader arg1)
		{
			var loc1 = (ushort)arg1.ReadUShort();
			this.informations = ProtocolTypeManager.GetInstance<GameFightFighterInformations>((uint)loc1);
			this.informations.deserialize(arg1);
		}
		
	}
}
