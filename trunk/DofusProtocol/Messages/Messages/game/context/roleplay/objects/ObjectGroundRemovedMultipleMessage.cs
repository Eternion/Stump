using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ObjectGroundRemovedMultipleMessage : Message
	{
		public const uint protocolId = 5944;
		internal Boolean _isInitialized = false;
		public List<uint> cells;
		
		public ObjectGroundRemovedMultipleMessage()
		{
			this.cells = new List<uint>();
		}
		
		public ObjectGroundRemovedMultipleMessage(List<uint> arg1)
			: this()
		{
			initObjectGroundRemovedMultipleMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5944;
		}
		
		public ObjectGroundRemovedMultipleMessage initObjectGroundRemovedMultipleMessage(List<uint> arg1)
		{
			this.cells = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.cells = new List<uint>();
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
			this.serializeAs_ObjectGroundRemovedMultipleMessage(arg1);
		}
		
		public void serializeAs_ObjectGroundRemovedMultipleMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.cells.Count);
			var loc1 = 0;
			while ( loc1 < this.cells.Count )
			{
				if ( this.cells[loc1] < 0 || this.cells[loc1] > 559 )
				{
					throw new Exception("Forbidden value (" + this.cells[loc1] + ") on element 1 (starting at 1) of cells.");
				}
				arg1.WriteShort((short)this.cells[loc1]);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ObjectGroundRemovedMultipleMessage(arg1);
		}
		
		public void deserializeAs_ObjectGroundRemovedMultipleMessage(BigEndianReader arg1)
		{
			var loc3 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc3 = arg1.ReadShort()) < 0 || loc3 > 559 )
				{
					throw new Exception("Forbidden value (" + loc3 + ") on elements of cells.");
				}
				this.cells.Add((uint)loc3);
				++loc2;
			}
		}
		
	}
}
