#region License GNU GPL
// LangToFlagConverter.cs
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
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Stump.Core.I18N;

namespace WorldEditor.Helpers.Converters
{
    public class LangToFlagConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Languages))
                return new BitmapImage(new Uri("pack://application:,,,/Images/flags/all.png"));

            switch ((Languages)value)
            {
                case Languages.French:
                    return new BitmapImage(new Uri("pack://application:,,,/Images/flags/fr.png"));
                case Languages.English:
                    return new BitmapImage(new Uri("pack://application:,,,/Images/flags/gb.png"));
                case Languages.German:
                    return new BitmapImage(new Uri("pack://application:,,,/Images/flags/de.png"));
                case Languages.Portugese:
                    return new BitmapImage(new Uri("pack://application:,,,/Images/flags/pt.png"));
                case Languages.Dutsh:
                    return new BitmapImage(new Uri("pack://application:,,,/Images/flags/nl.png"));
                case Languages.Spanish:
                    return new BitmapImage(new Uri("pack://application:,,,/Images/flags/es.png"));
                case Languages.Italian:
                    return new BitmapImage(new Uri("pack://application:,,,/Images/flags/it.png"));
                case Languages.Russish:
                    return new BitmapImage(new Uri("pack://application:,,,/Images/flags/ru.png"));
                case Languages.Japanish:
                    return new BitmapImage(new Uri("pack://application:,,,/Images/flags/jp.png"));
                default: 
                    return new BitmapImage(new Uri("pack://application:,,,/Images/flags/all.png"));
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}