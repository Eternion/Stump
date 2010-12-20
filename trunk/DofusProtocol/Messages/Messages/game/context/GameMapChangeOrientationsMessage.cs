using System;
using System.Collections.Generic;
using Stump.BaseCore.Framework.Utils;
using Stump.BaseCore.Framework.IO;
using Stump.DofusProtocol.Classes;
namespace Stump.DofusProtocol.Messages
{
	
	public class GameMapChangeOrientationsMessage : Message
	{
		public const uint protocolId = 6155;
		internal Boolean _isInitialized = false;
		public List<ActorOrientation> orientations;
		
		public GameMapChangeOrientationsMessage()
		{
			this.orientations = new List<ActorOrientation>();
		}
		
		public GameMapChangeOrientationsMessage(List<ActorOrientation> arg1)
			: this()
		{
			initGameMapChangeOrientationsMessage(arg1);
		}
		
		public override uint getMessageId()
		{
			return 6155;
		}
		
		public GameMapChangeOrientationsMessage initGameMapChangeOrientationsMessage(List<ActorOrientation> arg1)
		{
			this.orientations = arg1;
			this._isInitialized = true;
			return this;
		}
		
		public override void reset()
		{
			this.orientations = new List<ActorOrientation>();
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
			this.serializeAs_GameMapChangeOrientationsMessage(arg1);
		}
		
		public void serializeAs_GameMapChangeOrientationsMessage(BigEndianWriter arg1)
		{
			arg1.WriteShort((short)this.orientations.Count);
			var loc1 = 0;
			while ( loc1 < this.orientations.Count )
			{
				this.orientations[loc1].serializeAs_ActorOrientation(arg1);
				++loc1;
			}
		}
		
		public virtual void deserialize(BigEndianReader arg1)
		{
			this.deserializeAs_GameMapChangeOrientationsMessage(arg1);
		}
		
		public void deserializeAs_GameMapChangeOrientationsMessage(BigEndianReader arg1)
		{
			object loc3 = null;
			var loc1 = (ushort)arg1.ReadUShort();
			var loc2 = 0;
			while ( loc2 < loc1 )
			{
				((loc3 = new ActorOrientation()) as ActorOrientation).deserialize(arg1);
				this.orientations.Add((ActorOrientation)loc3);
				++loc2;
			}
		}
		
	}
}
