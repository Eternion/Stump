using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class InteractiveMapUpdateMessage : Message
	{
		public const uint protocolId = 5002;
		internal Boolean _isInitialized = false;
		public List<InteractiveElement> interactiveElements;
		
		public InteractiveMapUpdateMessage()
		{
			this.interactiveElements = new List<InteractiveElement>();
		}
		
		public InteractiveMapUpdateMessage(List<InteractiveElement> arg1)
			: this()
		{
			initInteractiveMapUpdateMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5002;
		}
		
		public InteractiveMapUpdateMessage initInteractiveMapUpdateMessage(List<InteractiveElement> arg1)
		{
			this.interactiveElements = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.interactiveElements = new List<InteractiveElement>();
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
			this.serializeAs_InteractiveMapUpdateMessage(arg1);
		}
		
		public void serializeAs_InteractiveMapUpdateMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.interactiveElements.Count);
			var loc1 = 0;
			while ( loc1 < this.interactiveElements.Count )
			{
				this.interactiveElements[loc1].serializeAs_InteractiveElement(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_InteractiveMapUpdateMessage(arg1);
		}
		
		public void deserializeAs_InteractiveMapUpdateMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new InteractiveElement()) as InteractiveElement).deserialize(arg1);
				this.interactiveElements.Add((InteractiveElement)loc3);
				++loc2;
			}
		}
		
	}
}
