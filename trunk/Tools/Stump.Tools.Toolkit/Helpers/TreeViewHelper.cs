using System.Windows;
using System.Windows.Media;

namespace Stump.Tools.Toolkit.Helpers
{
    public class TreeViewHelper
    {
        public static T FindVisualParent<T>(UIElement child) where T : UIElement
        {
            if (child == null)
            {
                return null;
            }

            var parent = VisualTreeHelper.GetParent(child) as UIElement;

            while (parent != null)
            {
                var found = parent as T;
                if (found != null)
                {
                    return found;
                }
                else
                {
                    parent = VisualTreeHelper.GetParent(parent) as UIElement;
                }
            }

            return null;
        }
    }
}