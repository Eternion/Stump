using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeStartOkMountMessage : ExchangeStartOkMountWithOutPaddockMessage
	{
		public const uint protocolId = 5979;
		internal Boolean _isInitialized = false;
		public List<MountClientData> paddockedMountsDescription;
		
		public ExchangeStartOkMountMessage()
		{
			this.paddockedMountsDescription = new List<MountClientData>();
		}
		
		public ExchangeStartOkMountMessage(List<MountClientData> arg1, List<MountClientData> arg2)
			: this()
		{
			initExchangeStartOkMountMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 5979;
		}
		
		public ExchangeStartOkMountMessage initExchangeStartOkMountMessage(List<MountClientData> arg1, List<MountClientData> arg2)
		{
			base.initExchangeStartOkMountWithOutPaddockMessage(arg1);
			this.paddockedMountsDescription = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.paddockedMountsDescription = new List<MountClientData>();
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
			this.serializeAs_ExchangeStartOkMountMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartOkMountMessage(BigEndianWriter arg1)
		{
			base.serializeAs_ExchangeStartOkMountWithOutPaddockMessage(arg1);
			arg1.WriteShort((short)this.paddockedMountsDescription.Count);
			var loc1 = 0;
			while ( loc1 < this.paddockedMountsDescription.Count )
			{
				this.paddockedMountsDescription[loc1].serializeAs_MountClientData(arg1);
				++loc1;
			}
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartOkMountMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartOkMountMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			base.deserialize(arg1);
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new MountClientData()) as MountClientData).deserialize(arg1);
				this.paddockedMountsDescription.Add((MountClientData)loc3);
				++loc2;
			}
		}
		
	}
}
