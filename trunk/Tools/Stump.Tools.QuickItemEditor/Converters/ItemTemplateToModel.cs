using System;
using System.Globalization;
using System.Windows.Data;
using Stump.Server.WorldServer.Database.Items.Templates;
using Stump.Tools.QuickItemEditor.Models;

namespace Stump.Tools.QuickItemEditor.Converters
{
    public class ItemTemplateToModel : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new ItemTemplateModel((ItemTemplate)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ( (ItemTemplateModel)value ).Template;
        }
    }
}