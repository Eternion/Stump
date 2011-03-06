using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class JobCrafterDirectoryDefineSettingsMessage : Message
	{
		public const uint protocolId = 5649;
		internal Boolean _isInitialized = false;
		public JobCrafterDirectorySettings settings;
		
		public JobCrafterDirectoryDefineSettingsMessage()
		{
			this.settings = new JobCrafterDirectorySettings();
		}
		
		public JobCrafterDirectoryDefineSettingsMessage(JobCrafterDirectorySettings arg1)
			: this()
		{
			initJobCrafterDirectoryDefineSettingsMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 5649;
		}
		
		public JobCrafterDirectoryDefineSettingsMessage initJobCrafterDirectoryDefineSettingsMessage(JobCrafterDirectorySettings arg1 = null)
		{
			this.settings = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.settings = new JobCrafterDirectorySettings();
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
			this.serializeAs_JobCrafterDirectoryDefineSettingsMessage(arg1);
		}
		
		public void serializeAs_JobCrafterDirectoryDefineSettingsMessage(BigEndianWriter arg1)
		{
			this.settings.serializeAs_JobCrafterDirectorySettings(arg1);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_JobCrafterDirectoryDefineSettingsMessage(arg1);
		}
		
		public void deserializeAs_JobCrafterDirectoryDefineSettingsMessage(BigEndianReader arg1)
		{
			this.settings = new JobCrafterDirectorySettings();
			this.settings.deserialize(arg1);
		}
		
	}
}
