#region License GNU GPL
// ControlExtensions.cs
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

using System.Windows;
using System.Windows.Interactivity;
using TriggerBase = System.Windows.Interactivity.TriggerBase;

namespace WorldEditor.Helpers
{
    public class ControlExtensions
    {
        public static StyleTriggerCollection GetTriggers(DependencyObject obj)
        {
            return (StyleTriggerCollection)obj.GetValue(TriggersProperty);
        }

        public static void SetTriggers(DependencyObject obj, StyleTriggerCollection value)
        {
            obj.SetValue(TriggersProperty, value);
        }

        public static readonly DependencyProperty TriggersProperty =
    DependencyProperty.RegisterAttached("Triggers", typeof(StyleTriggerCollection), typeof(ControlExtensions), new UIPropertyMetadata(null, OnTriggersChanged));

        static void OnTriggersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var triggers = (StyleTriggerCollection)e.NewValue;

            if (triggers != null)
            {
                var existingTriggers = Interaction.GetTriggers(d);

                foreach (var trigger in triggers)
                {
                    existingTriggers.Add((TriggerBase)trigger.Clone());
                }
            }
        }
    }
}