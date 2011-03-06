using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class StartupActionAddObject : Object
	{
		public const uint protocolId = 52;
		public uint uid = 0;
		public String title = "";
		public String text = "";
		public String descUrl = "";
		public String pictureUrl = "";
		public List<ObjectItemMinimalInformation> items;
		
		public StartupActionAddObject()
		{
			this.items = new List<ObjectItemMinimalInformation>();
		}
		
		public StartupActionAddObject(uint arg1, String arg2, String arg3, String arg4, String arg5, List<ObjectItemMinimalInformation> arg6)
			: this()
		{
			initStartupActionAddObject(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public virtual uint getTypeId()
		{
			return 52;
		}
		
		public StartupActionAddObject initStartupActionAddObject(uint arg1 = 0, String arg2 = "", String arg3 = "", String arg4 = "", String arg5 = "", List<ObjectItemMinimalInformation> arg6 = null)
		{
			this.uid = arg1;
			this.title = arg2;
			this.text = arg3;
			this.descUrl = arg4;
			this.pictureUrl = arg5;
			this.items = arg6;
			return this;
		}
		
		public virtual void reset()
		{
			this.uid = 0;
			this.title = "";
			this.text = "";
			this.descUrl = "";
			this.pictureUrl = "";
			this.items = new List<ObjectItemMinimalInformation>();
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_StartupActionAddObject(arg1);
		}
		
		public void serializeAs_StartupActionAddObject(BigEndianWriter arg1)
		{
			if ( this.uid < 0 )
			{
				throw new Exception("Forbidden value (" + this.uid + ") on element uid.");
			}
			arg1.WriteInt((int)this.uid);
			arg1.WriteUTF((string)this.title);
			arg1.WriteUTF((string)this.text);
			arg1.WriteUTF((string)this.descUrl);
			arg1.WriteUTF((string)this.pictureUrl);
			arg1.WriteShort((short)this.items.Count);
			var loc1 = 0;
			while ( loc1 < this.items.Count )
			{
				this.items[loc1].serializeAs_ObjectItemMinimalInformation(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_StartupActionAddObject(arg1);
		}
		
		public void deserializeAs_StartupActionAddObject(BigEndianReader arg1)
		{
			object loc3 = null;
			this.uid = (uint)arg1.ReadInt();
			if ( this.uid < 0 )
			{
				throw new Exception("Forbidden value (" + this.uid + ") on element of StartupActionAddObject.uid.");
			}
			this.title = (String)arg1.ReadUTF();
			this.text = (String)arg1.ReadUTF();
			this.descUrl = (String)arg1.ReadUTF();
			this.pictureUrl = (String)arg1.ReadUTF();
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ObjectItemMinimalInformation()) as ObjectItemMinimalInformation).deserialize(arg1);
				this.items.Add((ObjectItemMinimalInformation)loc3);
				++loc2;
			}
		}
		
	}
}
