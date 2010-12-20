using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameDataPaddockObjectRemoveMessage : Message
	{
		public const uint protocolId = 5993;
		internal Boolean _isInitialized = false;
		public uint cellId = 0;
		
		public GameDataPaddockObjectRemoveMessage()
		{
		}
		
		public GameDataPaddockObjectRemoveMessage(uint arg1)
			: this()
		{
			initGameDataPaddockObjectRemoveMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5993;
		}
		
		public GameDataPaddockObjectRemoveMessage initGameDataPaddockObjectRemoveMessage(uint arg1 = 0)
		{
			this.cellId = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.cellId = 0;
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
			this.serializeAs_GameDataPaddockObjectRemoveMessage(arg1);
		}
		
		public void serializeAs_GameDataPaddockObjectRemoveMessage(BigEndianWriter arg1)
		{
			if ( this.cellId < 0 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element cellId.");
			}
			arg1.WriteShort((short)this.cellId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameDataPaddockObjectRemoveMessage(arg1);
		}
		
		public void deserializeAs_GameDataPaddockObjectRemoveMessage(BigEndianReader arg1)
		{
			this.cellId = (uint)arg1.ReadShort();
			if ( this.cellId < 0 || this.cellId > 559 )
			{
				throw new Exception("Forbidden value (" + this.cellId + ") on element of GameDataPaddockObjectRemoveMessage.cellId.");
			}
		}
		
	}
}
