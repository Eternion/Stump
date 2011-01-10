// /*************************************************************************
//  *
//  *  Copyright (C) 2010 - 2011 Stump Team
//  *
//  *  This program is free software: you can redistribute it and/or modify
//  *  it under the terms of the GNU General Public License as published by
//  *  the Free Software Foundation, either version 3 of the License, or
//  *  (at your option) any later version.
//  *
//  *  This program is distributed in the hope that it will be useful,
//  *  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  *  GNU General Public License for more details.
//  *
//  *  You should have received a copy of the GNU General Public License
//  *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
//  *
//  *************************************************************************/
using System;

namespace Stump.Server.WorldServer.Handlers
{
    public static class PredicatesDefinitions
    {
        public static readonly Predicate<WorldClient> HasChoosenCharacter = entry => entry.ActiveCharacter != null;

        public static readonly Predicate<WorldClient> IsMoving = entry => HasChoosenCharacter(entry) && entry.ActiveCharacter.IsMoving;
        public static readonly Predicate<WorldClient> IsFighting = entry => HasChoosenCharacter(entry) && entry.ActiveCharacter.IsInFight;
        public static readonly Predicate<WorldClient> IsDialoging = entry => HasChoosenCharacter(entry) && entry.ActiveCharacter.IsInDialog;
        public static readonly Predicate<WorldClient> IsTrading = entry => HasChoosenCharacter(entry) && entry.ActiveCharacter.IsInTrade;
        public static readonly Predicate<WorldClient> IsDialogingWithNpc = entry => HasChoosenCharacter(entry) && entry.ActiveCharacter.IsInDialogWithNpc;

        public static readonly Predicate<WorldClient> IsDialogRequested =
            entry => HasChoosenCharacter(entry) && entry.ActiveCharacter.IsDialogRequested;
    }
}