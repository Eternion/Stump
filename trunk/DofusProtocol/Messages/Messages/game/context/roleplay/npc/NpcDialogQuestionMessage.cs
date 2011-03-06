using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class NpcDialogQuestionMessage : Message
	{
		public const uint protocolId = 5617;
		internal Boolean _isInitialized = false;
		public uint messageId = 0;
		public List<String> dialogParams;
		public List<uint> visibleReplies;
		
		public NpcDialogQuestionMessage()
		{
			this.dialogParams = new List<String>();
			this.visibleReplies = new List<uint>();
		}
		
		public NpcDialogQuestionMessage(uint arg1, List<String> arg2, List<uint> arg3)
			: this()
		{
			initNpcDialogQuestionMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5617;
		}
		
		public NpcDialogQuestionMessage initNpcDialogQuestionMessage(uint arg1 = 0, List<String> arg2 = null, List<uint> arg3 = null)
		{
			this.messageId = arg1;
			this.dialogParams = arg2;
			this.visibleReplies = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.messageId = 0;
			this.dialogParams = new List<String>();
			this.visibleReplies = new List<uint>();
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
			this.serializeAs_NpcDialogQuestionMessage(arg1);
		}
		
		public void serializeAs_NpcDialogQuestionMessage(BigEndianWriter arg1)
		{
			if ( this.messageId < 0 )
			{
				throw new Exception("Forbidden value (" + this.messageId + ") on element messageId.");
			}
			arg1.WriteShort((short)this.messageId);
			arg1.WriteShort((short)this.dialogParams.Count);
			var loc1 = 0;
			while ( loc1 < this.dialogParams.Count )
			{
				arg1.WriteUTF((string)this.dialogParams[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.visibleReplies.Count);
			var loc2 = 0;
			while ( loc2 < this.visibleReplies.Count )
			{
				if ( this.visibleReplies[loc2] < 0 )
				{
					throw new Exception("Forbidden value (" + this.visibleReplies[loc2] + ") on element 3 (starting at 1) of visibleReplies.");
				}
				arg1.WriteShort((short)this.visibleReplies[loc2]);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_NpcDialogQuestionMessage(arg1);
		}
		
		public void deserializeAs_NpcDialogQuestionMessage(BigEndianReader arg1)
		{
			object loc5 = null;
			var loc6 = 0;
			this.messageId = (uint)arg1.ReadShort();
			if ( this.messageId < 0 )
			{
				throw new Exception("Forbidden value (" + this.messageId + ") on element of NpcDialogQuestionMessage.messageId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc5 = arg1.ReadUTF();
				this.dialogParams.Add((String)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				if ( (loc6 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc6 + ") on elements of visibleReplies.");
				}
				this.visibleReplies.Add((uint)loc6);
				++loc4;
			}
		}
		
	}
}
