using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Worlds.Chat;

namespace Stump.Server.WorldServer.Worlds.Actors.RolePlay
{
    public abstract class NamedActor : RolePlayActor
    {
        #region Network

        public virtual string Name
        {
            get;
            protected set;
        }

        public override GameContextActorInformations GetGameContextActorInformations()
        {
            return new GameRolePlayNamedActorInformations(Id, Look, GetEntityDispositionInformations(), Name);
        }
        #endregion

        #region Actions

        #region Chat

        public void Say(string message)
        {
            Say(ChannelId.General, message);
        }

        public void Say(ChannelId channel, string message)
        {
            if (ChatManager.Instance.ChatHandlers.Length <= (int) channel)
                return;

            ChatManager.ChatParserDelegate handler = ChatManager.Instance.ChatHandlers[(int) channel];

            Context.DoForAll(entry => handler(entry.Client, channel, message));
        }

        #endregion
        
        #endregion
    }
}