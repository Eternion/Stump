using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class Preset : Object
	{
		public const uint protocolId = 355;
		public uint presetId = 0;
		public uint symbolId = 0;
		public List<PresetItem> objects;
		
		public Preset()
		{
			this.@objects = new List<PresetItem>();
		}
		
		public Preset(uint arg1, uint arg2, List<PresetItem> arg3)
			: this()
		{
			initPreset(arg1, arg2, arg3);
		}
		
		public virtual uint getTypeId()
		{
			return 355;
		}
		
		public Preset initPreset(uint arg1 = 0, uint arg2 = 0, List<PresetItem> arg3 = null)
		{
			this.presetId = arg1;
			this.symbolId = arg2;
			this.@objects = arg3;
			return this;
		}
		
		public virtual void reset()
		{
			this.presetId = 0;
			this.symbolId = 0;
			this.@objects = new List<PresetItem>();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_Preset(arg1);
		}
		
		public void serializeAs_Preset(BigEndianWriter arg1)
		{
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element presetId.");
			}
			arg1.WriteByte((byte)this.presetId);
			if ( this.symbolId < 0 )
			{
				throw new Exception("Forbidden value (" + this.symbolId + ") on element symbolId.");
			}
			arg1.WriteByte((byte)this.symbolId);
			arg1.WriteShort((short)this.@objects.Count);
			var loc1 = 0;
			while ( loc1 < this.@objects.Count )
			{
				this.@objects[loc1].serializeAs_PresetItem(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_Preset(arg1);
		}
		
		public void deserializeAs_Preset(BigEndianReader arg1)
		{
			object loc3 = null;
			this.presetId = (uint)arg1.ReadByte();
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element of Preset.presetId.");
			}
			this.symbolId = (uint)arg1.ReadByte();
			if ( this.symbolId < 0 )
			{
				throw new Exception("Forbidden value (" + this.symbolId + ") on element of Preset.symbolId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new PresetItem()) as PresetItem).deserialize(arg1);
				this.@objects.Add((PresetItem)loc3);
				++loc2;
			}
		}
		
	}
}
