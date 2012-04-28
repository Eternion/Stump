using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace Stump.Tools.Toolkit.Helpers
{
    public class EventBehavior
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command", typeof (ICommand), typeof (EventBehavior),
                                                new FrameworkPropertyMetadata(null));

        public static readonly DependencyProperty EventNameProperty =
            DependencyProperty.RegisterAttached("EventName", typeof (string), typeof (EventBehavior),
                                                new FrameworkPropertyMetadata(string.Empty, OnEventNameChanged));

        public static ICommand GetCommand(DependencyObject obj)
        {
            return (ICommand) obj.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject obj, string value)
        {
            obj.SetValue(CommandProperty, value);
        }

        public static string GetEventName(DependencyObject obj)
        {
            return (string) obj.GetValue(EventNameProperty);
        }

        public static void SetEventName(DependencyObject obj, string value)
        {
            obj.SetValue(EventNameProperty, value);
        }

        private static void OnEventNameChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as UIElement;
            if (element != null)
            {
                if ((!string.IsNullOrEmpty((string) e.NewValue) && (string.IsNullOrEmpty((string) e.OldValue))))
                {
                    var eventName = (string) e.NewValue;

                    EventInfo eventInfo = element.GetType().GetEvent(eventName);
                    if (eventInfo != null)
                    {
                        SinkControlEvent(element, eventName);
                    }
                    else
                    {
                        throw new NullReferenceException(
                            string.Format("Event name {0} has not been found on this object !", eventName));
                    }
                }
            }
            else
            {
                throw new NullReferenceException("This behavior can only be installed on UIElement objects !");
            }
        }

        /// <summary>
        /// Sinks the control event.
        /// Source: http://social.msdn.microsoft.com/Forums/en-US/netfxbcl/thread/ae1b155e-1388-431c-bc0e-e7846f9368c8
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventName">Name of the event.</param>
        private static void SinkControlEvent(object sender, string eventName)
        {
            // Get the event information for the specified event.   
            EventInfo eventInfo = sender.GetType().GetEvent(eventName);

            // Get the handler type for the specified event.   
            Type handlerType = eventInfo.EventHandlerType;

            // Get the ParameterInfo collection for the event handler's invoke method.   
            if (handlerType != null)
            {
                ParameterInfo[] parameterInfos = handlerType.GetMethod("Invoke").GetParameters();

                // Get the collection of types corresponding to the event handler's parameters.  
                Type[] parameterTypes = parameterInfos.Select(p => p.ParameterType).ToArray();

                // Get the type of the AnonymousDelegateClass, and fill the generic parameters.   
                Type type = typeof (AnonymousDelegateClass<,>).MakeGenericType(parameterTypes);

                // Get the constructor for the class that takes the string value.   
                ConstructorInfo constructor = type.GetConstructor(new[] {typeof (string)});

                // Create an instance of the class, which will subscribe to the event specified.   
                object instance = constructor.Invoke(new object[] {eventName});

                // Get the Handler method, which will be the event handler for the event.   
                MethodInfo method = type.GetMethod("OnEventRaised");

                // Create a delegate of the same type of the handler type based on the Handler method.  
                Delegate eventhandler = Delegate.CreateDelegate(handlerType, instance, method);

                // Finally, add the event handler to the object specified.   
                eventInfo.AddEventHandler(sender, eventhandler);
            }
        }
    }

    /// <summary>
    /// Anonymous delegate class.
    /// </summary>
    /// <typeparam name="TSender">The type of the sender.</typeparam>
    /// <typeparam name="TEventargs">The type of the eventargs.</typeparam>
    public class AnonymousDelegateClass<TSender, TEventargs> where TEventargs : EventArgs
    {
        // This is the name of the event subscribed to.   

        public AnonymousDelegateClass(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
            private set;
        }

        // This the generic handler that is used to subscribe to the object.   
        public void OnEventRaised(TSender sender, TEventargs e)
        {
            var element = sender as UIElement;
            if (element != null)
            {
                var command = (ICommand) element.GetValue(EventBehavior.CommandProperty);
                command.Execute(e);
            }
            else
            {
                throw new NullReferenceException("This behavior can only be installed on UIElement objects !");
            }
        }
    }
}