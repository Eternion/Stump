
using System;

namespace Stump.Server.WorldServer.Handlers
{
    public static class PredicatesDefinitions
    {
        public static readonly Predicate<WorldClient> HasChoosenCharacter = entry => entry.ActiveCharacter != null;

        public static readonly Predicate<WorldClient> IsMoving =
            entry => HasChoosenCharacter(entry) && entry.ActiveCharacter.IsMoving;

        public static readonly Predicate<WorldClient> IsFighting =
            entry => HasChoosenCharacter(entry) && entry.ActiveCharacter.IsInFight;

        public static readonly Predicate<WorldClient> IsDialoging =
            entry => HasChoosenCharacter(entry) && entry.ActiveCharacter.IsInDialog;

        public static readonly Predicate<WorldClient> IsTrading =
            entry => HasChoosenCharacter(entry) && entry.ActiveCharacter.IsInTrade;

        public static readonly Predicate<WorldClient> IsDialogingWithNpc =
            entry => HasChoosenCharacter(entry) && entry.ActiveCharacter.IsInDialogWithNpc;

        public static readonly Predicate<WorldClient> IsDialogRequested =
            entry => HasChoosenCharacter(entry) && entry.ActiveCharacter.IsDialogRequested;
    }
}