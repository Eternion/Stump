using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameContextMoveElementMessage : Message
	{
		public const uint protocolId = 253;
		internal Boolean _isInitialized = false;
		public EntityMovementInformations movement;
		
		public GameContextMoveElementMessage()
		{
			this.movement = new EntityMovementInformations();
		}
		
		public GameContextMoveElementMessage(EntityMovementInformations arg1)
			: this()
		{
			initGameContextMoveElementMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 253;
		}
		
		public GameContextMoveElementMessage initGameContextMoveElementMessage(EntityMovementInformations arg1 = null)
		{
			this.movement = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.movement = new EntityMovementInformations();
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
			this.serializeAs_GameContextMoveElementMessage(arg1);
		}
		
		public void serializeAs_GameContextMoveElementMessage(BigEndianWriter arg1)
		{
			this.movement.serializeAs_EntityMovementInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameContextMoveElementMessage(arg1);
		}
		
		public void deserializeAs_GameContextMoveElementMessage(BigEndianReader arg1)
		{
			this.movement = new EntityMovementInformations();
			this.movement.deserialize(arg1);
		}
		
	}
}
