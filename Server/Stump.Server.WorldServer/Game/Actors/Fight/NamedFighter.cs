using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Actors.Interfaces;
using Stump.Server.WorldServer.Game.Fights.Teams;
using Stump.Server.WorldServer.Handlers.Chat;

namespace Stump.Server.WorldServer.Game.Actors.Fight
{
    public abstract class NamedFighter : FightActor, INamedActor
    {
        protected NamedFighter(FightTeam team)
            : base(team)
        {
        }

        public abstract string Name
        {
            get;
        }

        public void Say(string msg)
        {
            ChatHandler.SendChatServerMessage(Fight.Clients, this, ChatActivableChannelsEnum.CHANNEL_GLOBAL, msg);
        }

        public override string GetMapRunningFighterName()
        {
            return Name;
        }
    }
}