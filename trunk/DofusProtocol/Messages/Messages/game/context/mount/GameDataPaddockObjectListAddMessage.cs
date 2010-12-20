using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameDataPaddockObjectListAddMessage : Message
	{
		public const uint protocolId = 5992;
		internal Boolean _isInitialized = false;
		public List<PaddockItem> paddockItemDescription;
		
		public GameDataPaddockObjectListAddMessage()
		{
			this.paddockItemDescription = new List<PaddockItem>();
		}
		
		public GameDataPaddockObjectListAddMessage(List<PaddockItem> arg1)
			: this()
		{
			initGameDataPaddockObjectListAddMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5992;
		}
		
		public GameDataPaddockObjectListAddMessage initGameDataPaddockObjectListAddMessage(List<PaddockItem> arg1)
		{
			this.paddockItemDescription = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.paddockItemDescription = new List<PaddockItem>();
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
			this.serializeAs_GameDataPaddockObjectListAddMessage(arg1);
		}
		
		public void serializeAs_GameDataPaddockObjectListAddMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.paddockItemDescription.Count);
			var loc1 = 0;
			while ( loc1 < this.paddockItemDescription.Count )
			{
				this.paddockItemDescription[loc1].serializeAs_PaddockItem(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameDataPaddockObjectListAddMessage(arg1);
		}
		
		public void deserializeAs_GameDataPaddockObjectListAddMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new PaddockItem()) as PaddockItem).deserialize(arg1);
				this.paddockItemDescription.Add((PaddockItem)loc3);
				++loc2;
			}
		}
		
	}
}
