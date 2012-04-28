using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;

namespace Stump.Tools.Toolkit.Behaviors
{
    public class BindableSelectedItemBehaviour : Behavior<TreeView>
    {
        #region SelectedItem Property

        public object SelectedItem
        {
            get
            {
                return (object)GetValue(SelectedItemProperty);
            }
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        public static readonly DependencyProperty SelectedItemProperty =
        DependencyProperty.Register("SelectedItem", typeof(object), typeof(BindableSelectedItemBehaviour), new UIPropertyMetadata(null));

        #endregion

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.SelectedItemChanged += OnTreeViewSelectedItemChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            if (this.AssociatedObject != null)
            {
                this.AssociatedObject.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            }
        }

        private void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            SelectedItem = e.NewValue;
        }
    }
}