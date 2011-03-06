using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ChatServerCopyWithObjectMessage : ChatServerCopyMessage
	{
		public const uint protocolId = 884;
		internal Boolean _isInitialized = false;
		public List<ObjectItem> objects;
		
		public ChatServerCopyWithObjectMessage()
		{
			this.@objects = new List<ObjectItem>();
		}
		
		public ChatServerCopyWithObjectMessage(uint arg1, String arg2, uint arg3, String arg4, uint arg5, String arg6, List<ObjectItem> arg7)
			: this()
		{
			initChatServerCopyWithObjectMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getMessageId()
		{
			return 884;
		}
		
		public ChatServerCopyWithObjectMessage initChatServerCopyWithObjectMessage(uint arg1 = 0, String arg2 = "", uint arg3 = 0, String arg4 = "", uint arg5 = 0, String arg6 = "", List<ObjectItem> arg7 = null)
		{
			base.initChatServerCopyMessage(arg1, arg2, arg3, arg4, arg5, arg6);
			this.@objects = arg7;
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
			this.serializeAs_ChatServerCopyWithObjectMessage(arg1);
		}
		
		public void serializeAs_ChatServerCopyWithObjectMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ChatServerCopyMessage(arg1);
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
			this.deserializeAs_ChatServerCopyWithObjectMessage(arg1);
		}
		
		public void deserializeAs_ChatServerCopyWithObjectMessage(BigEndianReader arg1)
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
