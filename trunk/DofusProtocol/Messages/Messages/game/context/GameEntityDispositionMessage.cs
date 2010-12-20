using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameEntityDispositionMessage : Message
	{
		public const uint protocolId = 5693;
		internal Boolean _isInitialized = false;
		public IdentifiedEntityDispositionInformations disposition;
		
		public GameEntityDispositionMessage()
		{
			this.disposition = new IdentifiedEntityDispositionInformations();
		}
		
		public GameEntityDispositionMessage(IdentifiedEntityDispositionInformations arg1)
			: this()
		{
			initGameEntityDispositionMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5693;
		}
		
		public GameEntityDispositionMessage initGameEntityDispositionMessage(IdentifiedEntityDispositionInformations arg1 = null)
		{
			this.disposition = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.disposition = new IdentifiedEntityDispositionInformations();
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
			this.serializeAs_GameEntityDispositionMessage(arg1);
		}
		
		public void serializeAs_GameEntityDispositionMessage(BigEndianWriter arg1)
		{
			this.disposition.serializeAs_IdentifiedEntityDispositionInformations(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameEntityDispositionMessage(arg1);
		}
		
		public void deserializeAs_GameEntityDispositionMessage(BigEndianReader arg1)
		{
			this.disposition = new IdentifiedEntityDispositionInformations();
			this.disposition.deserialize(arg1);
		}
		
	}
}
