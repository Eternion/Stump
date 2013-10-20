#region License GNU GPL
// LangExtension.cs
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
using DBSynchroniser.Records.Langs;

namespace WorldEditor.Loaders.I18N
{
    public static class LangExtension
    {
        public static void SetAllLangs(this LangText record, string text)
        {
            record.French =
                record.German =
                record.English =
                record.Dutsh =
                record.Japanish = record.Spanish = record.Italian = record.Russish = record.Portugese = text;
        }

        public static void SetAllLangs(this LangTextUi record, string text)
        {
            record.French =
                record.German =
                record.English =
                record.Dutsh =
                record.Japanish = record.Spanish = record.Italian = record.Russish = record.Portugese = text;
        }

        public static string GetText(this LangText record, Languages language)
        {
            switch (language)
            {
                case Languages.French:
                    return record.French;
                case Languages.German:
                    return record.German;
                case Languages.Dutsh:
                    return record.French;
                case Languages.Italian:
                    return record.Italian;
                case Languages.English:
                    return record.English;
                case Languages.Japanish:
                    return record.Japanish;
                case Languages.Russish:
                    return record.Russish;
                case Languages.Spanish:
                    return record.Spanish;
                case Languages.Portugese:
                    return record.Portugese;
                default:
                    throw new Exception(string.Format("Language {0} not handled", language));
            }
        }

        public static string GetText(this LangTextUi record, Languages language)
        {
            switch (language)
            {
                case Languages.French:
                    return record.French;
                case Languages.German:
                    return record.German;
                case Languages.Dutsh:
                    return record.French;
                case Languages.Italian:
                    return record.Italian;
                case Languages.English:
                    return record.English;
                case Languages.Japanish:
                    return record.Japanish;
                case Languages.Russish:
                    return record.Russish;
                case Languages.Spanish:
                    return record.Spanish;
                case Languages.Portugese:
                    return record.Portugese;
                default:
                    throw new Exception(string.Format("Language {0} not handled", language));
            }
        }
    }
}