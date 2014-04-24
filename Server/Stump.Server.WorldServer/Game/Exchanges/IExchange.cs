using Stump.DofusProtocol.Enums;
using Stump.Server.WorldServer.Game.Dialogs;

namespace Stump.Server.WorldServer.Game.Exchanges
{
    public interface IExchange : IDialog
    {
        ExchangeTypeEnum ExchangeType
        {
            get;
        }
    }
}