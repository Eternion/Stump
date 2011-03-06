using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class ShortcutObjectPreset : ShortcutObject
	{
		public const uint protocolId = 370;
		public uint presetId = 0;
		
		public ShortcutObjectPreset()
		{
		}
		
		public ShortcutObjectPreset(uint arg1, uint arg2)
			: this()
		{
			initShortcutObjectPreset(arg1, arg2);
		}
		
		public override uint getTypeId()
		{
			return 370;
		}
		
		public ShortcutObjectPreset initShortcutObjectPreset(uint arg1 = 0, uint arg2 = 0)
		{
			base.initShortcutObject(arg1);
			this.presetId = arg2;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.presetId = 0;
		}
		
		public override void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_ShortcutObjectPreset(arg1);
		}
		
		public void serializeAs_ShortcutObjectPreset(BigEndianWriter arg1)
		{
			base.serializeAs_ShortcutObject(arg1);
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element presetId.");
			}
			arg1.WriteByte((byte)this.presetId);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ShortcutObjectPreset(arg1);
		}
		
		public void deserializeAs_ShortcutObjectPreset(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.presetId = (uint)arg1.ReadByte();
			if ( this.presetId < 0 )
			{
				throw new Exception("Forbidden value (" + this.presetId + ") on element of ShortcutObjectPreset.presetId.");
			}
		}
		
	}
}
