using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
namespace Stump.DofusProtocol.Classes
{
	
	public class Version : Object
	{
		public const uint protocolId = 11;
		public uint major = 0;
		public uint minor = 0;
		public uint release = 0;
		public uint revision = 0;
		public uint patch = 0;
		public uint buildType = 0;
		
		public Version()
		{
		}
		
		public Version(uint arg1, uint arg2, uint arg3, uint arg4, uint arg5, uint arg6)
			: this()
		{
			initVersion(arg1, arg2, arg3, arg4, arg5, arg6);
		}
		
		public virtual uint getTypeId()
		{
			return 11;
		}
		
		public Version initVersion(uint arg1 = 0, uint arg2 = 0, uint arg3 = 0, uint arg4 = 0, uint arg5 = 0, uint arg6 = 0)
		{
			this.major = arg1;
			this.minor = arg2;
			this.release = arg3;
			this.revision = arg4;
			this.patch = arg5;
			this.buildType = arg6;
			return this;
		}
		
		public virtual void reset()
		{
			this.major = 0;
			this.minor = 0;
			this.release = 0;
			this.revision = 0;
			this.patch = 0;
			this.buildType = 0;
		}
		
		public virtual void serialize(BigEndianWriter arg1)
		{
			this.serializeAs_Version(arg1);
		}
		
		public void serializeAs_Version(BigEndianWriter arg1)
		{
			if ( this.major < 0 )
			{
				throw new Exception("Forbidden value (" + this.major + ") on element major.");
			}
			arg1.WriteByte((byte)this.major);
			if ( this.minor < 0 )
			{
				throw new Exception("Forbidden value (" + this.minor + ") on element minor.");
			}
			arg1.WriteByte((byte)this.minor);
			if ( this.release < 0 )
			{
				throw new Exception("Forbidden value (" + this.release + ") on element release.");
			}
			arg1.WriteByte((byte)this.release);
			if ( this.revision < 0 || this.revision > 65535 )
			{
				throw new Exception("Forbidden value (" + this.revision + ") on element revision.");
			}
			arg1.WriteShort((short)this.revision);
			if ( this.patch < 0 )
			{
				throw new Exception("Forbidden value (" + this.patch + ") on element patch.");
			}
			arg1.WriteByte((byte)this.patch);
			arg1.WriteByte((byte)this.buildType);
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_Version(arg1);
		}
		
		public void deserializeAs_Version(BigEndianReader arg1)
		{
			this.major = (uint)arg1.ReadByte();
			if ( this.major < 0 )
			{
				throw new Exception("Forbidden value (" + this.major + ") on element of Stump.DofusProtocol.Classes.Stump.DofusProtocol.Classes.Version.major.");
			}
			this.minor = (uint)arg1.ReadByte();
			if ( this.minor < 0 )
			{
				throw new Exception("Forbidden value (" + this.minor + ") on element of Stump.DofusProtocol.Classes.Stump.DofusProtocol.Classes.Version.minor.");
			}
			this.release = (uint)arg1.ReadByte();
			if ( this.release < 0 )
			{
				throw new Exception("Forbidden value (" + this.release + ") on element of Stump.DofusProtocol.Classes.Stump.DofusProtocol.Classes.Version.release.");
			}
			this.revision = (uint)arg1.ReadUShort();
			if ( this.revision < 0 || this.revision > 65535 )
			{
				throw new Exception("Forbidden value (" + this.revision + ") on element of Stump.DofusProtocol.Classes.Stump.DofusProtocol.Classes.Version.revision.");
			}
			this.patch = (uint)arg1.ReadByte();
			if ( this.patch < 0 )
			{
				throw new Exception("Forbidden value (" + this.patch + ") on element of Stump.DofusProtocol.Classes.Stump.DofusProtocol.Classes.Version.patch.");
			}
			this.buildType = (uint)arg1.ReadByte();
			if ( this.buildType < 0 )
			{
				throw new Exception("Forbidden value (" + this.buildType + ") on element of Stump.DofusProtocol.Classes.Stump.DofusProtocol.Classes.Version.buildType.");
			}
		}
		
	}
}
