using System.Collections.Generic;
using System.Collections.ObjectModel;
using Stump.Server.WorldServer.Game.Actors.RolePlay;
using Stump.Server.WorldServer.Game.Items;

namespace Stump.Server.WorldServer.Game.Exchanges
{
    public delegate void ItemMovedHandler(ITrader trader, PlayerItem item, bool modified, int difference);
    public delegate void KamasChangedHandler(ITrader trader, uint kamasAmount);
    public delegate void ReadyStatusChangedHandler(ITrader trader, bool isReady);

    public interface ITrader
    {
        event ItemMovedHandler ItemMoved;
        event KamasChangedHandler KamasChanged;
        event ReadyStatusChangedHandler ReadyStatusChanged;

        ITrade Trade { get; }
        RolePlayActor Actor { get; }
        ReadOnlyCollection<PlayerItem> Items { get; }
        uint Kamas { get; }
        bool ReadyToApply { get; }

        bool MoveItem(int guid, int amount);
        bool SetKamas(uint amount);
        void ToggleReady();
        void ToggleReady(bool status);
    }
}