#region License GNU GPL

// EventToCommand.cs
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
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;

namespace WorldEditor.Helpers.Markups
{
    public class EventToCommand : MarkupExtension
    {
        private Type m_eventArgsType;

        public EventToCommand()
        {
        }

        public EventToCommand(string bindingCommandPath)
        {
            BindingCommandPath = bindingCommandPath;
        }

        public string BindingCommandPath
        {
            get;
            set;
        }

        // not that useful
        public ICommand Command
        {
            get;
            set;
        }

        public override object ProvideValue(IServiceProvider sp)
        {
            var pvt = sp.GetService(typeof (IProvideValueTarget)) as IProvideValueTarget;
            if (pvt != null)
            {
                var evt = pvt.TargetProperty as EventInfo;
                var doAction = GetType().GetMethod("DoAction", BindingFlags.NonPublic | BindingFlags.Instance);
                Type dlgType = null;
                if (evt != null)
                {
                    dlgType = evt.EventHandlerType;
                }
                var mi = pvt.TargetProperty as MethodInfo;
                if (mi != null)
                {
                    dlgType = mi.GetParameters()[1].ParameterType;
                }
                if (dlgType != null)
                {
                    m_eventArgsType = dlgType.GetMethod("Invoke").GetParameters()[1].ParameterType;
                    return Delegate.CreateDelegate(dlgType, this, doAction);
                }
            }
            return null;
        }

        private void DoAction(object sender, RoutedEventArgs e)
        {
            var dc = (sender as FrameworkElement).DataContext;
            if (BindingCommandPath != null)
            {
                Command = (ICommand) ParsePropertyPath(dc, BindingCommandPath);
            }
            var eventArgsType = typeof (EventCommandArgs<>).MakeGenericType(m_eventArgsType);
            var cmdParams = Activator.CreateInstance(eventArgsType, sender, e);
            if (Command != null && Command.CanExecute(cmdParams))
                Command.Execute(cmdParams);
        }

        private static object ParsePropertyPath(object target, string path)
        {
            var props = path.Split('.');
            return props.Aggregate(target, (current, prop) => current.GetType().GetProperty(prop).GetValue(current));
        }
    }

    public sealed class EventCommandArgs<TEventArgs> where TEventArgs : RoutedEventArgs
    {
        public EventCommandArgs(object sender, TEventArgs args)
        {
            Sender = sender;
            EventArgs = args;
        }

        public TEventArgs EventArgs
        {
            get;
            private set;
        }

        public object Sender
        {
            get;
            private set;
        }
    }
}