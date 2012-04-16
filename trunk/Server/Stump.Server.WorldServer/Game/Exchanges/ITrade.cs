using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Dialogs;

namespace Stump.Server.WorldServer.Game.Exchanges
{
    public interface ITrade : IDialog
    {
        int Id { get; }

        ExchangeTypeEnum Type
        {
            get;
        }
    }
}