using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeGuildTaxCollectorGetMessage : Message
	{
		public const uint protocolId = 5762;
		internal Boolean _isInitialized = false;
		public String collectorName = "";
		public int worldX = 0;
		public int worldY = 0;
		public uint mapId = 0;
		public String userName = "";
		public double experience = 0;
		public List<ObjectItemQuantity> objectsInfos;
		
		public ExchangeGuildTaxCollectorGetMessage()
		{
			this.@objectsInfos = new List<ObjectItemQuantity>();
		}
		
		public ExchangeGuildTaxCollectorGetMessage(String arg1, int arg2, int arg3, uint arg4, String arg5, double arg6, List<ObjectItemQuantity> arg7)
			: this()
		{
			initExchangeGuildTaxCollectorGetMessage(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
		}
		
		public override uint getMessageId()
		{
			return 5762;
		}
		
		public ExchangeGuildTaxCollectorGetMessage initExchangeGuildTaxCollectorGetMessage(String arg1 = "", int arg2 = 0, int arg3 = 0, uint arg4 = 0, String arg5 = "", double arg6 = 0, List<ObjectItemQuantity> arg7 = null)
		{
			this.collectorName = arg1;
			this.worldX = arg2;
			this.worldY = arg3;
			this.mapId = arg4;
			this.userName = arg5;
			this.experience = arg6;
			this.@objectsInfos = arg7;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.collectorName = "";
			this.worldX = 0;
			this.worldY = 0;
			this.mapId = 0;
			this.userName = "";
			this.experience = 0;
			this.@objectsInfos = new List<ObjectItemQuantity>();
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
			this.serializeAs_ExchangeGuildTaxCollectorGetMessage(arg1);
		}
		
		public void serializeAs_ExchangeGuildTaxCollectorGetMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.collectorName);
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element worldX.");
			}
			arg1.WriteShort((short)this.worldX);
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element worldY.");
			}
			arg1.WriteShort((short)this.worldY);
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element mapId.");
			}
			arg1.WriteInt((int)this.mapId);
			arg1.WriteUTF((string)this.userName);
			arg1.WriteDouble(this.experience);
			arg1.WriteShort((short)this.@objectsInfos.Count);
			var loc1 = 0;
			while ( loc1 < this.@objectsInfos.Count )
			{
				this.@objectsInfos[loc1].serializeAs_ObjectItemQuantity(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeGuildTaxCollectorGetMessage(arg1);
		}
		
		public void deserializeAs_ExchangeGuildTaxCollectorGetMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			this.collectorName = (String)arg1.ReadUTF();
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of ExchangeGuildTaxCollectorGetMessage.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of ExchangeGuildTaxCollectorGetMessage.worldY.");
			}
			this.mapId = (uint)arg1.ReadInt();
			if ( this.mapId < 0 )
			{
				throw new Exception("Forbidden value (" + this.mapId + ") on element of ExchangeGuildTaxCollectorGetMessage.mapId.");
			}
			this.userName = (String)arg1.ReadUTF();
			this.experience = (double)arg1.ReadDouble();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItemQuantity()) as ObjectItemQuantity).deserialize(arg1);
				this.@objectsInfos.Add((ObjectItemQuantity)loc3);
				++loc2;
			}
		}
		
	}
}
