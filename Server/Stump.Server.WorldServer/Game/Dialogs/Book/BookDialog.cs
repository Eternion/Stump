using Stump.DofusProtocol.Enums;
using Stump.DofusProtocol.Messages;
using Stump.Server.WorldServer.Game.Actors.RolePlay.Characters;
using Stump.Server.WorldServer.Handlers.Dialogs;

namespace Stump.Server.WorldServer.Game.Dialogs.Book
{
    public class BookDialog : IDialog
    {
        public BookDialog(Character character, short bookId)
        {
            Character = character;
            BookId = bookId;
        }

        public DialogTypeEnum DialogType => DialogTypeEnum.DIALOG_BOOK;

        public Character Character
        {
            get;
        }

        public short BookId
        {
            get;
        }

        public void Open()
        {
            Character.SetDialog(this);
            Character.Client.Send(new DocumentReadingBeginMessage(BookId));
        }

        public void Close()
        {
            Character.CloseDialog(this);
            DialogHandler.SendLeaveDialogMessage(Character.Client, DialogType);
        }
    }
}
