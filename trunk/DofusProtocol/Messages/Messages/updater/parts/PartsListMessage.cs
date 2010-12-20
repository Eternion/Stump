using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class PartsListMessage : Message
	{
		public const uint protocolId = 1502;
		internal Boolean _isInitialized = false;
		public List<ContentPart> parts;
		
		public PartsListMessage()
		{
			this.parts = new List<ContentPart>();
		}
		
		public PartsListMessage(List<ContentPart> arg1)
			: this()
		{
			initPartsListMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 1502;
		}
		
		public PartsListMessage initPartsListMessage(List<ContentPart> arg1)
		{
			this.parts = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.parts = new List<ContentPart>();
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
			this.serializeAs_PartsListMessage(arg1);
		}
		
		public void serializeAs_PartsListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.parts.Count);
			var loc1 = 0;
			while ( loc1 < this.parts.Count )
			{
				this.parts[loc1].serializeAs_ContentPart(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_PartsListMessage(arg1);
		}
		
		public void deserializeAs_PartsListMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ContentPart()) as ContentPart).deserialize(arg1);
				this.parts.Add((ContentPart)loc3);
				++loc2;
			}
		}
		
	}
}
