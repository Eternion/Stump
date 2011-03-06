using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class SpellForgottenMessage : Message
	{
		public const uint protocolId = 5834;
		internal Boolean _isInitialized = false;
		public List<uint> spellsId;
		public uint boostPoint = 0;
		
		public SpellForgottenMessage()
		{
			this.spellsId = new List<uint>();
		}
		
		public SpellForgottenMessage(List<uint> arg1, uint arg2)
			: this()
		{
			initSpellForgottenMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5834;
		}
		
		public SpellForgottenMessage initSpellForgottenMessage(List<uint> arg1, uint arg2 = 0)
		{
			this.spellsId = arg1;
			this.boostPoint = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.spellsId = new List<uint>();
			this.boostPoint = 0;
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
			this.serializeAs_SpellForgottenMessage(arg1);
		}
		
		public void serializeAs_SpellForgottenMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.spellsId.Count);
			var loc1 = 0;
			while ( loc1 < this.spellsId.Count )
			{
				if ( this.spellsId[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.spellsId[loc1] + ") on element 1 (starting at 1) of spellsId.");
				}
				arg1.WriteShort((short)this.spellsId[loc1]);
				++loc1;
			}
			if ( this.boostPoint < 0 )
			{
				throw new Exception("Forbidden value (" + this.boostPoint + ") on element boostPoint.");
			}
			arg1.WriteShort((short)this.boostPoint);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SpellForgottenMessage(arg1);
		}
		
		public void deserializeAs_SpellForgottenMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of spellsId.");
				}
				this.spellsId.Add((uint)loc3);
				++loc2;
			}
			this.boostPoint = (uint)arg1.ReadShort();
			if ( this.boostPoint < 0 )
			{
				throw new Exception("Forbidden value (" + this.boostPoint + ") on element of SpellForgottenMessage.boostPoint.");
			}
		}
		
	}
}
