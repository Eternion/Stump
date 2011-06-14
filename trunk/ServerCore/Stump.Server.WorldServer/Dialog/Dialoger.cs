
using System;
using Stump.Server.WorldServer.Entities;

namespace Stump.Server.WorldServer.Dialog
{
    public class Dialoger
    {
        public Dialoger(Character character, IDialog dialog)
        {
            Character = character;
            Dialog = dialog;
        }

        public IDialog Dialog
        {
            get;
            private set;
        }

        public Character Character
        {
            get;
            private set;
        }
    }
}