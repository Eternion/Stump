using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class EmotePlayMassiveMessage : EmotePlayAbstractMessage
	{
		public const uint protocolId = 5691;
		internal Boolean _isInitialized = false;
		public List<int> actorIds;
		
		public EmotePlayMassiveMessage()
		{
			this.actorIds = new List<int>();
		}
		
		public EmotePlayMassiveMessage(uint arg1, uint arg2, List<int> arg3)
			: this()
		{
			initEmotePlayMassiveMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5691;
		}
		
		public EmotePlayMassiveMessage initEmotePlayMassiveMessage(uint arg1 = 0, uint arg2 = 0, List<int> arg3 = null)
		{
			base.initEmotePlayAbstractMessage(arg1, arg2);
			this.actorIds = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.actorIds = new List<int>();
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
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_EmotePlayMassiveMessage(arg1);
		}
		
		public void serializeAs_EmotePlayMassiveMessage(BigEndianWriter arg1)
		{
			base.serializeAs_EmotePlayAbstractMessage(arg1);
			arg1.WriteShort((short)this.actorIds.Count);
			var loc1 = 0;
			while ( loc1 < this.actorIds.Count )
			{
				arg1.WriteInt((int)this.actorIds[loc1]);
				++loc1;
			}
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_EmotePlayMassiveMessage(arg1);
		}
		
		public void deserializeAs_EmotePlayMassiveMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			base.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = arg1.ReadInt();
				this.actorIds.Add((int)loc3);
				++loc2;
			}
		}
		
	}
}
