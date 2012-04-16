using System.Windows;

namespace Stump.Tools.QuickItemEditor
{
    public class Transition : FrameworkElement
    {
        #region TransitionState enum

        public enum TransitionState
        {
            A,
            B
        }

        #endregion

        public static readonly DependencyProperty SourceProperty = 
            DependencyProperty.Register(
            "Source",
            typeof (object),
            typeof (Transition),
            new PropertyMetadata(
                delegate(DependencyObject obj, DependencyPropertyChangedEventArgs args) { ((Transition) obj).Swap(); }));

        public static readonly DependencyProperty DisplayAProperty =
            DependencyProperty.Register(
                "DisplayA",
                typeof (object),
                typeof (Transition));

        public static readonly DependencyProperty DisplayBProperty = 
            DependencyProperty.Register(
                "DisplayB",
                typeof (object),
                typeof (Transition));

        public static readonly DependencyProperty StateProperty = 
            DependencyProperty.Register(
                "State",
                typeof (TransitionState),
                typeof (Transition),
                new PropertyMetadata(TransitionState.A));

        public object Source
        {
            get { return GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        public object DisplayA
        {
            get { return GetValue(DisplayAProperty); }
            set { SetValue(DisplayAProperty, value); }
        }

        public object DisplayB
        {
            get { return GetValue(DisplayBProperty); }
            set { SetValue(DisplayBProperty, value); }
        }

        public TransitionState State
        {
            get { return (TransitionState) GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        private void Swap()
        {
            if (State == TransitionState.A)
            {
                DisplayB = Source;
                State = TransitionState.B;
            }
            else
            {
                DisplayA = Source;
                State = TransitionState.A;
            }
        }
    }
}