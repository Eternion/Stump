// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameActionFightDispellEffectMessage.xml' the '03/10/2011 12:46:53'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameActionFightDispellEffectMessage : GameActionFightDispellMessage
	{
		public const uint Id = 6113;
		public override uint MessageId
		{
			get
			{
				return 6113;
			}
		}
		
		public int boostUID;
		
		public GameActionFightDispellEffectMessage()
		{
		}
		
		public GameActionFightDispellEffectMessage(short actionId, int sourceId, int targetId, int boostUID)
			 : base(actionId, sourceId, targetId)
		{
			this.boostUID = boostUID;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteInt(boostUID);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			boostUID = reader.ReadInt();
			if ( boostUID < 0 )
			{
				throw new Exception("Forbidden value on boostUID = " + boostUID + ", it doesn't respect the following condition : boostUID < 0");
			}
		}
	}
}
