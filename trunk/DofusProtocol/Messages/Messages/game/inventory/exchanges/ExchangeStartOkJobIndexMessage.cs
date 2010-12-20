using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ExchangeStartOkJobIndexMessage : Message
	{
		public const uint protocolId = 5819;
		internal Boolean _isInitialized = false;
		public List<uint> jobs;
		
		public ExchangeStartOkJobIndexMessage()
		{
			this.jobs = new List<uint>();
		}
		
		public ExchangeStartOkJobIndexMessage(List<uint> arg1)
			: this()
		{
			initExchangeStartOkJobIndexMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5819;
		}
		
		public ExchangeStartOkJobIndexMessage initExchangeStartOkJobIndexMessage(List<uint> arg1)
		{
			this.jobs = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.jobs = new List<uint>();
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
			this.serializeAs_ExchangeStartOkJobIndexMessage(arg1);
		}
		
		public void serializeAs_ExchangeStartOkJobIndexMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.jobs.Count);
			var loc1 = 0;
			while ( loc1 < this.jobs.Count )
			{
				if ( this.jobs[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.jobs[loc1] + ") on element 1 (starting at 1) of jobs.");
				}
				arg1.WriteInt((int)this.jobs[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ExchangeStartOkJobIndexMessage(arg1);
		}
		
		public void deserializeAs_ExchangeStartOkJobIndexMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadInt()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of jobs.");
				}
				this.jobs.Add((uint)loc3);
				++loc2;
			}
		}
		
	}
}
