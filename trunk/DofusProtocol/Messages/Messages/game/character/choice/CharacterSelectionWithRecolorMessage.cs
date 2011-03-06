using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CharacterSelectionWithRecolorMessage : CharacterSelectionMessage
	{
		public const uint protocolId = 6075;
		internal Boolean _isInitialized = false;
		public List<int> indexedColor;
		
		public CharacterSelectionWithRecolorMessage()
		{
			this.indexedColor = new List<int>();
		}
		
		public CharacterSelectionWithRecolorMessage(int arg1, List<int> arg2)
			: this()
		{
			initCharacterSelectionWithRecolorMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6075;
		}
		
		public CharacterSelectionWithRecolorMessage initCharacterSelectionWithRecolorMessage(int arg1 = 0, List<int> arg2 = null)
		{
			base.initCharacterSelectionMessage(arg1);
			this.indexedColor = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.indexedColor = new List<int>();
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
			this.serializeAs_CharacterSelectionWithRecolorMessage(arg1);
		}
		
		public void serializeAs_CharacterSelectionWithRecolorMessage(BigEndianWriter arg1)
		{
			base.serializeAs_CharacterSelectionMessage(arg1);
			arg1.WriteShort((short)this.indexedColor.Count);
			var loc1 = 0;
			while ( loc1 < this.indexedColor.Count )
			{
				arg1.WriteInt((int)this.indexedColor[loc1]);
				++loc1;
			}
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CharacterSelectionWithRecolorMessage(arg1);
		}
		
		public void deserializeAs_CharacterSelectionWithRecolorMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			base.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc3 = arg1.ReadInt();
				this.indexedColor.Add((int)loc3);
				++loc2;
			}
		}
		
	}
}
