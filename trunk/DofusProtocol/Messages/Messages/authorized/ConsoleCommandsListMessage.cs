using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class ConsoleCommandsListMessage : Message
	{
		public const uint protocolId = 6127;
		internal Boolean _isInitialized = false;
		public List<String> aliases;
		public List<String> arguments;
		public List<String> descriptions;
		
		public ConsoleCommandsListMessage()
		{
			this.aliases = new List<String>();
			this.arguments = new List<String>();
			this.descriptions = new List<String>();
		}
		
		public ConsoleCommandsListMessage(List<String> arg1, List<String> arg2, List<String> arg3)
			: this()
		{
			initConsoleCommandsListMessage(arg1, arg2, arg3);
		}
		
		public override uint getMessageId()
		{
			return 6127;
		}
		
		public ConsoleCommandsListMessage initConsoleCommandsListMessage(List<String> arg1, List<String> arg2, List<String> arg3)
		{
			this.aliases = arg1;
			this.arguments = arg2;
			this.descriptions = arg3;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.aliases = new List<String>();
			this.arguments = new List<String>();
			this.descriptions = new List<String>();
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
			this.serializeAs_ConsoleCommandsListMessage(arg1);
		}
		
		public void serializeAs_ConsoleCommandsListMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.aliases.Count);
			var loc1 = 0;
			while ( loc1 < this.aliases.Count )
			{
				arg1.WriteUTF((string)this.aliases[loc1]);
				++loc1;
			}
			arg1.WriteShort((short)arguments.Count);
			var loc2 = 0;
			while ( loc2 < arguments.Count )
			{
				arg1.WriteUTF((string)arguments[loc2]);
				++loc2;
			}
			arg1.WriteShort((short)this.descriptions.Count);
			var loc3 = 0;
			while ( loc3 < this.descriptions.Count )
			{
				arg1.WriteUTF((string)this.descriptions[loc3]);
				++loc3;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_ConsoleCommandsListMessage(arg1);
		}
		
		public void deserializeAs_ConsoleCommandsListMessage(BigEndianReader arg1)
		{
			object loc7 = null;
			object loc8 = null;
			object loc9 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				loc7 = arg1.ReadUTF();
				this.aliases.Add((String)loc7);
				++loc2;
			}
			var loc3 = (ushort)arg1.ReadUShort();
			var loc4 = 0;
			while ( loc4 < loc3 )
			{
				loc8 = arg1.ReadUTF();
				arguments.Add((String)loc8);
				++loc4;
			}
			var loc5 = (ushort)arg1.ReadUShort();
			var loc6 = 0;
			while ( loc6 < loc5 )
			{
				loc9 = arg1.ReadUTF();
				this.descriptions.Add((String)loc9);
				++loc6;
			}
		}
		
	}
}
