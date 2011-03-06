using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GuildInformationsPaddocksMessage : Message
	{
		public const uint protocolId = 5959;
		internal Boolean _isInitialized = false;
		public uint nbPaddockMax = 0;
		public List<PaddockContentInformations> paddocksInformations;
		
		public GuildInformationsPaddocksMessage()
		{
			this.paddocksInformations = new List<PaddockContentInformations>();
		}
		
		public GuildInformationsPaddocksMessage(uint arg1, List<PaddockContentInformations> arg2)
			: this()
		{
			initGuildInformationsPaddocksMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5959;
		}
		
		public GuildInformationsPaddocksMessage initGuildInformationsPaddocksMessage(uint arg1 = 0, List<PaddockContentInformations> arg2 = null)
		{
			this.nbPaddockMax = arg1;
			this.paddocksInformations = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.nbPaddockMax = 0;
			this.paddocksInformations = new List<PaddockContentInformations>();
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
			this.serializeAs_GuildInformationsPaddocksMessage(arg1);
		}
		
		public void serializeAs_GuildInformationsPaddocksMessage(BigEndianWriter arg1)
		{
			if ( this.nbPaddockMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbPaddockMax + ") on element nbPaddockMax.");
			}
			arg1.WriteByte((byte)this.nbPaddockMax);
			arg1.WriteShort((short)this.paddocksInformations.Count);
			var loc1 = 0;
			while ( loc1 < this.paddocksInformations.Count )
			{
				this.paddocksInformations[loc1].serializeAs_PaddockContentInformations(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GuildInformationsPaddocksMessage(arg1);
		}
		
		public void deserializeAs_GuildInformationsPaddocksMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.nbPaddockMax = (uint)arg1.ReadByte();
			if ( this.nbPaddockMax < 0 )
			{
				throw new Exception("Forbidden value (" + this.nbPaddockMax + ") on element of GuildInformationsPaddocksMessage.nbPaddockMax.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new PaddockContentInformations()) as PaddockContentInformations).deserialize(arg1);
				this.paddocksInformations.Add((PaddockContentInformations)loc3);
				++loc2;
			}
		}
		
	}
}
