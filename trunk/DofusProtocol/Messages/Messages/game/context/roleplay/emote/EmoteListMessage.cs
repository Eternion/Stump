using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class EmoteListMessage : Message
	{
		public const uint protocolId = 5689;
		internal Boolean _isInitialized = false;
		public List<uint> emoteIds;
		
		public EmoteListMessage()
		{
			this.emoteIds = new List<uint>();
		}
		
		public EmoteListMessage(List<uint> arg1)
			: this()
		{
			initEmoteListMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5689;
		}
		
		public EmoteListMessage initEmoteListMessage(List<uint> arg1)
		{
			this.emoteIds = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.emoteIds = new List<uint>();
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
			this.serializeAs_EmoteListMessage(arg1);
		}
		
		public void serializeAs_EmoteListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.emoteIds.Count);
			var loc1 = 0;
			while ( loc1 < this.emoteIds.Count )
			{
				if ( this.emoteIds[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.emoteIds[loc1] + ") on element 1 (starting at 1) of emoteIds.");
				}
				arg1.WriteByte((byte)this.emoteIds[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_EmoteListMessage(arg1);
		}
		
		public void deserializeAs_EmoteListMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadByte()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of emoteIds.");
				}
				this.emoteIds.Add((uint)loc3);
				++loc2;
			}
		}
		
	}
}
