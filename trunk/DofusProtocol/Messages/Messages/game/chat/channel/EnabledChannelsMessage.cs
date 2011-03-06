using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class EnabledChannelsMessage : Message
	{
		public const uint protocolId = 892;
		internal Boolean _isInitialized = false;
		public List<uint> channels;
		public List<uint> disallowed;
		
		public EnabledChannelsMessage()
		{
			this.channels = new List<uint>();
			this.disallowed = new List<uint>();
		}
		
		public EnabledChannelsMessage(List<uint> arg1, List<uint> arg2)
			: this()
		{
			initEnabledChannelsMessage(arg1, arg2);
		}
		
		public override uint getMessageId()
		{
			return 892;
		}
		
		public EnabledChannelsMessage initEnabledChannelsMessage(List<uint> arg1, List<uint> arg2)
		{
			this.channels = arg1;
			this.disallowed = arg2;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.channels = new List<uint>();
			this.disallowed = new List<uint>();
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
			this.serializeAs_EnabledChannelsMessage(arg1);
		}
		
		public void serializeAs_EnabledChannelsMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.channels.Count);
			var loc1 = 0;
			while ( loc1 < this.channels.Count )
			{
				arg1.WriteByte((byte)this.channels[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.disallowed.Count);
			var loc2 = 0;
			while ( loc2 < this.disallowed.Count )
			{
				arg1.WriteByte((byte)this.disallowed[loc2]);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_EnabledChannelsMessage(arg1);
		}
		
		public void deserializeAs_EnabledChannelsMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			var loc6 = 0;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc5 = arg1.ReadByte()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc5 + ") on elements of channels.");
				}
				this.channels.Add((uint)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				if ( (loc6 = arg1.ReadByte()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc6 + ") on elements of disallowed.");
				}
				this.disallowed.Add((uint)loc6);
				++loc4;
			}
		}
		
	}
}
