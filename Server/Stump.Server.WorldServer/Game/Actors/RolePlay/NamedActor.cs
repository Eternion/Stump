using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Types;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Chat;

namespace Stump.Server.WorldServer.Game.Actors.RolePlay
{
    public abstract class NamedActor : RolePlayActor, INamedActor
    {
        #region Network

        public virtual string Name
        {
            get;
            protected set;
        }

        public override GameContextActorInformations GetGameContextActorInformations(Character character)
        {
            return new GameRolePlayNamedActorInformations(Id, Look.GetEntityLook(), GetEntityDispositionInformations(), Name);
        }
        #endregion

        #region Actions

        #region Chat

        public void Say(string msg)
        {
            ChatHandler.SendChatServerMessage(CharacterContainer.Clients, this, ChatActivableChannelsEnum.CHANNEL_GLOBAL, msg);
        }

        #endregion
        
        #endregion
    }
}