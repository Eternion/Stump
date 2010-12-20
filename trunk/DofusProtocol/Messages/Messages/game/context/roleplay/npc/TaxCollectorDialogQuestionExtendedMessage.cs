using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class TaxCollectorDialogQuestionExtendedMessage : TaxCollectorDialogQuestionBasicMessage
	{
		public const uint protocolId = 5615;
		internal Boolean _isInitialized = false;
		public uint pods = 0;
		public uint prospecting = 0;
		public uint wisdom = 0;
		public uint taxCollectorsCount = 0;
		
		public TaxCollectorDialogQuestionExtendedMessage()
		{
		}
		
		public TaxCollectorDialogQuestionExtendedMessage(BasicGuildInformations arg1, uint arg2, uint arg3, uint arg4, uint arg5)
			: this()
		{
			initTaxCollectorDialogQuestionExtendedMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 5615;
		}
		
		public TaxCollectorDialogQuestionExtendedMessage initTaxCollectorDialogQuestionExtendedMessage(BasicGuildInformations arg1 = null, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0, uint arg5 = 0)
		{
			base.initTaxCollectorDialogQuestionBasicMessage(arg1);
			this.pods = arg2;
			this.prospecting = arg3;
			this.wisdom = arg4;
			this.taxCollectorsCount = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.pods = 0;
			this.prospecting = 0;
			this.wisdom = 0;
			this.taxCollectorsCount = 0;
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
			this.serializeAs_TaxCollectorDialogQuestionExtendedMessage(arg1);
		}
		
		public void serializeAs_TaxCollectorDialogQuestionExtendedMessage(BigEndianWriter arg1)
		{
			base.serializeAs_TaxCollectorDialogQuestionBasicMessage(arg1);
			if ( this.pods < 0 )
			{
				throw new Exception("Forbidden value (" + this.pods + ") on element pods.");
			}
			arg1.WriteShort((short)this.pods);
			if ( this.prospecting < 0 )
			{
				throw new Exception("Forbidden value (" + this.prospecting + ") on element prospecting.");
			}
			arg1.WriteShort((short)this.prospecting);
			if ( this.wisdom < 0 )
			{
				throw new Exception("Forbidden value (" + this.wisdom + ") on element wisdom.");
			}
			arg1.WriteShort((short)this.wisdom);
			if ( this.taxCollectorsCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorsCount + ") on element taxCollectorsCount.");
			}
			arg1.WriteByte((byte)this.taxCollectorsCount);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_TaxCollectorDialogQuestionExtendedMessage(arg1);
		}
		
		public void deserializeAs_TaxCollectorDialogQuestionExtendedMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.pods = (uint)arg1.ReadShort();
			if ( this.pods < 0 )
			{
				throw new Exception("Forbidden value (" + this.pods + ") on element of TaxCollectorDialogQuestionExtendedMessage.pods.");
			}
			this.prospecting = (uint)arg1.ReadShort();
			if ( this.prospecting < 0 )
			{
				throw new Exception("Forbidden value (" + this.prospecting + ") on element of TaxCollectorDialogQuestionExtendedMessage.prospecting.");
			}
			this.wisdom = (uint)arg1.ReadShort();
			if ( this.wisdom < 0 )
			{
				throw new Exception("Forbidden value (" + this.wisdom + ") on element of TaxCollectorDialogQuestionExtendedMessage.wisdom.");
			}
			this.taxCollectorsCount = (uint)arg1.ReadByte();
			if ( this.taxCollectorsCount < 0 )
			{
				throw new Exception("Forbidden value (" + this.taxCollectorsCount + ") on element of TaxCollectorDialogQuestionExtendedMessage.taxCollectorsCount.");
			}
		}
		
	}
}
