using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameMapChangeOrientationMessage : Message
	{
		public const uint protocolId = 946;
		internal Boolean _isInitialized = false;
		public ActorOrientation orientation;
		
		public GameMapChangeOrientationMessage()
		{
			this.orientation = new ActorOrientation();
		}
		
		public GameMapChangeOrientationMessage(ActorOrientation arg1)
			: this()
		{
			initGameMapChangeOrientationMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 946;
		}
		
		public GameMapChangeOrientationMessage initGameMapChangeOrientationMessage(ActorOrientation arg1 = null)
		{
			this.orientation = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.orientation = new ActorOrientation();
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
			this.serializeAs_GameMapChangeOrientationMessage(arg1);
		}
		
		public void serializeAs_GameMapChangeOrientationMessage(BigEndianWriter arg1)
		{
			this.orientation.serializeAs_ActorOrientation(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameMapChangeOrientationMessage(arg1);
		}
		
		public void deserializeAs_GameMapChangeOrientationMessage(BigEndianReader arg1)
		{
			this.orientation = new ActorOrientation();
			this.orientation.deserialize(arg1);
		}
		
	}
}
