using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CharacterReplayWithRecolorRequestMessage : CharacterReplayRequestMessage
	{
		public const uint protocolId = 6111;
		internal Boolean _isInitialized = false;
		public List<int> indexedColor;
		
		public CharacterReplayWithRecolorRequestMessage()
		{
			this.indexedColor = new List<int>();
		}
		
		public CharacterReplayWithRecolorRequestMessage(uint arg1, List<int> arg2)
			: this()
		{
			initCharacterReplayWithRecolorRequestMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6111;
		}
		
		public CharacterReplayWithRecolorRequestMessage initCharacterReplayWithRecolorRequestMessage(uint arg1 = 0, List<int> arg2 = null)
		{
			base.initCharacterReplayRequestMessage(arg1);
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
			this.serializeAs_CharacterReplayWithRecolorRequestMessage(arg1);
		}
		
		public void serializeAs_CharacterReplayWithRecolorRequestMessage(BigEndianWriter arg1)
		{
			base.serializeAs_CharacterReplayRequestMessage(arg1);
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
			this.deserializeAs_CharacterReplayWithRecolorRequestMessage(arg1);
		}
		
		public void deserializeAs_CharacterReplayWithRecolorRequestMessage(BigEndianReader arg1)
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
