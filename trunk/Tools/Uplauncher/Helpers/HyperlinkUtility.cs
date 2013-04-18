#region License GNU GPL
// HyperlinkUtility.cs
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

using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace Uplauncher.Helpers
{
    public static class HyperlinkUtility
    {
        public static readonly DependencyProperty LaunchDefaultBrowserProperty = DependencyProperty.RegisterAttached("LaunchDefaultBrowser", typeof(bool), typeof(HyperlinkUtility), new PropertyMetadata(false, HyperlinkUtility_LaunchDefaultBrowserChanged));

        public static bool GetLaunchDefaultBrowser(DependencyObject d)
        {
            return (bool)d.GetValue(LaunchDefaultBrowserProperty);
        }

        public static void SetLaunchDefaultBrowser(DependencyObject d, bool value)
        {
            d.SetValue(LaunchDefaultBrowserProperty, value);
        }

        private static void HyperlinkUtility_LaunchDefaultBrowserChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var d = (UIElement)sender;
            if ((bool)e.NewValue)
                d.AddHandler(Hyperlink.RequestNavigateEvent, new RequestNavigateEventHandler(Hyperlink_RequestNavigateEvent));
            else
                d.RemoveHandler(Hyperlink.RequestNavigateEvent, new RequestNavigateEventHandler(Hyperlink_RequestNavigateEvent));
        }

        private static void Hyperlink_RequestNavigateEvent(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}