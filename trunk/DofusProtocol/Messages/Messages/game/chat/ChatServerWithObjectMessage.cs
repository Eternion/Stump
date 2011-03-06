using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChatServerWithObjectMessage : ChatServerMessage
	{
		public const uint protocolId = 883;
		internal Boolean _isInitialized = false;
		public List<ObjectItem> objects;
		
		public ChatServerWithObjectMessage()
		{
			this.@objects = new List<ObjectItem>();
		}
		
		public ChatServerWithObjectMessage(uint arg1, String arg2, uint arg3, String arg4, int arg5, String arg6, int arg7, List<ObjectItem> arg8)
			: this()
		{
			initChatServerWithObjectMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
		}
		
		public override uint getMessageId()
		{
			return 883;
		}
		
		public ChatServerWithObjectMessage initChatServerWithObjectMessage(uint arg1 = 0, String arg2 = "", uint arg3 = 0, String arg4 = "", int arg5 = 0, String arg6 = "", int arg7 = 0, List<ObjectItem> arg8 = null)
		{
			base.initChatServerMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
			this.@objects = arg8;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.@objects = new List<ObjectItem>();
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
			this.serializeAs_ChatServerWithObjectMessage(arg1);
		}
		
		public void serializeAs_ChatServerWithObjectMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ChatServerMessage(arg1);
			arg1.WriteShort((short)this.@objects.Count);
			var loc1 = 0;
			while ( loc1 < this.@objects.Count )
			{
				this.@objects[loc1].serializeAs_ObjectItem(arg1);
				++loc1;
			}
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ChatServerWithObjectMessage(arg1);
		}
		
		public void deserializeAs_ChatServerWithObjectMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			base.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItem()) as ObjectItem).deserialize(arg1);
				this.@objects.Add((ObjectItem)loc3);
				++loc2;
			}
		}
		
	}
}
