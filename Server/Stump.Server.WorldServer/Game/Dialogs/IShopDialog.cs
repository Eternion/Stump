namespace Stump.Server.WorldServer.Game.Dialogs
{
    public interface IShopDialog : IDialog
    {
        bool BuyItem(int id, int quantity);
        bool SellItem(int id, int quantity);
    }
}