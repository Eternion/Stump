using System;
using System.Globalization;
using System.Windows.Data;

namespace Stump.Tools.Toolkit.Converters
{
    public class ByteUnityFormatter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double bytes = (int)value;

            if (bytes > 1024 * 1024 * 1024)
                return string.Format("{0:0.00} GB", bytes / (1024 * 1024 * 1024));
            else if (bytes > 1024 * 1024)
                return string.Format("{0:0.00} MB", bytes / ( 1024 * 1024 ));
            else if (bytes > 1024)
                return string.Format("{0:0.00} KB", bytes / 1024);
            else
            {
                return string.Format("{0:0.00} bytes", bytes);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}