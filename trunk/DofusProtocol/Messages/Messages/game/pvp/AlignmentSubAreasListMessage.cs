using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class AlignmentSubAreasListMessage : Message
	{
		public const uint protocolId = 6059;
		internal Boolean _isInitialized = false;
		public List<int> angelsSubAreas;
		public List<int> evilsSubAreas;
		
		public AlignmentSubAreasListMessage()
		{
			this.angelsSubAreas = new List<int>();
			this.evilsSubAreas = new List<int>();
		}
		
		public AlignmentSubAreasListMessage(List<int> arg1, List<int> arg2)
			: this()
		{
			initAlignmentSubAreasListMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 6059;
		}
		
		public AlignmentSubAreasListMessage initAlignmentSubAreasListMessage(List<int> arg1, List<int> arg2)
		{
			this.angelsSubAreas = arg1;
			this.evilsSubAreas = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.angelsSubAreas = new List<int>();
			this.evilsSubAreas = new List<int>();
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
			this.serializeAs_AlignmentSubAreasListMessage(arg1);
		}
		
		public void serializeAs_AlignmentSubAreasListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.angelsSubAreas.Count);
			var loc1 = 0;
			while ( loc1 < this.angelsSubAreas.Count )
			{
				arg1.WriteShort((short)this.angelsSubAreas[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.evilsSubAreas.Count);
			var loc2 = 0;
			while ( loc2 < this.evilsSubAreas.Count )
			{
				arg1.WriteShort((short)this.evilsSubAreas[loc2]);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_AlignmentSubAreasListMessage(arg1);
		}
		
		public void deserializeAs_AlignmentSubAreasListMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			var loc6 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc5 = arg1.ReadShort();
				this.angelsSubAreas.Add((int)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				loc6 = arg1.ReadShort();
				this.evilsSubAreas.Add((int)loc6);
				++loc4;
			}
		}
		
	}
}
