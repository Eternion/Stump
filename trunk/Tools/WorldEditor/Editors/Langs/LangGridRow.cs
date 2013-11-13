#region License GNU GPL

// D2IGridRow.cs
// 
// Copyright (C) 2013 - BehaviorIsManaged
// 
// This program is free software; you can redistribute it and/or modify it 
// under the terms of the GNU General Public License as published by the Free Software Foundation;
// either version 2 of the License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; 
// without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
// See the GNU General Public License for more details. 
// You should have received a copy of the GNU General Public License along with this program; 
// if not, write to the Free Software Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA

#endregion

using System;
using System.ComponentModel;
using Standard;
using Stump.Core.I18N;

namespace WorldEditor.Editors.Langs
{
    public abstract class LangGridRow : INotifyPropertyChanged
    {

        public abstract string French
        {
            get;
            set;
        }


        public abstract string English
        {
            get;
            set;
        }


        public abstract string German
        {
            get;
            set;
        }


        public abstract string Spanish
        {
            get;
            set;
        }


        public abstract string Italian
        {
            get;
            set;
        }


        public abstract string Japanish
        {
            get;
            set;
        }


        public abstract string Dutsh
        {
            get;
            set;
        }


        public abstract string Portugese
        {
            get;
            set;
        }


        public abstract string Russish
        {
            get;
            set;
        }


        public RowState State
        {
            get;
            set;
        }

        public bool DoesContainText(string text)
        {
            return !string.IsNullOrEmpty(French) && French.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                   !string.IsNullOrEmpty(English) && English.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                   !string.IsNullOrEmpty(Portugese) && Portugese.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                   !string.IsNullOrEmpty(Russish) && Russish.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                   !string.IsNullOrEmpty(German) && German.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                   !string.IsNullOrEmpty(Japanish) && Japanish.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                   !string.IsNullOrEmpty(Dutsh) && Dutsh.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 |
                   !string.IsNullOrEmpty(Spanish) && Spanish.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1 ||
                   !string.IsNullOrEmpty(Italian) && Italian.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1;
        }        
        
        public bool DoesContainText(string text, Languages lang)
        {
            if (lang == Languages.All)
                return DoesContainText(text);

            return (lang == Languages.French && !string.IsNullOrEmpty(French) && French.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) ||
                   (lang == Languages.English && !string.IsNullOrEmpty(English) && English.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) ||
                   (lang == Languages.Portugese && !string.IsNullOrEmpty(Portugese) && Portugese.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) ||
                   (lang == Languages.Russish && !string.IsNullOrEmpty(Russish) && Russish.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) ||
                   (lang == Languages.German && !string.IsNullOrEmpty(German) && German.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) ||
                   (lang == Languages.Japanish && !string.IsNullOrEmpty(Japanish) && Japanish.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) ||
                   (lang == Languages.Dutsh && !string.IsNullOrEmpty(Dutsh) && Dutsh.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) ||
                   (lang == Languages.Spanish && !string.IsNullOrEmpty(Spanish) && Spanish.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1) ||
                   (lang == Languages.Italian && !string.IsNullOrEmpty(Italian) && Italian.IndexOf(text, StringComparison.CurrentCultureIgnoreCase) != -1);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            if (State == RowState.None)
                State = RowState.Dirty;

            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(property));
        }

        public abstract string GetKey();
        public abstract void Save();
    }
}