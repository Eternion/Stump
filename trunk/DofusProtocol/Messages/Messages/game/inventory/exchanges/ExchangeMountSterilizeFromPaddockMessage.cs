using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeMountSterilizeFromPaddockMessage : Message
	{
		public const uint protocolId = 6056;
		internal Boolean _isInitialized = false;
		public String name = "";
		public int worldX = 0;
		public int worldY = 0;
		public String sterilizator = "";
		
		public ExchangeMountSterilizeFromPaddockMessage()
		{
		}
		
		public ExchangeMountSterilizeFromPaddockMessage(String arg1, int arg2, int arg3, String arg4)
			: this()
		{
			initExchangeMountSterilizeFromPaddockMessage(arg1, arg2, arg3, arg4);
		}
		
		public override uint getMessageId()
		{
			return 6056;
		}
		
		public ExchangeMountSterilizeFromPaddockMessage initExchangeMountSterilizeFromPaddockMessage(String arg1 = "", int arg2 = 0, int arg3 = 0, String arg4 = "")
		{
			this.name = arg1;
			this.worldX = arg2;
			this.worldY = arg3;
			this.sterilizator = arg4;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.name = "";
			this.worldX = 0;
			this.worldY = 0;
			this.sterilizator = "";
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
			this.serializeAs_ExchangeMountSterilizeFromPaddockMessage(arg1);
		}
		
		public void serializeAs_ExchangeMountSterilizeFromPaddockMessage(BigEndianWriter arg1)
		{
			arg1.WriteUTF((string)this.name);
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
			arg1.WriteUTF((string)this.sterilizator);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeMountSterilizeFromPaddockMessage(arg1);
		}
		
		public void deserializeAs_ExchangeMountSterilizeFromPaddockMessage(BigEndianReader arg1)
		{
			this.name = (String)arg1.ReadUTF();
			this.worldX = (int)arg1.ReadShort();
			if ( this.worldX < -255 || this.worldX > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldX + ") on element of ExchangeMountSterilizeFromPaddockMessage.worldX.");
			}
			this.worldY = (int)arg1.ReadShort();
			if ( this.worldY < -255 || this.worldY > 255 )
			{
				throw new Exception("Forbidden value (" + this.worldY + ") on element of ExchangeMountSterilizeFromPaddockMessage.worldY.");
			}
			this.sterilizator = (String)arg1.ReadUTF();
		}
		
	}
}
