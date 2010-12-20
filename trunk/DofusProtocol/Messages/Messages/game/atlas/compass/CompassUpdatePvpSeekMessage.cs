using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class CompassUpdatePvpSeekMessage : CompassUpdateMessage
	{
		public const uint protocolId = 6013;
		internal Boolean _isInitialized = false;
		public uint memberId = 0;
		public String memberName = "";
		
		public CompassUpdatePvpSeekMessage()
		{
		}
		
		public CompassUpdatePvpSeekMessage(uint arg1, int arg2, int arg3, uint arg4, String arg5)
			: this()
		{
			initCompassUpdatePvpSeekMessage(arg1, arg2, arg3, arg4, arg5);
		}
		
		public override uint getMessageId()
		{
			return 6013;
		}
		
		public CompassUpdatePvpSeekMessage initCompassUpdatePvpSeekMessage(uint arg1 = 0, int arg2 = 0, int arg3 = 0, uint arg4 = 0, String arg5 = "")
		{
			base.initCompassUpdateMessage(arg1, arg2, arg3);
			this.memberId = arg4;
			this.memberName = arg5;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			base.reset();
			this.memberId = 0;
			this.memberName = "";
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
			this.serializeAs_CompassUpdatePvpSeekMessage(arg1);
		}
		
		public void serializeAs_CompassUpdatePvpSeekMessage(BigEndianWriter arg1)
		{
			base.serializeAs_CompassUpdateMessage(arg1);
			if ( this.memberId < 0 )
			{
				throw new Exception("Forbidden value (" + this.memberId + ") on element memberId.");
			}
			arg1.WriteInt((int)this.memberId);
			arg1.WriteUTF((string)this.memberName);
		}
		
		public override void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_CompassUpdatePvpSeekMessage(arg1);
		}
		
		public void deserializeAs_CompassUpdatePvpSeekMessage(BigEndianReader arg1)
		{
			base.deserialize(arg1);
			this.memberId = (uint)arg1.ReadInt();
			if ( this.memberId < 0 )
			{
				throw new Exception("Forbidden value (" + this.memberId + ") on element of CompassUpdatePvpSeekMessage.memberId.");
			}
			this.memberName = (String)arg1.ReadUTF();
		}
		
	}
}
