using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ShortcutBarContentMessage : Message
	{
		public const uint protocolId = 6231;
		internal Boolean _isInitialized = false;
		public uint barType = 0;
		public List<Shortcut> shortcuts;
		
		public ShortcutBarContentMessage()
		{
			this.shortcuts = new List<Shortcut>();
		}
		
		public ShortcutBarContentMessage(uint arg1, List<Shortcut> arg2)
			: this()
		{
			initShortcutBarContentMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6231;
		}
		
		public ShortcutBarContentMessage initShortcutBarContentMessage(uint arg1 = 0, List<Shortcut> arg2 = null)
		{
			this.barType = arg1;
			this.shortcuts = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.barType = 0;
			this.shortcuts = new List<Shortcut>();
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
			this.serializeAs_ShortcutBarContentMessage(arg1);
		}
		
		public void serializeAs_ShortcutBarContentMessage(BigEndianWriter arg1)
		{
			arg1.WriteByte((byte)this.barType);
			arg1.WriteShort((short)this.shortcuts.Count);
			var loc1 = 0;
			while ( loc1 < this.shortcuts.Count )
			{
				arg1.WriteShort((short)this.shortcuts[loc1].getTypeId());
				this.shortcuts[loc1].serialize(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ShortcutBarContentMessage(arg1);
		}
		
		public void deserializeAs_ShortcutBarContentMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			object loc4 = null;
			this.barType = (uint)arg1.ReadByte();
			if ( this.barType < 0 )
			{
				throw new Exception("Forbidden value (" + this.barType + ") on element of ShortcutBarContentMessage.barType.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = (ushort)arg1.ReadUShort();
				(( loc4 = ProtocolTypeManager.GetInstance<Shortcut>((uint)loc3)) as Shortcut).deserialize(arg1);
				this.shortcuts.Add((Shortcut)loc4);
				++loc2;
			}
		}
		
	}
}
