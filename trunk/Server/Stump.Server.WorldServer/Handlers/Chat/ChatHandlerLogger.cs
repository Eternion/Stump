using System;
using System.Globalization;
using MongoDB.Bson;
using Stump.DofusProtocol.Messages;
using Stump.Server.BaseServer;
using Stump.Server.WorldServer.Core.Network;
using Stump.Server.WorldServer.Game;

namespace Stump.Server.WorldServer.Handlers.Chat
{
    public class ChatHandlerLogger : ChatHandler
    {
        [WorldHandler(ChatClientPrivateMessage.Id)]
        public override bool HandleChatClientPrivateMessage(WorldClient client, ChatClientPrivateMessage message)
        {
            if (!base.HandleChatClientPrivateMessage(client, message))
                return false;

            var chr = World.Instance.GetCharacter(message.receiver);

            var document = new BsonDocument
                    {
                        { "SenderId", client.Character.Id },
                        { "ReceiverId", chr.Id },
                        { "Message", message.content },
                        { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
                    };

            ServerBase.MongoLogger.Insert("PrivateMSG", document);

            return true;
        }

        [WorldHandler(ChatClientMultiMessage.Id)]
        public override void HandleChatClientMultiMessage(WorldClient client, ChatClientMultiMessage message)
        {
            var document = new BsonDocument
                    {
                        { "SenderId", client.Character.Id },
                        { "Message", message.content },
                        { "Date", DateTime.Now.ToString(CultureInfo.InvariantCulture) }
                    };

            ServerBase.MongoLogger.Insert("MultiMessage", document);
        }
    }
}
