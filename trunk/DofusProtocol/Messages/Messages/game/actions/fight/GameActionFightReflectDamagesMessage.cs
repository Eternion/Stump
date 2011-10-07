// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'GameActionFightReflectDamagesMessage.xml' the '03/10/2011 12:46:53'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class GameActionFightReflectDamagesMessage : AbstractGameActionMessage
	{
		public const uint Id = 5530;
		public override uint MessageId
		{
			get
			{
				return 5530;
			}
		}
		
		public int targetId;
		public int amount;
		
		public GameActionFightReflectDamagesMessage()
		{
		}
		
		public GameActionFightReflectDamagesMessage(short actionId, int sourceId, int targetId, int amount)
			 : base(actionId, sourceId)
		{
			this.targetId = targetId;
			this.amount = amount;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			base.Serialize(writer);
			writer.WriteInt(targetId);
			writer.WriteInt(amount);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			base.Deserialize(reader);
			targetId = reader.ReadInt();
			amount = reader.ReadInt();
			if ( amount < 0 )
			{
				throw new Exception("Forbidden value on amount = " + amount + ", it doesn't respect the following condition : amount < 0");
			}
		}
	}
}
