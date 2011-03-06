using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class NpcDialogCreationMessage : Message
	{
		public const uint protocolId = 5618;
		internal Boolean _isInitialized = false;
		public int mapId = 0;
		public int npcId = 0;
		
		public NpcDialogCreationMessage()
		{
		}
		
		public NpcDialogCreationMessage(int arg1, int arg2)
			: this()
		{
			initNpcDialogCreationMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5618;
		}
		
		public NpcDialogCreationMessage initNpcDialogCreationMessage(int arg1 = 0, int arg2 = 0)
		{
			this.mapId = arg1;
			this.npcId = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.mapId = 0;
			this.npcId = 0;
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
			this.serializeAs_NpcDialogCreationMessage(arg1);
		}
		
		public void serializeAs_NpcDialogCreationMessage(BigEndianWriter arg1)
		{
			arg1.WriteInt((int)this.mapId);
			arg1.WriteInt((int)this.npcId);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_NpcDialogCreationMessage(arg1);
		}
		
		public void deserializeAs_NpcDialogCreationMessage(BigEndianReader arg1)
		{
			this.mapId = (int)arg1.ReadInt();
			this.npcId = (int)arg1.ReadInt();
		}
		
	}
}
