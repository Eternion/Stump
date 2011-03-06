using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class SetUpdateMessage : Message
	{
		public const uint protocolId = 5503;
		internal Boolean _isInitialized = false;
		public uint setId = 0;
		public List<uint> setObjects;
		public List<ObjectEffect> setEffects;
		
		public SetUpdateMessage()
		{
			this.setObjects = new List<uint>();
			this.setEffects = new List<ObjectEffect>();
		}
		
		public SetUpdateMessage(uint arg1, List<uint> arg2, List<ObjectEffect> arg3)
			: this()
		{
			initSetUpdateMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 5503;
		}
		
		public SetUpdateMessage initSetUpdateMessage(uint arg1 = 0, List<uint> arg2 = null, List<ObjectEffect> arg3 = null)
		{
			this.setId = arg1;
			this.setObjects = arg2;
			this.setEffects = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.setId = 0;
			this.setObjects = new List<uint>();
			this.setEffects = new List<ObjectEffect>();
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
			this.serializeAs_SetUpdateMessage(arg1);
		}
		
		public void serializeAs_SetUpdateMessage(BigEndianWriter arg1)
		{
			if ( this.setId < 0 )
			{
				throw new Exception("Forbidden value (" + this.setId + ") on element setId.");
			}
			arg1.WriteShort((short)this.setId);
			arg1.WriteShort((short)this.setObjects.Count);
			var loc1 = 0;
			while ( loc1 < this.setObjects.Count )
			{
				if ( this.setObjects[loc1] < 0 )
				{
					throw new Exception("Forbidden value (" + this.setObjects[loc1] + ") on element 2 (starting at 1) of setObjects.");
				}
				arg1.WriteShort((short)this.setObjects[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)this.setEffects.Count);
			var loc2 = 0;
			while ( loc2 < this.setEffects.Count )
			{
				arg1.WriteShort((short)this.setEffects[loc2].getTypeId());
				this.setEffects[loc2].serialize(arg1);
				++loc2;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_SetUpdateMessage(arg1);
		}
		
		public void deserializeAs_SetUpdateMessage(BigEndianReader arg1)
		{
			var loc5 = 0;
			var loc6 = 0;
			object loc7 = null;
			this.setId = (uint)arg1.ReadShort();
			if ( this.setId < 0 )
			{
				throw new Exception("Forbidden value (" + this.setId + ") on element of SetUpdateMessage.setId.");
			}
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				if ( (loc5 = arg1.ReadShort()) < 0 )
				{
					throw new Exception("Forbidden value (" + loc5 + ") on elements of setObjects.");
				}
				this.setObjects.Add((uint)loc5);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				loc6 = (ushort)arg1.ReadUShort();
				(( loc7 = ProtocolTypeManager.GetInstance<ObjectEffect>((uint)loc6)) as ObjectEffect).deserialize(arg1);
				this.setEffects.Add((ObjectEffect)loc7);
				++loc4;
			}
		}
		
	}
}
