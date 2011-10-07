// File generated by 'DofusProtocolBuilder.exe v1.0.0.0'
// From 'StartupActionsObjetAttributionMessage.xml' the '03/10/2011 12:47:10'
using System;
using Stump.Core.IO;

namespace Stump.DofusProtocol.Messages
{
	public class StartupActionsObjetAttributionMessage : Message
	{
		public const uint Id = 1303;
		public override uint MessageId
		{
			get
			{
				return 1303;
			}
		}
		
		public int actionId;
		public int characterId;
		
		public StartupActionsObjetAttributionMessage()
		{
		}
		
		public StartupActionsObjetAttributionMessage(int actionId, int characterId)
		{
			this.actionId = actionId;
			this.characterId = characterId;
		}
		
		public override void Serialize(IDataWriter writer)
		{
			writer.WriteInt(actionId);
			writer.WriteInt(characterId);
		}
		
		public override void Deserialize(IDataReader reader)
		{
			actionId = reader.ReadInt();
			if ( actionId < 0 )
			{
				throw new Exception("Forbidden value on actionId = " + actionId + ", it doesn't respect the following condition : actionId < 0");
			}
			characterId = reader.ReadInt();
			if ( characterId < 0 )
			{
				throw new Exception("Forbidden value on characterId = " + characterId + ", it doesn't respect the following condition : characterId < 0");
			}
		}
	}
}
