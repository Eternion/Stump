using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class AuthenticationTicketWithClientPacksMessage : AuthenticationTicketMessage
	{
		public const uint protocolId = 6190;
		internal Boolean _isInitialized = false;
		public List<uint> packs;
		
		public AuthenticationTicketWithClientPacksMessage()
		{
			this.packs = new List<uint>();
		}
		
		public AuthenticationTicketWithClientPacksMessage(String arg1, String arg2, List<uint> arg3)
			: this()
		{
			initAuthenticationTicketWithClientPacksMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6190;
		}
		
		public AuthenticationTicketWithClientPacksMessage initAuthenticationTicketWithClientPacksMessage(String arg1 = "", String arg2 = "", List<uint> arg3 = null)
		{
			base.initAuthenticationTicketMessage(arg1, arg2);
			this.packs = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.packs = new List<uint>();
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
			this.serializeAs_AuthenticationTicketWithClientPacksMessage(arg1);
		}
		
		public void serializeAs_AuthenticationTicketWithClientPacksMessage(BigEndianWriter arg1)
		{
			base.serializeAs_AuthenticationTicketMessage(arg1);
			arg1.WriteShort((short)this.packs.Count);
			var loc1 = 0;
			while ( loc1 < this.packs.Count )
			{
				if ( this.packs[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.packs[loc1] + ") on element 1 (starting at 1) of packs.");
				}
				arg1.WriteInt((int)this.packs[loc1]);
				++loc1;
			}
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AuthenticationTicketWithClientPacksMessage(arg1);
		}
		
		public void deserializeAs_AuthenticationTicketWithClientPacksMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			base.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadInt()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of packs.");
				}
				this.packs.Add((uint)loc3);
				++loc2;
			}
		}
		
	}
}
