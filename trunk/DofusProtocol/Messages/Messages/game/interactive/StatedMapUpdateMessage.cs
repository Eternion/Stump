using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class StatedMapUpdateMessage : Message
	{
		public const uint protocolId = 5716;
		internal Boolean _isInitialized = false;
		public List<StatedElement> statedElements;
		
		public StatedMapUpdateMessage()
		{
			this.statedElements = new List<StatedElement>();
		}
		
		public StatedMapUpdateMessage(List<StatedElement> arg1)
			: this()
		{
			initStatedMapUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5716;
		}
		
		public StatedMapUpdateMessage initStatedMapUpdateMessage(List<StatedElement> arg1)
		{
			this.statedElements = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.statedElements = new List<StatedElement>();
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
			this.serializeAs_StatedMapUpdateMessage(arg1);
		}
		
		public void serializeAs_StatedMapUpdateMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.statedElements.Count);
			var loc1 = 0;
			while ( loc1 < this.statedElements.Count )
			{
				this.statedElements[loc1].serializeAs_StatedElement(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StatedMapUpdateMessage(arg1);
		}
		
		public void deserializeAs_StatedMapUpdateMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new StatedElement()) as StatedElement).deserialize(arg1);
				this.statedElements.Add((StatedElement)loc3);
				++loc2;
			}
		}
		
	}
}
