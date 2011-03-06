using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameEntitiesDispositionMessage : Message
	{
		public const uint protocolId = 5696;
		internal Boolean _isInitialized = false;
		public List<IdentifiedEntityDispositionInformations> dispositions;
		
		public GameEntitiesDispositionMessage()
		{
			this.dispositions = new List<IdentifiedEntityDispositionInformations>();
		}
		
		public GameEntitiesDispositionMessage(List<IdentifiedEntityDispositionInformations> arg1)
			: this()
		{
			initGameEntitiesDispositionMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5696;
		}
		
		public GameEntitiesDispositionMessage initGameEntitiesDispositionMessage(List<IdentifiedEntityDispositionInformations> arg1)
		{
			this.dispositions = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.dispositions = new List<IdentifiedEntityDispositionInformations>();
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
			this.serializeAs_GameEntitiesDispositionMessage(arg1);
		}
		
		public void serializeAs_GameEntitiesDispositionMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.dispositions.Count);
			var loc1 = 0;
			while ( loc1 < this.dispositions.Count )
			{
				arg1.WriteShort((short)this.dispositions[loc1].getTypeId());
				this.dispositions[loc1].serialize(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameEntitiesDispositionMessage(arg1);
		}
		
		public void deserializeAs_GameEntitiesDispositionMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			object loc4 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = (ushort)arg1.ReadUShort();
				(( loc4 = ProtocolTypeManager.GetInstance<IdentifiedEntityDispositionInformations>((uint)loc3)) as IdentifiedEntityDispositionInformations).deserialize(arg1);
				this.dispositions.Add((IdentifiedEntityDispositionInformations)loc4);
				++loc2;
			}
		}
		
	}
}
