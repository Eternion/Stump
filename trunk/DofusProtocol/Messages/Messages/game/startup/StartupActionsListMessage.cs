using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class StartupActionsListMessage : Message
	{
		public const uint protocolId = 1301;
		internal Boolean _isInitialized = false;
		public List<StartupActionAddObject> actions;
		
		public StartupActionsListMessage()
		{
			this.actions = new List<StartupActionAddObject>();
		}
		
		public StartupActionsListMessage(List<StartupActionAddObject> arg1)
			: this()
		{
			initStartupActionsListMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 1301;
		}
		
		public StartupActionsListMessage initStartupActionsListMessage(List<StartupActionAddObject> arg1)
		{
			this.actions = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.actions = new List<StartupActionAddObject>();
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
			this.serializeAs_StartupActionsListMessage(arg1);
		}
		
		public void serializeAs_StartupActionsListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.actions.Count);
			var loc1 = 0;
			while ( loc1 < this.actions.Count )
			{
				this.actions[loc1].serializeAs_StartupActionAddObject(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StartupActionsListMessage(arg1);
		}
		
		public void deserializeAs_StartupActionsListMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new StartupActionAddObject()) as StartupActionAddObject).deserialize(arg1);
				this.actions.Add((StartupActionAddObject)loc3);
				++loc2;
			}
		}
		
	}
}
