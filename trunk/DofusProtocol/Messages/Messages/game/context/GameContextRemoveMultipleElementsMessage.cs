using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameContextRemoveMultipleElementsMessage : Message
	{
		public const uint protocolId = 252;
		internal Boolean _isInitialized = false;
		public List<int> id;
		
		public GameContextRemoveMultipleElementsMessage()
		{
			this.id = new List<int>();
		}
		
		public GameContextRemoveMultipleElementsMessage(List<int> arg1)
			: this()
		{
			initGameContextRemoveMultipleElementsMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 252;
		}
		
		public GameContextRemoveMultipleElementsMessage initGameContextRemoveMultipleElementsMessage(List<int> arg1)
		{
			this.id = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.id = new List<int>();
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
			this.serializeAs_GameContextRemoveMultipleElementsMessage(arg1);
		}
		
		public void serializeAs_GameContextRemoveMultipleElementsMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.id.Count);
			var loc1 = 0;
			while ( loc1 < this.id.Count )
			{
				arg1.WriteInt((int)this.id[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameContextRemoveMultipleElementsMessage(arg1);
		}
		
		public void deserializeAs_GameContextRemoveMultipleElementsMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = arg1.ReadInt();
				this.id.Add((int)loc3);
				++loc2;
			}
		}
		
	}
}
